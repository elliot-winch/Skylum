using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeOnCollision : MonoBehaviour
{
    [SerializeField]
    private PartDestruction m_PartCollision;
    [SerializeField]
    private ParticleSystem m_PS;

    // Start is called before the first frame update
    void Start()
    {
        m_PartCollision.PartRemaining.Subscribe(PlaceSmoke);
    }

    private void PlaceSmoke(float partPercentage)
    {
        if(partPercentage < 1)
        {
            m_PS.Play();
        }
        else
        {
            m_PS.Stop();
        }

        m_PS.transform.localPosition = Vector3.Lerp(m_PartCollision.Close, m_PartCollision.Far, partPercentage);
    }
}
