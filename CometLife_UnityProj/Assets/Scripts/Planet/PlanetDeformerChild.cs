using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetDeformerChild : MonoBehaviour {

    PlanetDeformerParent m_parent;

    MeshCollider m_meshCollider;
    Mesh m_deformingMesh;
    Vector3[] m_currentVertices;
    List<List<int>> weldedVertIndexes = new List<List<int>>();

    public void deformMesh(Vector3 _collisionPoint, float _radius = 2)
    {
        bool changed = false;

        foreach (List<int> weldedList in weldedVertIndexes)
        {
            float dist = Vector3.Distance(m_currentVertices[weldedList[0]], _collisionPoint);

            if (dist > _radius)
                continue;

            changed = true;

            float randomisedRadius = _radius + Random.Range(-_radius / 100, _radius / 100);


            Vector3 dirToCenter = m_currentVertices[weldedList[0]].normalized;

            Debug.Log(dist + " _ " + Mathf.Log(dist, 0.3f));
            dirToCenter *= Mathf.Log(dist, 0.3f);

            float dif = (randomisedRadius - dist) / -100;
            Vector3 positionToAdd = dif * m_currentVertices[weldedList[0]];


            positionToAdd += dirToCenter;


            foreach (int vertIndex in weldedList)
            {
                m_currentVertices[vertIndex] += positionToAdd;
            }
        }

        if (!changed)
            return;

        m_deformingMesh.vertices = m_currentVertices;
        m_deformingMesh.RecalculateNormals();
        m_meshCollider.sharedMesh = m_deformingMesh;
    }

    public void setParent(PlanetDeformerParent _parent)
    {
        m_parent = _parent;
    }

    void Start()
    {
        m_deformingMesh = GetComponent<MeshFilter>().mesh;
        m_meshCollider = GetComponent<MeshCollider>();

        if (m_meshCollider == null)
        {
            m_meshCollider = gameObject.AddComponent<MeshCollider>();
            m_meshCollider.convex = true;
        }

        m_meshCollider.sharedMesh = m_deformingMesh;
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

	void OnCollisionEnter(Collision _collision)
    {
        CometMovement comet = _collision.transform.GetComponent<CometMovement>();
        if (comet != null)
        {
            if (m_parent != null)
            {
                m_parent.childHit(_collision.contacts[0].point, comet.GetComponent<SphereCollider>().radius * comet.transform.localScale.x);
                m_parent.removeComet(comet);
            }

            Destroy(comet.gameObject);
        }
    }
}
