using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    public Vector3Int chunkCoords { get; private set; }
    public int levelOfDetail { get; private set; }
    public Bounds bounds { get; set; }
    public bool hasCollider { get; private set; }
    public Mesh mesh { get; private set; }
    public bool isSplitted { get; set; }
    public Chunk parentChunk { get; private set; }

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshCollider = gameObject.AddComponent<MeshCollider>();

        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshFilter.sharedMesh = mesh;
    }

    public void SetChunk(Vector3Int coords, int lod, Chunk parent)
    {
        this.gameObject.name = $"Chunk[{lod}] ({coords.x},{coords.y},{coords.z})";
        this.chunkCoords = coords;
        this.levelOfDetail = lod;
        this.parentChunk = parent;
    }

    public void SetMaterial(Material material)
    {
        meshRenderer.material = material;
    }
}
