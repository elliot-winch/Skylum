using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LopsidedWaveGenerator))]
public class LopsidedWaveGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LopsidedWaveGenerator waveGen = (LopsidedWaveGenerator)target;
        if (GUILayout.Button("Generate"))
        {
            waveGen.SaveWave();
        }
    }
}
