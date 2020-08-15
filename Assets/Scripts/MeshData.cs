using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData : MonoBehaviour
{
    [SerializeField] private Mesh _mesh;
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private Vector3[] _vertices;
    [SerializeField] private int[] _trinagles;

    // Start is called before the first frame update
    void Start()
    {
        _mesh = new Mesh();
        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _trinagles;
        if (_meshFilter == null) gameObject.GetComponent<MeshFilter>();
        _meshFilter.mesh = _mesh;
    }

    public void SetData(Vector3[] vertices, int[] trinagles)
    {
        _vertices = vertices;
        _trinagles = trinagles;
    }

}
