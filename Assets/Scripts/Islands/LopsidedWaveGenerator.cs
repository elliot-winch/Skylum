using UnityEditor;
using UnityEngine;

public class LopsidedWaveGenerator : MonoBehaviour
{
    [SerializeField]
    private int n;

    public void SaveWave()
    {
        WaveObject waveObj = ScriptableObject.CreateInstance<WaveObject>();
        waveObj.Wave = CalculateWaves();

        string fileName = string.Format("Assets/Waves/N{0}.asset", n);
        AssetDatabase.CreateAsset(waveObj, fileName);
        AssetDatabase.SaveAssets();
    }

    public Wave CalculateWaves()
    {
        CosineWave[] waves = new CosineWave[n];
       
        float b = Permutation(2 * n, n);

        for (int k = 1; k <= n; k++)
        {
            float a = Permutation(2 * n, n - k);
            float amp = a / b;
            amp /= k;

            CosineWave wave = new()
            {
                Frequency = k,
                Amplitude = amp
            };

            waves[k - 1] = wave;
        }

        return new Wave()
        {
            Subwaves = waves
        };
    }

    private int Factorial(int n)
    {
        int value = 1;
        for(int i = 1; i <= n; i++)
        {
            value *= value;
        }
        return value;
    }

    private int Permutation(int n, int r)
    {
        return Factorial(n) / Factorial(n - r);
    }
}
