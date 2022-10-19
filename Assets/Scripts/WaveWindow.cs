using UnityEngine;
using UnityEditor;

public class WaveWindow : EditorWindow
{
	[MenuItem("Window/CurveDrawingTest")]
	public static void ShowWindow()
	{
		GetWindow<WaveWindow>().Show();
	}

	void OnGUI()
	{
		EditorGUILayout.BeginVertical();

		var rect = new Rect(0, 0, position.width, position.height);
		EditorGUI.DrawRect(rect, Color.black);

		float yMin = -1;
		float yMax = 1;
		float step = 1 / position.width;

		Vector3 prevPos = new Vector3(0, curveFunc(0), 0);
		for (float t = step; t < 1; t += step)
		{
			Vector3 pos = new Vector3(t, curveFunc(t), 0);
			Handles.DrawLine(
				new Vector3(rect.xMin + prevPos.x * rect.width, rect.yMax - ((prevPos.y - yMin) / (yMax - yMin)) * rect.height, 0),
				new Vector3(rect.xMin + pos.x * rect.width, rect.yMax - ((pos.y - yMin) / (yMax - yMin)) * rect.height, 0));

			prevPos = pos;
		}

		EditorGUILayout.EndVertical();
	}

	float curveFunc(float t)
	{
		return Mathf.Sin(t * 2 * Mathf.PI);
	}
}