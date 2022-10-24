using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_ChangeResolution : MonoBehaviour
{
    [SerializeField]
    private ChunkFieldBuilder m_Chunk;
    [SerializeField]
    private int m_Resolution;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            m_Chunk.SetResolution(m_Resolution);
            m_Chunk.GeneratePoints();
        }
    }
}
