using UnityEngine;
using MarchingCubes;

public abstract class FieldBuilder : MonoBehaviour
{
    [SerializeField]
    protected ComputeShader m_FieldCompute = null;

    public Vector3Int Dimensions = new(64, 64, 64);
    public float GridScale = 1.0f / 64;
    public Vector3 PositionOffset;

    private ComputeBuffer m_VoxelBuffer;
    public ComputeBuffer VoxelBuffer => m_VoxelBuffer;

    private int VoxelCount => Dimensions.x * Dimensions.y * Dimensions.z;

    private bool m_Allocated = false;

    public Topic<Field> Field = new();

    private void OnDestroy()
    {
        DeallocateBuffers();
    }

    public void GeneratePoints()
    {
        AllocateBuffers();

        // Noise field update
        m_FieldCompute.SetInts("Dims", Dimensions);
        m_FieldCompute.SetFloat("Scale", GridScale);
        m_FieldCompute.SetFloat("Time", Time.time);
        m_FieldCompute.SetVector("offset", new Vector4(PositionOffset.x, PositionOffset.y, PositionOffset.z));
        m_FieldCompute.SetBuffer(0, "Voxels", m_VoxelBuffer);

        UpdateComputeShaderParameters();

        m_FieldCompute.DispatchThreads(0, Dimensions);

        //Copy points from GPU to CPU
        float[] points = new float[VoxelCount];
        m_VoxelBuffer.GetData(points);

        Field.Value = new Field(points, Dimensions, GridScale);
    }

    private void AllocateBuffers()
    {
        if (m_Allocated == false)
        {
            AssignBuffers();
            m_Allocated = true;
        }
    }

    private void DeallocateBuffers()
    {
        if (m_Allocated)
        {
            ClearBuffers();
            m_Allocated = false;
        }
    }

    protected virtual void AssignBuffers()
    {
        m_VoxelBuffer = new ComputeBuffer(VoxelCount, sizeof(float));
    }

    protected virtual void ClearBuffers()
    {
        m_VoxelBuffer.Dispose();
    }

    protected virtual void UpdateComputeShaderParameters() { }
}
