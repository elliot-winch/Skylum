using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaveObject))]
public class WaveEditor : Editor
{
    public float height = 100;

	public Rect ValueRange = new()
	{
		min = new Vector3(-Mathf.PI, -1),
		max = new Vector3(Mathf.PI, 1)
	};

	public Color colorA = Color.cyan;
	public Color colorB = Color.magenta;
	public Color totalColor = Color.red;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

		WaveObject waveObj = (WaveObject)target;
		Wave wave = waveObj?.Wave;

		if (wave == null)
        {
			return;
        }

		EditorGUILayout.BeginVertical();

		var rect = new Rect(0, 400, EditorGUIUtility.currentViewWidth, height);
		EditorGUI.DrawRect(rect, Color.black);

		for(int i = 0; i < wave.Subwaves.Length; i++)
		{
			//Color color = Color.Lerp(colorA, colorB, i / (float)wave.Subwaves.Length);
			DrawWave(new Wave() { Subwaves = new CosineWave[] { wave.Subwaves[i] } }, ValueRange, rect, Color.white);
		}

		DrawWave(wave, ValueRange, rect, Color.red);

		EditorGUILayout.EndVertical();
	}

	private void DrawWave(Wave wave, Rect valueBounds, Rect graphBounds, Color color)
    {
		Handles.color = color;

		Vector3 prevPos = RectToRect(new(valueBounds.xMin, wave.Evaluate(valueBounds.xMin)), valueBounds, graphBounds);

		float step = valueBounds.width / graphBounds.width;

		for (float xPos = valueBounds.xMin; xPos < valueBounds.xMax; xPos+=step)
		{
			Vector3 pos = RectToRect(new(xPos, wave.Evaluate(xPos)), valueBounds, graphBounds);

			Handles.DrawLine(prevPos, pos);

			prevPos = pos;
		}
    }

	private Vector3 RectToRect(Vector3 value, Rect rectA, Rect rectB)
    {
		float xPerc = Mathf.InverseLerp(rectA.xMin, rectA.xMax, value.x);
		float xPos = Mathf.Lerp(rectB.xMin, rectB.xMax, xPerc);
		float yPerc = Mathf.InverseLerp(rectA.yMin, rectA.yMax, value.y);
		float yPos = Mathf.Lerp(rectB.yMin, rectB.yMax, yPerc);
		return new(xPos, yPos);
	}
}
