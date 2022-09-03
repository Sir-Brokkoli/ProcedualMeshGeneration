using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : ChunkMeshGenerator
{
    [SerializeField]
    private ComputeShader triangulationShader;
    private ComputeBuffer pointsBuffer;
    private ComputeBuffer triangleBuffer;
    private ComputeBuffer triangleCountBuffer;

    int numPointsPerAxis = 65;

    private const int THREAD_GROUP_SIZE = 8;

    private void Awake()
    {
        CreateBuffers();
    }

    private void OnDestroy()
    {
        ReleaseBuffers();
    }

    public override void GenerateMesh(Chunk chunk, ChunkData chunkData)
    {
        print("Start mesh generation");
        ExecuteTriangulationShader(chunkData, chunk, out int numTriangles, out Triangle[] triangles);
        print("Triangulated...");
        Mesh mesh = chunk.mesh;
        GenerateMeshFromTriangles(mesh, triangles, numTriangles);
        mesh.RecalculateNormals();
    }

    private void ExecuteTriangulationShader(ChunkData chunkData, Chunk chunk, out int numTriangles, out Triangle[] triangles)
    {
        int steps = numPointsPerAxis - 1;
        int numThreadsPerAxis = Mathf.CeilToInt(steps / (float)THREAD_GROUP_SIZE);
        CreateBuffers();

        pointsBuffer.SetData(chunkData.data);
        triangleBuffer.SetCounterValue(0);

        triangulationShader.SetBuffer(0, "data", pointsBuffer);
        triangulationShader.SetBuffer(0, "triangles", triangleBuffer);
        triangulationShader.SetInt("points_per_axis", numPointsPerAxis);
        triangulationShader.SetFloat("step_size", chunk.bounds.size.x / steps);
        triangulationShader.SetVector("offset", chunk.bounds.min);

        triangulationShader.Dispatch(0, numThreadsPerAxis, numThreadsPerAxis, numThreadsPerAxis);

        ComputeBuffer.CopyCount(triangleBuffer, triangleCountBuffer, 0);
        int[] triangleCountArray = { 0 };
        triangleCountBuffer.GetData(triangleCountArray);
        numTriangles = triangleCountArray[0];
        print(numTriangles);

        triangles = new Triangle[numTriangles];
        triangleBuffer.GetData(triangles, 0, 0, numTriangles);

        ReleaseBuffers();
    }

    private void GenerateMeshFromTriangles(Mesh mesh, Triangle[] triangles, int numTriangles)
    { 
        mesh.Clear();

        var vertices = new List<Vector3>();
        var meshTriangles = new int[numTriangles * 3];
        var vertexIndexDict = new Dictionary<Vector3, int>();

        for (int i = 0; i < numTriangles; i++)
        {
            for (int vertexIndex = 0; vertexIndex < 3; vertexIndex++)
            {
                if (vertexIndexDict.TryGetValue(triangles[i][vertexIndex], out int index))
                {
                    meshTriangles[i * 3 + vertexIndex] = index;
                }
                else
                {
                    index = vertices.Count;
                    vertices.Add(triangles[i][vertexIndex]);
                    meshTriangles[i * 3 + vertexIndex] = index;
                    vertexIndexDict.Add(triangles[i][vertexIndex], index);
                }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = meshTriangles;
    }

    private void CreateBuffers()
    {
        int numPoints = numPointsPerAxis * numPointsPerAxis * numPointsPerAxis;
        int voxelsPerAxis = numPointsPerAxis - 1;
        int numVoxels = voxelsPerAxis * voxelsPerAxis * voxelsPerAxis;
        int maxTriangleCount = numVoxels * 5;

        ReleaseBuffers();

        pointsBuffer = new ComputeBuffer(numPoints, sizeof(float));
        triangleBuffer = new ComputeBuffer(maxTriangleCount, sizeof(float) * 3 * 3, ComputeBufferType.Append);
        triangleCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
    }

    private void ReleaseBuffers()
    {
        pointsBuffer?.Release();
        triangleBuffer?.Release();
        triangleCountBuffer?.Release();
    }

    private struct Triangle
    {
#pragma warning disable 649
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;

        public Vector3 this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return a;
                    case 1:
                        return b;
                    case 2:
                        return c;
                    default:
                        throw new KeyNotFoundException();
                }
            }
        }
    }
}
