using UnityEngine;

public abstract class MeshBuilder
{
    protected Vector3Int m_Size;
    protected int m_NumTriangles;
    protected ComputeShader m_MarchingCubes;

    public abstract Mesh Mesh { get; }

    protected MeshBuilder(Vector3Int size, int vertexCount, ComputeShader marchingCubes)
    {
        m_Size = size;
        m_NumTriangles = vertexCount;
        m_MarchingCubes = marchingCubes;
    }

    public abstract void BuildIsosurface(ComputeBuffer voxels, float isoValue, float scale);

    public abstract void Dispose();
}
