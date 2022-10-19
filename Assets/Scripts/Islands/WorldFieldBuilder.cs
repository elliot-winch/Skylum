using System;
using UnityEngine;

public class WorldFieldBuilder : FieldBuilder
{
    public NoiseParameters NoiseParameters;
    public Island[] Islands;
    public WaveObject WaveObject;

    private ComputeBuffer m_BackgroundFieldNoiseBuffer;
    private ComputeBuffer m_IslandsBuffer;
    private ComputeBuffer m_WavesBuffer;

    protected override void AssignBuffers()
    {
        base.AssignBuffers();

        m_BackgroundFieldNoiseBuffer = new ComputeBuffer(1, NoiseParameters.SizeOf);
        m_IslandsBuffer = new ComputeBuffer(Islands.Length, Island.SizeOf);
        m_WavesBuffer = new ComputeBuffer(WaveObject.Wave.Subwaves.Length, CosineWave.SizeOf);
    }

    protected override void ClearBuffers()
    {
        base.ClearBuffers();

        m_BackgroundFieldNoiseBuffer.Dispose();
        m_IslandsBuffer.Dispose();
        m_WavesBuffer.Dispose();
    }

    protected override void UpdateComputeShaderParameters()
    {
        base.UpdateComputeShaderParameters();

        m_BackgroundFieldNoiseBuffer.SetData(new NoiseParameters[] { NoiseParameters });
        m_IslandsBuffer.SetData(Islands);
        m_WavesBuffer.SetData(WaveObject.Wave.Subwaves);

        m_FieldCompute.SetBuffer(0, "noiseParameters", m_BackgroundFieldNoiseBuffer);
        m_FieldCompute.SetInt("numIslands", Islands.Length);
        m_FieldCompute.SetBuffer(0, "islands", m_IslandsBuffer);
        m_FieldCompute.SetInt("numWaves", WaveObject.Wave.Subwaves.Length);
        m_FieldCompute.SetBuffer(0, "waves", m_WavesBuffer);
    }
}
