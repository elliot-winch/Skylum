using System;
using UnityEngine;

public class IslandFieldBuilder : FieldBuilder
{
    //Mirror of struct in Island.compute
    [Serializable]
    struct Point
    {
        public float x;
        public float y;

        public static int SizeOf => sizeof(float) * 2;
    }

    [SerializeField]
    private Point[] m_Keypoints;
    [SerializeField]
    private float m_Girth;

    private ComputeBuffer m_KeypointBuffer;

    protected override void AssignBuffers()
    {
        base.AssignBuffers();

        m_KeypointBuffer = new ComputeBuffer(m_Keypoints.Length, Point.SizeOf);
    }

    protected override void ClearBuffers()
    {
        base.ClearBuffers();

        if(m_KeypointBuffer != null)
        {
            m_KeypointBuffer.Dispose();
        }
    }

    protected override void UpdateComputeShaderParameters()
    {
        base.UpdateComputeShaderParameters();

        m_KeypointBuffer.SetData(m_Keypoints);

        m_FieldCompute.SetFloat("girth", m_Girth);
        m_FieldCompute.SetInt("numKeypoints", m_Keypoints.Length);
        m_FieldCompute.SetBuffer(0, "Keypoints", m_KeypointBuffer);
    }
}
