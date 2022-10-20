using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField]
    private WorldFieldBuilder m_FieldBuilderPrefab;

    [SerializeField]
    private float m_SpacePerPoint = 1/64;
    [SerializeField]
    private int m_PointsPerChunk = 64;
    [SerializeField]
    private Vector3Int m_NumChunks;
    [SerializeField]
    private NoiseParameters m_BackgroundFieldNoise;
    [SerializeField]
    private Island[] m_Islands;

    [SerializeField]
    private AnimationCurve m_IslandShape;
    [SerializeField]
    private int m_NumIslandShapeSamples = 64;

    private Dictionary<Vector3Int, WorldFieldBuilder> m_Chunks = new();

    private void Start()
    {
        //TODO: only spawn chunks as needed instead of always
        LoadWorld();
    }

    private void OnValidate()
    {
        //Debugging
        if (Application.isPlaying)
        {
            DeleteWorld();

            LoadWorld();
        }
    }

    private void LoadWorld()
    {
        for (int i = 0; i < m_NumChunks.x; i++)
        {
            for (int j = 0; j < m_NumChunks.y; j++)
            {
                for (int k = 0; k < m_NumChunks.z; k++)
                {
                    LoadChunk(new Vector3Int(i, j, k));
                }
            }
        }
    }

    private void DeleteWorld()
    {
        for (int i = 0; i < m_NumChunks.x; i++)
        {
            for (int j = 0; j < m_NumChunks.y; j++)
            {
                for (int k = 0; k < m_NumChunks.z; k++)
                {
                    RemoveChunk(new Vector3Int(i, j, k));
                }
            }
        }
    }

    private void LoadChunk(Vector3Int chunkIndex)
    {
        WorldFieldBuilder chunk = m_Chunks.GetValueOrDefault(chunkIndex);

        if (chunk == null)
        {
            chunk = Instantiate(m_FieldBuilderPrefab);
            Vector3 chunkPosition = ChunkIndexToWorldPosition(chunkIndex);

            chunk.transform.SetParent(this.transform);
            chunk.transform.localPosition = chunkPosition;
            chunk.PositionOffset = chunkPosition;
        }

        //Update chunk params
        chunk.NoiseParameters = m_BackgroundFieldNoise;

        chunk.GridScale = m_SpacePerPoint;
        chunk.Dimensions = Vector3Int.one * m_PointsPerChunk;

        chunk.Islands = m_Islands; //TODO: only send islands in range
        chunk.IslandShape = m_IslandShape;
        chunk.NumIslandShapeSamples = m_NumIslandShapeSamples;
        m_Chunks[chunkIndex] = chunk;

        //(Re)generate points
        chunk.GeneratePoints();
    }

    private void RemoveChunk(Vector3Int chunkIndex)
    {
        WorldFieldBuilder chunk = m_Chunks.GetValueOrDefault(chunkIndex);

        if(chunk != null)
        {
            m_Chunks.Remove(chunkIndex);
            Destroy(chunk.gameObject);
        }
    }

    //Since vertices placed on the edges of chunks have invalid normals, we need
    //the chunks to overlap, else there will be gaps between chunks.
    public Vector3 ChunkIndexToWorldPosition(Vector3 chunkIndex)
    {
        Vector3 position = new(chunkIndex.x, chunkIndex.y, chunkIndex.z);
        return position * (1 - m_SpacePerPoint);
    }
}
