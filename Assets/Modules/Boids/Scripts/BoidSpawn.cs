using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoidSpawn : MonoBehaviour
{
    [SerializeField]
    private int m_Seed;
    [SerializeField]
    private Boid m_BoidPrefab;
    [SerializeField]
    private int m_NumRandomBoids;
    [SerializeField]
    private Vector3 m_Margins;
    [SerializeField]
    private float m_MarginCorrection = 1f;
    [SerializeField]
    private float m_MaxStartingSpeed;
    [SerializeField]
    private BoidBehaviourController[] m_BehaviourControllers;

    [SerializeField]
    private BoidParameters m_BoidParameters;

    [SerializeField]
    private List<Boid> m_Boids;

    private void Start()
    {
        Random.InitState(m_Seed);

        SpawnRandomBoids();

        InitBoids();
    }

    private void SpawnRandomBoids()
    {
        for (int i = 0; i < m_NumRandomBoids; i++)
        {
            Boid boid = Instantiate(m_BoidPrefab);

            Vector3 randomPosition = new()
            {
                x = Random.value * m_Margins.x,
                y = Random.value * m_Margins.y,
                z = Random.value * m_Margins.z
            };

            Vector3 randomRotation = new()
            {
                x = Random.value * 360f,
                y = Random.value * 360f,
                z = Random.value * 360f
            };

            boid.transform.SetParent(transform);
            boid.transform.localPosition = randomPosition;
            boid.transform.localRotation = Quaternion.Euler(randomRotation);

            boid.Velocity = boid.transform.up * Random.Range(0, m_MaxStartingSpeed);

            boid.Parameters = m_BoidParameters;

            m_Boids.Add(boid);
        }
    }

    private void InitBoids()
    {
        foreach(Boid boid in m_Boids)
        {
            boid.AdjustAcceleration.Add(LimitBoid);

            foreach (BoidBehaviourController bc in m_BehaviourControllers)
            {
                boid.AdjustAcceleration.Add(bc.AdjustAcceleration);
            }
        }
    }

    private Vector3 LimitBoid(Boid boid)
    {
        //Assuming boid is child of this object
        Vector3 boidPosition = boid.transform.localPosition;
        Vector3 adjustment = Vector3.zero;

        if(boidPosition.x > m_Margins.x || boidPosition.x < 0)
        {
            adjustment += m_MarginCorrection * (m_Margins.x - boidPosition.x) * transform.right;
        }

        if(boidPosition.y > m_Margins.y || boidPosition.y < 0)
        {
            adjustment += m_MarginCorrection * (m_Margins.y - boidPosition.y) * transform.up;
        }

        if (boidPosition.z > m_Margins.z || boidPosition.z < 0)
        {
            adjustment += m_MarginCorrection * (m_Margins.z - boidPosition.z) * transform.forward;
        }

        return adjustment;
    }

    private void Update()
    {
        foreach(Boid b in m_Boids)
        {
            b.FrameCalculate(m_Boids.Where(a => a != b));
            b.FrameMove();
        }
    }
}
