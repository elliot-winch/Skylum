using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField]
    private ChunkFieldBuilder m_FieldBuilderPrefab;

    [SerializeField]
    private float m_SpacePerPoint = 1/64;
    [SerializeField]
    private int m_PointsPerChunk = 64;
    [SerializeField]
    private Vector3Int m_NumChunks;
    [SerializeField]
    public NoiseParameters BackgroundFieldNoise;
    [SerializeField]
    private Island[] m_Islands;

    [SerializeField]
    private AnimationCurve m_IslandShape;
    [SerializeField]
    private int m_NumIslandShapeSamples = 64;

    private Dictionary<Vector3Int, ChunkFieldBuilder> m_Chunks = new();

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
        var watch = new System.Diagnostics.Stopwatch();
        watch.Start();

        ChunkFieldBuilder chunk = m_Chunks.GetValueOrDefault(chunkIndex);

        if (chunk == null)
        {
            chunk = Instantiate(m_FieldBuilderPrefab);
            Vector3 chunkPosition = ChunkIndexToWorldPosition(chunkIndex);

            chunk.transform.SetParent(this.transform);
            chunk.transform.localPosition = chunkPosition;
            chunk.PositionOffset = chunkPosition;
        }

        //Update chunk params
        chunk.NoiseParameters = BackgroundFieldNoise;

        chunk.GridScale = m_SpacePerPoint;
        chunk.Dimensions.Value = Vector3Int.one * m_PointsPerChunk; //TEMP: resolution will change on distance
        //to camera, if it's a collider etc.

        chunk.Islands = m_Islands; //TODO: only send islands in range
        chunk.IslandShape = m_IslandShape;
        chunk.NumIslandShapeSamples = m_NumIslandShapeSamples;
        m_Chunks[chunkIndex] = chunk;

        //(Re)generate points
        chunk.GeneratePoints();

        watch.Stop();

        //Debug.LogFormat("Chunk {0} took {1} ms to spawn", chunkIndex, watch.ElapsedMilliseconds);
    }

    private void RemoveChunk(Vector3Int chunkIndex)
    {
        ChunkFieldBuilder chunk = m_Chunks.GetValueOrDefault(chunkIndex);

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
