using UnityEngine;
using System.Collections;

public class FractalDataGenerator : ChunkDataGenerator
{
    private const int THREAD_GROUP_SIZE = 8;

    [SerializeField]
    private ComputeShader shader;

    [SerializeField]
    private float noiseWeight = 1;
    [SerializeField]
    private float radius = 30;
    [SerializeField]
    private float amplitude = 10;
    [SerializeField]
    private float scale = 10;
    [SerializeField]
    private int octaves = 6;

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

        shader.SetBuffer(0, "data", dataBuffer);
        shader.SetFloat("radius", radius);
        shader.SetInt("points_per_axis", pointsPerAxis);
        shader.SetVector("chunk_size", size);
        shader.SetVector("chunk_offset", position);

        shader.SetFloat("noise_weight", noiseWeight);
        shader.SetFloat("amplitude", amplitude);
        shader.SetFloat("scale", scale);
        shader.SetInt("octaves", octaves);

        shader.Dispatch(0, numThreadsPerAxis, numThreadsPerAxis, numThreadsPerAxis);

        var data = new float[pointsPerAxis * pointsPerAxis * pointsPerAxis];
        dataBuffer.GetData(data);
        dataBuffer.Release();

        return data;
    }
}
