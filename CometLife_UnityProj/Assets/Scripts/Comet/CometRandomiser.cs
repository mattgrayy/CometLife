using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometRandomiser : MonoBehaviour
{
    Mesh m_deformingMesh;
    Vector3[] m_currentVertices;
    List<List<int>> weldedVertIndexes = new List<List<int>>();

    [SerializeField, Range(0.01f, 0.30f)] float randomChangeMod = 0.01f;

    void Start()
    {
        m_deformingMesh = GetComponent<MeshFilter>().mesh;
        GetComponent<MeshCollider>().sharedMesh = m_deformingMesh;
        m_currentVertices = m_deformingMesh.vertices;

        weldVerts();
        randomiseMesh();
    }

    void weldVerts()
    {
        // loop through currentVertices by index
        for (int vertIndex = 0; vertIndex < m_currentVertices.Length; vertIndex++)
        {
            // used to mark if we found an existing vert in the same position
            bool welded = false;

            // loop through the current known vert locations (welded) to see if we match
            foreach(List<int> vertList in weldedVertIndexes)
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

    void randomiseMesh()
    {
        foreach(List<int> weldedList in weldedVertIndexes)
        {
            Vector3 positionToAdd = Random.Range(-randomChangeMod, randomChangeMod) * m_currentVertices[weldedList[0]];

            foreach (int vertIndex in weldedList)
            {
                m_currentVertices[vertIndex] += positionToAdd;
            }
        }

        m_deformingMesh.vertices = m_currentVertices;
        m_deformingMesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = m_deformingMesh;
    }
}
