using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetDeformer : MonoBehaviour
{
    Mesh m_deformingMesh;
    Vector3[] m_currentVertices;
    List<List<int>> weldedVertIndexes = new List<List<int>>();

    void Start()
    {
        m_deformingMesh = GetComponent<MeshFilter>().mesh;
        GetComponent<MeshCollider>().sharedMesh = m_deformingMesh;
        m_currentVertices = m_deformingMesh.vertices;

        weldVerts();
    }

    void weldVerts()
    {
        // loop through currentVertices by index
        for (int vertIndex = 0; vertIndex < m_currentVertices.Length; vertIndex++)
        {
            // used to mark if we found an existing vert in the same position
            bool welded = false;

            // loop through the current known vert locations (welded) to see if we match
            foreach (List<int> vertList in weldedVertIndexes)
            {
                // if one exists, add ourselves to that list
                if (m_currentVertices[vertList[0]] == m_currentVertices[vertIndex])
                {
                    vertList.Add(vertIndex);
                    welded = true;
                    break;
                }
            }

            // if we still aren't welded, make a new weld group for future verts to compare
            if (!welded)
            {
                List<int> newList = new List<int>();
                newList.Add(vertIndex);
                weldedVertIndexes.Add(newList);
            }
        }
    }

    void deformMesh(Vector3 _collisionPoint, float _velocity)
    {
        foreach (List<int> weldedList in weldedVertIndexes)
        {
            if (Vector3.Distance(m_currentVertices[weldedList[0]], _collisionPoint) > 20)
                continue;

            Vector3 positionToAdd = Random.Range(-0.05f, -0.01f) * m_currentVertices[weldedList[0]];

            foreach (int vertIndex in weldedList)
            {
                m_currentVertices[vertIndex] += positionToAdd;
            }
        }

        m_deformingMesh.vertices = m_currentVertices;
        m_deformingMesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = m_deformingMesh;
    }

    void OnCollisionEnter(Collision _collision)
    {
        CometMovement comet = _collision.transform.GetComponent<CometMovement>();
        if (comet != null)
        {
            GetComponent<Planet>().removeComet(comet);
            deformMesh(_collision.contacts[0].point, _collision.relativeVelocity.magnitude);
            comet.stopMovement();
        }
    }
}
