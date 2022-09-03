using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkHandler : MonoBehaviour
{
    private const int pointsPerAxis = 65;

    List<Chunk> chunks;
    Dictionary<Vector3Int, Chunk> highResolutionChunkDict;
    Dictionary<Vector3Int, ChunkData> chunkData;
    Queue<Chunk> recycledChunks;

    [SerializeField] ChunkMeshGenerator meshGenerator;
    [SerializeField] ChunkDataGenerator chunkDataGenerator;
    [SerializeField] Transform chunkHolder;

    [SerializeField] Transform lodFocusObject;
    [SerializeField] float lodFocusDistance = 10f;
    [SerializeField] int maxLOD = 3;

    [SerializeField] float initialChunkSize = 64f;

    [SerializeField] Material material;

    [SerializeField] bool drawBounds = true;

    private void Awake()
    {
        chunks = new List<Chunk>();
        highResolutionChunkDict = new Dictionary<Vector3Int, Chunk>();
        chunkData = new Dictionary<Vector3Int, ChunkData>();
        recycledChunks = new Queue<Chunk>();
    }

    private void Start()
    {
        CreateChunk(Vector3Int.zero, 0, null);
    }

    private void Update()
    {
        List<Chunk> chunksToRemove = new List<Chunk>();

        for (int i = chunks.Count - 1; i >= 0; i--)
        {
            Chunk chunk = chunks[i];

            if (chunk.isSplitted) continue;

            if (chunk.bounds.SqrDistance(lodFocusObject.position - transform.position) < lodFocusDistance * lodFocusDistance)
            {
                SplitChunk(chunk);
            }
            else if (chunk.parentChunk != null && 
                chunk.parentChunk.bounds.SqrDistance(lodFocusObject.position - transform.position) > lodFocusDistance * lodFocusDistance)
            {
                chunksToRemove.Add(chunk);
            }
        }

        foreach (Chunk chunk in chunksToRemove)
        {
            chunks.Remove(chunk);
            if (chunk.levelOfDetail == maxLOD)
            {
                highResolutionChunkDict.Remove(chunk.chunkCoords);
            }

            chunk.mesh.Clear();
            chunk.gameObject.SetActive(false);
            chunk.parentChunk.gameObject.SetActive(true);
            chunk.parentChunk.isSplitted = false;
            //recycledChunks.Enqueue(chunk);
            Destroy(chunk.gameObject);
        }
    }

    public void CreateChunkMesh(Chunk chunk)
    {
        ChunkData data = GetChunkData(chunk);
        meshGenerator.GenerateMesh(chunk, data);
    }

    private void CreateChunk(Vector3Int coord, int lod, Chunk parent)
    {
        Chunk chunk;
        if (recycledChunks.Count == 0)
        {
            chunk = InstanciateChunkInstance();
        }
        else
        {
            chunk = recycledChunks.Dequeue();
        }

        chunk.SetChunk(coord, lod, parent);
        CalculateChunkBounds(chunk);
        CreateChunkMesh(chunk);
        chunk.SetMaterial(material);
        chunk.gameObject.SetActive(true);
        chunks.Add(chunk);
    }

    private ChunkData GetChunkData(Chunk chunk)
    {
        if (chunk.levelOfDetail != maxLOD || !chunkData.TryGetValue(chunk.chunkCoords, out ChunkData data))
        {
            data = GenerateChunkData(chunk);
        }

        return data;
    }

    private ChunkData GenerateChunkData(Chunk chunk)
    {
        float[] values = chunkDataGenerator.GenerateChunkData(chunk.bounds.min, chunk.bounds.size, pointsPerAxis);
        ChunkData data = new ChunkData(chunk.chunkCoords, chunk.levelOfDetail, values);
        if (chunk.levelOfDetail == maxLOD)
        {
            chunkData.Add(chunk.chunkCoords, data);
        }

        return data;
    }

    private Chunk InstanciateChunkInstance()
    {
        GameObject chunkObject = new GameObject();
        chunkObject.transform.parent = chunkHolder.transform;
        chunkObject.transform.localPosition = Vector3.zero;
        Chunk chunk = chunkObject.AddComponent<Chunk>();
        return chunk;
    }

    private void CalculateChunkBounds(Chunk chunk)
    {
        float lodFactor = Mathf.Pow(2, chunk.levelOfDetail);
        float chunkSize = initialChunkSize / lodFactor;
        Vector3 chunkLocalCenterPos = chunkSize * (chunk.chunkCoords - (lodFactor - 1f) / 2f * Vector3.one);
        chunk.bounds = new Bounds(chunkLocalCenterPos, chunkSize * Vector3.one);
    }

    private void SplitChunk(Chunk chunk)
    {
        if (chunk.levelOfDetail >= maxLOD) return;

        for (int x = 0; x < 2; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                for (int z = 0; z < 2; z++)
                {
                    CreateChunk(2 * chunk.chunkCoords + new Vector3Int(x, y, z), chunk.levelOfDetail + 1, chunk);
                    chunk.isSplitted = true;
                    chunk.gameObject.SetActive(false);
                }
            }
        }

        chunk.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if (!drawBounds) return;

        if (Application.isPlaying)
        {
            foreach (Chunk chunk in chunks)
            {
                Gizmos.color = Color.Lerp(Color.white, Color.red, ((float)chunk.levelOfDetail - 1f) / 10f);
                Gizmos.DrawWireCube(transform.position + chunk.bounds.center, chunk.bounds.size);
            }
        }
        else
        {
            Gizmos.DrawWireCube(transform.position, initialChunkSize * Vector3.one);
        }
    }
}
