using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NoiseParameters", menuName = "Custom/Noise Parameters")]
public class NoiseParametersObject : ScriptableObject
{
    [SerializeField]
    private NoiseParameters m_NoiseParameters;

    public NoiseParameters NoiseParameters => m_NoiseParameters;
}

[Serializable]
public struct NoiseParameters
{
    public int Octaves;
    public float StartingFrequency;
    public float FrequencyStep;
    public float StartingAmplitude;
    public float AmplitudeStep;
    public Vector4 NoiseOffset;

    public static int SizeOf => sizeof(int) + sizeof(float) * 4 + sizeof(float) * 4;

    public float MaxSize
    {
        get
        {
            float size = 0f;
            for(int i = 0; i < Octaves; i++)
            {
                size += StartingAmplitude * (1 + AmplitudeStep * i);
            }
            return size;
        }
    }
}
