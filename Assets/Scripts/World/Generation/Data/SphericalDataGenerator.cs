using UnityEngine;
using System.Collections;

public class SphericalDataGenerator : ChunkDataGenerator
{
    private const int THREAD_GROUP_SIZE = 8;
    private const string DATA_FIELD = "data";
    private const string RADIUS_FIELD = "radius";
    private const string POINTS_PER_AXIS_FIELD = "points_per_axis";
    private const string CHUNK_SIZE_FIELD = "chunk_size";
    private const string OFFSET_FIELD = "chunk_offset";

    [SerializeField]
    private ComputeShader shader;

    float radius = 30;

    private ComputeBuffer dataBuffer;

    private void OnDestroy()
    {
        dataBuffer?.Release();
    }

    public override float[] GenerateChunkData(Vector3 position, Vector3 size, int pointsPerAxis)
    {
        int bufferSize = pointsPerAxis * pointsPerAxis * pointsPerAxis;
        dataBuffer = new ComputeBuffer(bufferSize, sizeof(float));
        int numThreadsPerAxis = Mathf.CeilToInt(pointsPerAxis / (float)THREAD_GROUP_SIZE);

        shader.SetBuffer(0, DATA_FIELD, dataBuffer);
        shader.SetFloat(RADIUS_FIELD, radius);
        shader.SetInt(POINTS_PER_AXIS_FIELD, pointsPerAxis);
        shader.SetVector(CHUNK_SIZE_FIELD, size);
        shader.SetVector(OFFSET_FIELD, position);

        shader.Dispatch(0, numThreadsPerAxis, numThreadsPerAxis, numThreadsPerAxis);

        var data = new float[pointsPerAxis * pointsPerAxis * pointsPerAxis];
        dataBuffer.GetData(data);
        dataBuffer.Release();

        return data;
    }
}
