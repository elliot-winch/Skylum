using UnityEngine;

public class FieldPlane : MonoBehaviour
{
    private enum Dimension
    {
        X,
        Y,
        Z
    };

    [SerializeField]
    private FieldBuilder m_FieldBuilder;
    [SerializeField]
    private int m_Level;
    [SerializeField]
    private Dimension m_Dimension;
    [SerializeField]
    private float m_TargetValue;

    [Header("Display")]
    [SerializeField]
    private Color m_MinInsideColor;
    [SerializeField]
    private Color m_MaxInsideColor;
    [SerializeField]
    private Color m_MinOutsideColor;
    [SerializeField]
    private Color m_MaxOutsideColor;
    [SerializeField]
    private MeshRenderer m_TextureObject;

    private Texture2D m_Texture;

    private Vector3Int Dimensions => m_FieldBuilder.Dimensions;

    private void Start()
    {
        m_FieldBuilder.Field.Subscribe(Visualise);
    }

    #region Indices
    private int PlaneToCubeIndex(int planeIndex)
    {
        //Unflatten texture index
        int u = planeIndex / GetTextureWidth();
        int v = planeIndex % GetTextureWidth();

        int x = 0, y = 0, z = 0;
        switch (m_Dimension)
        {
            case Dimension.X:
                x = Mathf.Clamp(m_Level, 0, Dimensions.x - 1);
                y = u;
                z = v;
                break;
            case Dimension.Y:
                x = v;
                y = Mathf.Clamp(m_Level, 0, Dimensions.y - 1);
                z = u;
                break;
            case Dimension.Z:
                x = u;
                y = v;
                z = Mathf.Clamp(m_Level, 0, Dimensions.z - 1);
                break;
        }

        return x + GetTextureWidth() * (y + GetTextureHeight() * z);
    }

    private int GetTextureWidth()
    {
        switch (m_Dimension)
        {
            case Dimension.X:
                return Dimensions.y;
            case Dimension.Y:
                return Dimensions.z;
            case Dimension.Z:
                return Dimensions.x;
        }
        return -1;
    }

    private int GetTextureHeight()
    {
        switch (m_Dimension)
        {
            case Dimension.X:
                return Dimensions.z;
            case Dimension.Y:
                return Dimensions.x;
            case Dimension.Z:
                return Dimensions.y;
        }
        return -1;
    }

    #endregion

    private void Visualise(Field field)
    {
        CreateTexture();

        if(field == null)
        {
            return;
        }

        float min = float.MaxValue;
        float max = float.MinValue;

        foreach (float p in field.Points)
        {
            if (p < min)
            {
                min = p;
            }

            if (p > max)
            {
                max = p;
            }
        }

        DrawTexture(field.Points, min, max);
    }

    private void DrawTexture(float[] points, float min, float max)
    {
        Color[] textureColors = new Color[GetTextureWidth() * GetTextureHeight()];

        for (int planeIndex = 0; planeIndex < textureColors.Length; planeIndex++)
        {
            int cubeIndex = PlaneToCubeIndex(planeIndex);

            float pointValue = points[cubeIndex];

            float normalisedValue = Mathf.InverseLerp(min, max, pointValue);

            Color c;

            if (pointValue > m_TargetValue)
            {
                c = Color.Lerp(m_MinInsideColor, m_MaxInsideColor, normalisedValue);
            }
            else
            {
                c = Color.Lerp(m_MinOutsideColor, m_MaxOutsideColor, normalisedValue);
            }

            textureColors[planeIndex] = c;
        }

        m_Texture.SetPixels(textureColors);
        m_Texture.Apply();
    }

    private void CreateTexture()
    {
        if (m_Texture == null || m_Texture.width != GetTextureWidth() || m_Texture.height != GetTextureHeight())
        {
            m_Texture = new Texture2D(GetTextureWidth(), GetTextureHeight())
            {
                filterMode = FilterMode.Point
            };
            m_TextureObject.material.SetTexture("_MainTex", m_Texture);
        }
    }
}
