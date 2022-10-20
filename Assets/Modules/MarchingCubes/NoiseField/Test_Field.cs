using UnityEngine;

public class Test_Field : MonoBehaviour
{
    private float[] m_Points = new float[]
    {
            1,
            1,
            1,
            1,
            2,
            2,
            2,
            2,
            3,
            3,
            3,
            3,
            4,
            4,
            4,
            4,
    };
    private Vector3Int m_Size = new Vector3Int(4, 2, 2);
    private float m_Scale = 1f;

    private Field m_Field;

    // Start is called before the first frame update
    void Start()
    {
        Test();
    }

    public void Test()
    {
        m_Field = new Field(m_Points, m_Size, m_Scale);

        Debug.LogFormat("Points Count: {0}, Size: {1}, Scale: {2}", m_Field.Points.Length, m_Field.Size, m_Field.Scale);

        Test_FlatCube(new Vector3Int(0, 0, 0));
        Test_FlatCube(new Vector3Int(3, 1, 1));
        Test_FlatCube(new Vector3Int(15, 12, 10));
        Test_FlatCube(new Vector3Int(15, -12, 10));

        Test_PosCube(new Vector3Int(0, 0, 0));
        Test_PosCube(new Vector3Int(1, 0, 0));
        Test_PosCube(new Vector3Int(3, 1, 1));
        Test_PosCube(new Vector3Int(13, 11, -10));
        Test_PosCube(new Vector3Int(-3, -1, 1));

        Test_Gradient(new Vector3(0.5f, 0.5f, 0.5f));
        Test_Gradient(new Vector3(1.5f, 0.5f, 0.5f));
        Test_Gradient(new Vector3(-0.5f, -0.5f, -0.5f));
        Test_Gradient(new Vector3(10.5f, 10.5f, 10.5f));
    }

    private void Test_FlatCube(Vector3Int index)
    {
        int flat = m_Field.CubeIndexToFlatIndex(index);
        Vector3Int cubeIndex = m_Field.FlatIndexToCubeIndex(flat);

        Debug.LogFormat("Original: {0} Flat: {1} Cube: {2}", index, flat, cubeIndex);
    }

    private void Test_PosCube(Vector3Int index)
    {
        Vector3 pos = m_Field.CubeIndexToPosition(index);
        Vector3Int cubeIndex = m_Field.PositionToCubeIndex(pos);

        Debug.LogFormat("Original: {0} Pos: {1} Cube: {2}", index, pos, cubeIndex);
    }

    private void Test_Gradient(Vector3 position)
    {
        Vector3Int cubeIndex = m_Field.PositionToCubeIndex(position);
        Vector3 gradient = m_Field.GetNearestGradient(position);
        Debug.LogFormat("GetNearestValue: Position {0} Cube Index {1} Gradient {2}", position, cubeIndex, gradient);
    }
}
