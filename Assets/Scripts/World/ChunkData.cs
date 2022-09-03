using UnityEngine;
using UnityEditor;

public class ChunkData
{
    public Vector3Int coords { get; private set; }
    public int lod { get; private set; }
    public float[] data { get; set; }

    public ChunkData(Vector3Int coords, int lod, float[] data)
    {
        this.coords = coords;
        this.lod = lod;
        this.data = data;
    }
}
