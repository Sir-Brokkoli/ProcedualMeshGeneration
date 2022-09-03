using UnityEngine;
using System.Collections;

public abstract class ChunkMeshGenerator : MonoBehaviour
{
    public abstract void GenerateMesh(Chunk chunk, ChunkData chunkData);
}
