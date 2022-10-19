using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Custom/Wave", order = 1)]
public class WaveObject : ScriptableObject
{
    public Wave Wave;
}

[Serializable]
public class Wave
{
    public CosineWave[] Subwaves;

    public float Evaluate(float angle)
    {
        float size = 0;
        for (int i = 0; i < Subwaves.Length; i++)
        {
            size += Subwaves[i].Evaulate(angle);
        }

        return size;
    }
}

[Serializable]
public struct CosineWave
{
    public float Frequency;
    public float Amplitude;
    public float Phase;
    public float DC;

    public static int SizeOf => sizeof(float) * 4;

    public float Evaulate(float angle)
    {
        return Mathf.Cos(angle * Frequency + Phase) * Amplitude + DC;
    }
}
