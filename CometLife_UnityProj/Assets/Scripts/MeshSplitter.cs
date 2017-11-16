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

        int curPoint = 0;

        List<Vector3> splitVerts = new List<Vector3>();
        List<int> splitIndices = new List<int>();

        foreach(int i in m_meshTris)
        {
            curPoint++;

            splitVerts.Add(m_mesh.vertices[i]);
            splitIndices.Add(curPoint-1);

            if (curPoint == _trianglesPerChunk * 3)
            {
                splitVertGroupToChild(splitVerts, splitIndices);
                splitVerts.Clear();
                splitIndices.Clear();
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

    void splitVertGroupToChild(List<Vector3> _verts, List<int> _inds)
    {
        Vector3[] vertices = new Vector3[_verts.Count];
        for (int i = 0; i < vertices.Length; i++)
            vertices[i] = _verts[i];

        int[] indices = new int[_inds.Count];
        for (int i = 0; i < indices.Length; i++)
            indices[i] = _inds[i];

        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();

        GameObject newChild = new GameObject("SplitChild", typeof(MeshFilter), typeof(MeshRenderer));
        newChild.transform.position = transform.position;
        newChild.transform.parent = transform;
        newChild.GetComponent<MeshFilter>().mesh = msh;
        newChild.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
    }
}