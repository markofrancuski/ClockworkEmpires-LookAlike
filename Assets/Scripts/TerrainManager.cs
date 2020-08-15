using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TerrainManager : MonoBehaviour
{
    public static int MeshNumber = 0;
    public static TerrainManager Instance;

    [SerializeField] private Mesh _mesh;
    [SerializeField] private Vector3[] _vertices;
    [SerializeField] private int[] _triangles;

    [SerializeField] private int _xSize = 20;
    [SerializeField] private int _zSize = 20;
    [SerializeField] private float _flatTerrainChance;
    [SerializeField] private float _minHeight;


    public bool ShouldGenerateNewPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        CreateShape();
        UpdateMesh();

        if (ShouldGenerateNewPrefab)
        {
            GameObject go = new GameObject("Terrain Mesh");
            MeshFilter goMeshFilter = go.AddComponent<MeshFilter>();
            MeshRenderer goMeshRenderer = go.AddComponent<MeshRenderer>();
            go.AddComponent<MeshCollider>();
            go.AddComponent<MeshData>().SetData(_vertices, _triangles);

            goMeshFilter.mesh = _mesh;

            PrefabUtility.SaveAsPrefabAsset( go, $"Assets/Prefabs/Terrain/Mesh_{MeshNumber}.prefab");
            MeshNumber++;
        }

    }

    public Mesh GetMesh() => _mesh;

    private void CreateShape()
    {
        _vertices = new Vector3[(_xSize + 1) * (_zSize + 1)];

        for (int i = 0, z = 0; z <= _zSize; z++)
        {
            for (int x = 0; x <= _xSize; x++)
            {
                if(_vertices[i] != null)
                {
                    float y = 0;
                    //y = Mathf.PerlinNoise(x * 0.3f, z * 0.3f) * 2f;

                    int chance = Random.Range(0, 101);
                    if (chance > _flatTerrainChance)
                    {
                        y = Mathf.Round(Random.Range(0, _minHeight));
                        //float rndHeight = Random.Range(_minHeight, _maxHeight);
                         //y = Mathf.PerlinNoise(x * rndHeight, z * rndHeight) * 2f;
                    }
                    _vertices[i] = new Vector3(x, y, z);
                }

                i++;
            }
        }

        _triangles = new int[_xSize * _zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < _zSize; z++)
        {
            for (int x = 0; x < _xSize; x++)
            {
                _triangles[tris + 0] = vert + 0;
                _triangles[tris + 1] = vert + _xSize + 1;
                _triangles[tris + 2] = vert + 1;

                _triangles[tris + 3] = vert + 1;
                _triangles[tris + 4] = vert + _xSize + 1;
                _triangles[tris + 5] = vert + _xSize + 2;

                vert++;
                tris += 6;
            }

            vert++;
        }
    }

    private void UpdateMesh()
    {
        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
    }

    public (Vector3, int) GetVerticeInfo(Vector3 point)
    {
        Vector3 vertice = Vector3.zero;
        int index = 0;

        //Debug.Log(point);
        // Find Index
        index = ((point.x +  point.x != 0 ? 1 : 0 ) *  _zSize) + (int) point.z;
        //Debug.Log(index);
        return (vertice, index);

    } 

    public void UpdateVerticeHeight(int index, float toHeight)
    {

        // TODO: Start Coroutiune to see if there are more vertices gonna be update in mean time if they do, batch them together ( call update mesh once instead of few times for each update).
    }



    private void OnDrawGizmos()
    {
        for (int i = 0; i < _vertices.Length; i++)
        {
            if (i == 0 || i == 1 || i == 4 ) Gizmos.color = Color.red;
            else Gizmos.color = Color.gray;
            Gizmos.DrawSphere(_vertices[i], 0.1f);
        }
    }

}
