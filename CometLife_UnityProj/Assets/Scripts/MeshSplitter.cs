using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSplitter : MonoBehaviour {

    Mesh m_mesh;
    int[] m_meshTris;

    [SerializeField] bool m_splitOnStart = true;
    [SerializeField] int m_trianglesPerChunkFallback = 50;

    void Start()
    {
        m_mesh = GetComponent<MeshFilter>().mesh;
        m_meshTris = m_mesh.GetTriangles(0);

        if(m_splitOnStart)
            splitMesh();
    }

    void splitMesh(int _trianglesPerChunk = -1, bool _setupDeformer = true)
    {
        if (_trianglesPerChunk == -1)
            _trianglesPerChunk = m_trianglesPerChunkFallback;

        Vector3[] meshVerts = m_mesh.vertices;
        int curPoint = 0;

        Vector3[] splitVerts2 = new Vector3[_trianglesPerChunk*3];
        int[] splitIndices2 = new int[_trianglesPerChunk*3];

        foreach(int i in m_meshTris)
        {
            splitVerts2[curPoint] = meshVerts[i];
            splitIndices2[curPoint] = curPoint;

            curPoint++;
            if (curPoint == _trianglesPerChunk * 3)
            {
                splitVertGroupToChild(splitVerts2, splitIndices2);
                curPoint = 0;
            }
        }

        if (_setupDeformer)
        {
            PlanetDeformerParent deformer = GetComponent<PlanetDeformerParent>();

            if (deformer == null)
                deformer = gameObject.AddComponent<PlanetDeformerParent>();

            deformer.setupChildren();
        }

        Destroy(GetComponent<MeshRenderer>());
        Destroy(GetComponent<MeshFilter>());
        Destroy(this);
    }

    void splitVertGroupToChild(Vector3[] _verts , int[] _inds)
    {
        Mesh msh = new Mesh();
        msh.vertices = _verts;
        msh.triangles = _inds;

        msh.RecalculateNormals();
        msh.RecalculateBounds();

        GameObject newChild = new GameObject("SplitChild", typeof(MeshFilter), typeof(MeshRenderer));
        newChild.transform.SetParent(transform, false);
        newChild.GetComponent<MeshFilter>().mesh = msh;
        newChild.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
    }
}