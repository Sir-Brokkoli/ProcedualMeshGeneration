using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMeshTest : MonoBehaviour
{
    [Range(-1, 1)] public float vertex1;
    [Range(-1, 1)] public float vertex2;
    [Range(-1, 1)] public float vertex3;
    [Range(-1, 1)] public float vertex4;
    [Range(-1, 1)] public float vertex5;
    [Range(-1, 1)] public float vertex6;
    [Range(-1, 1)] public float vertex7;
    [Range(-1, 1)] public float vertex8;

    public ComputeShader triangulationShader;
    public ComputeShader waterShader;

    public Material material;

    private void OnValidate()
    {
        ComputeBuffer pointsBuffer = new ComputeBuffer(8, sizeof(float));
        ComputeBuffer triangleBuffer = new ComputeBuffer(5, sizeof(float) * 3 * 3, ComputeBufferType.Append);
        ComputeBuffer triangleCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);

        pointsBuffer.SetData(new float[8] {vertex1, vertex2, vertex3, vertex4, vertex5, vertex6, vertex7, vertex8});

        triangulationShader.SetBuffer(0, "data", pointsBuffer);
        triangulationShader.SetBuffer(0, "triangles", triangleBuffer);
        triangulationShader.SetInt("points_per_axis", 2);
        triangulationShader.SetFloat("step_size", 2);
        triangulationShader.SetVector("offset", -1 * Vector3.one);

        triangulationShader.Dispatch(0, 1, 1, 1);

        ComputeBuffer.CopyCount(triangleBuffer, triangleCountBuffer, 0);
        int[] triangleCountArray = { 0 };
        triangleCountBuffer.GetData(triangleCountArray);
        int numTriangles = triangleCountArray[0];

        var triangles = new Triangle[numTriangles];
        triangleBuffer.GetData(triangles, 0, 0, numTriangles);

        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh.Clear();

        var vertices = new List<Vector3>();
        var meshTriangles = new int[numTriangles * 3];

        for (int i = 0; i < numTriangles; i++)
        {
            for (int vertexIndex = 0; vertexIndex < 3; vertexIndex++)
            {
                vertices.Add(triangles[i][vertexIndex]);
                meshTriangles[i * 3 + vertexIndex] = vertices.Count - 1;
            }
        }

        filter.mesh.vertices = vertices.ToArray();
        filter.mesh.triangles = meshTriangles;

        GetComponent<MeshRenderer>().material = this.material;

        pointsBuffer.Release();
        triangleBuffer.Release();
        triangleCountBuffer.Release();
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
