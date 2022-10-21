using UnityEngine;

/*
 * While the World class is not optimised (empty space costs just as much
 * as non-empty space), we will spawn multiple 'worlds'. Boids and other 
 * classes that use the noise field will not work, so this measure must
 * only be temporary. 
 * 
 * A spawning script to place Islands into the World will replace this 
 * script once World can support many (mostly empty) chunks.
 */
public class WorldCreator : MonoBehaviour
{
    [SerializeField]
    private World m_WorldPrefab;
    [SerializeField]
    private float m_Padding; //Islands size plus some distance to safely separate the islands
    [SerializeField]
    private Vector3Int m_NumWorlds;

    private void Start()
    {
        Vector3 noiseOffset = RandomExtended.RandomVector3() * 1000f;

        for (int i = 0; i < m_NumWorlds.x; i++)
        {
            for (int j = 0; j < m_NumWorlds.y; j++)
            {
                for (int k = 0; k < m_NumWorlds.z; k++)
                {
                    Vector3 coord = new Vector3(i, j, k);
                    World world = Instantiate(m_WorldPrefab);
                    world.transform.position = (coord + RandomExtended.RandomVector3()) * m_Padding;
                    world.name = "World at " + coord;

                    //Set noise offset
                    world.BackgroundFieldNoise.NoiseOffset = noiseOffset + world.transform.position;
                }
            }
        }
    }
}
