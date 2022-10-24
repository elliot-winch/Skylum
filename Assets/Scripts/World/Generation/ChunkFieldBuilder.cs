using System;
using UnityEngine;

public class ChunkFieldBuilder : FieldBuilder
{
    public NoiseParameters NoiseParameters;
    public Island[] Islands;
    public AnimationCurve IslandShape;
    public int NumIslandShapeSamples = 64;

    private ComputeBuffer m_BackgroundFieldNoiseBuffer;
    private ComputeBuffer m_IslandsBuffer;
    private ComputeBuffer m_SamplesBuffer;

    public void SetResolution(int samples)
    {
        Dimensions.Value = new Vector3Int(samples, samples, samples);
        GridScale = 1 / (float)samples;

        //Buffers will now have different sizes
        DeallocateBuffers();
    }

    protected override void AssignBuffers()
    {
        base.AssignBuffers();

        m_BackgroundFieldNoiseBuffer = new ComputeBuffer(1, NoiseParameters.SizeOf);
        m_IslandsBuffer = new ComputeBuffer(Islands.Length, Island.SizeOf);
        m_SamplesBuffer = new ComputeBuffer(NumIslandShapeSamples, sizeof(float));
    }

    protected override void ClearBuffers()
    {
        base.ClearBuffers();

        m_BackgroundFieldNoiseBuffer.Dispose();
        m_IslandsBuffer.Dispose();
        m_SamplesBuffer.Dispose();
    }

    protected override void UpdateComputeShaderParameters()
    {
        base.UpdateComputeShaderParameters();

        float[] samples = SampleAnimationCurve();

        m_BackgroundFieldNoiseBuffer.SetData(new NoiseParameters[] { NoiseParameters });
        m_IslandsBuffer.SetData(Islands);
        m_SamplesBuffer.SetData(samples);

        m_FieldCompute.SetBuffer(0, "noiseParameters", m_BackgroundFieldNoiseBuffer);
        m_FieldCompute.SetInt("numIslands", Islands.Length);
        m_FieldCompute.SetBuffer(0, "islands", m_IslandsBuffer);
        m_FieldCompute.SetInt("numSamples", samples.Length);
        m_FieldCompute.SetBuffer(0, "islandShapeSamples", m_SamplesBuffer);
    }

    private float[] SampleAnimationCurve()
    {
        float[] samples = new float[NumIslandShapeSamples];

        for(int i = 0; i < NumIslandShapeSamples; i++)
        {
            samples[i] = IslandShape.Evaluate(i / (float)NumIslandShapeSamples);
        }

        return samples;
    }
}
