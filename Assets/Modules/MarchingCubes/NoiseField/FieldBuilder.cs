using UnityEngine;
using MarchingCubes;

public abstract class FieldBuilder : MonoBehaviour
{
    [SerializeField]
    protected ComputeShader m_FieldCompute = null;

    public float GridScale = 1.0f / 64;
    public Vector3 PositionOffset;

    private ComputeBuffer m_VoxelBuffer;
    public ComputeBuffer VoxelBuffer => m_VoxelBuffer;

    private int VoxelCount => Dimensions.Value.x * Dimensions.Value.y * Dimensions.Value.z;

    private bool m_Allocated = false;

    public Topic<Field> Field { get; } = new();
    public Topic<Vector3Int> Dimensions { get; } = new(new(64, 64, 64));


    private void OnDestroy()
    {
        DeallocateBuffers();
    }

    public void GeneratePoints()
    {
        AllocateBuffers();

        // Noise field update
        m_FieldCompute.SetInts("Dims", Dimensions.Value);
        m_FieldCompute.SetFloat("Scale", GridScale);
        m_FieldCompute.SetFloat("Time", Time.time);
        m_FieldCompute.SetVector("offset", new Vector4(PositionOffset.x, PositionOffset.y, PositionOffset.z));
        m_FieldCompute.SetBuffer(0, "Voxels", m_VoxelBuffer);

        UpdateComputeShaderParameters();

        m_FieldCompute.DispatchThreads(0, Dimensions.Value);

        //Copy points from GPU to CPU
        float[] points = new float[VoxelCount];
        m_VoxelBuffer.GetData(points);

        Field.Value = new Field(points, Dimensions.Value, GridScale);
    }

    protected void AllocateBuffers()
    {
        if (m_Allocated == false)
        {
            AssignBuffers();
            m_Allocated = true;
        }
    }

    protected void DeallocateBuffers()
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
