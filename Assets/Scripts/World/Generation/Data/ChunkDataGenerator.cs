using UnityEngine;
using System.Collections;

public abstract class ChunkDataGenerator : MonoBehaviour
{
    public abstract float[] GenerateChunkData(Vector3 position, Vector3 size, int pointsPerAxis);
}
