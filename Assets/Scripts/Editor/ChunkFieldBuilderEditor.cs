using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldBuilder))]
public class ChunkFieldBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Regenerate"))
        {
            FieldBuilder fb = (FieldBuilder)target;
            fb.GeneratePoints();
        }
    }
}
