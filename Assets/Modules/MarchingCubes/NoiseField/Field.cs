using UnityEngine;

public class Field
{
    public float[] Points { get; private set; }
    public Vector3Int Size { get; private set; }
    public float Scale { get; private set; }

    public Field(float[] points, Vector3Int size, float scale)
    {
        Points = points;
        Size = size;
        Scale = scale;
    }

    public Vector3Int FlatIndexToCubeIndex(int index)
    {
        int z = index / (Size.x * Size.y);
        index -= (z * Size.x * Size.y);
        int y = index / Size.x;
        int x = index % Size.x;
        return new Vector3Int(x, y, z);
    }

    public int CubeIndexToFlatIndex(Vector3Int index)
    {
        return index.x + Size.x * (index.y + Size.y * index.z);
    }

    public Vector3 CubeIndexToPosition(Vector3Int index)
    {
        return Scale * new Vector3(index.x, index.y, index.z);
    }

    public Vector3Int PositionToCubeIndex(Vector3 position)
    {
        Vector3 index = position / Scale;
        return new Vector3Int(
                Mathf.FloorToInt(index.x),
                Mathf.FloorToInt(index.y),
                Mathf.FloorToInt(index.z)
            );
    }

    private float CubeIndexToValue(Vector3Int cubeIndex)
    {
        int index = CubeIndexToFlatIndex(cubeIndex);
        if(index >= Points.Length)
        {
            Debug.LogErrorFormat("Field: Trying to find value outside of field. Index {0} out of Points {1}", index, Points.Length);
        }
        return Points[index];
    }

    public float GetNearestValue(Vector3 position)
    {
        return CubeIndexToValue(PositionToCubeIndex(position));
    }

    public Vector3 GetNearestGradient(Vector3 position)
    {
        Vector3Int cubeIndex = PositionToCubeIndex(position);

        return new Vector3()
        {
            x = Gradient(cubeIndex, new Vector3Int(1, 0, 0)),
            y = Gradient(cubeIndex, new Vector3Int(0, 1, 0)),
            z = Gradient(cubeIndex, new Vector3Int(0, 0, 1)),
        };
    }

    private float Gradient(Vector3Int cubeIndex, Vector3Int direction)
    {
        return CubeIndexToValue(cubeIndex + direction) - CubeIndexToValue(cubeIndex);
    }
}
