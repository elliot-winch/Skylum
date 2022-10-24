using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeQuest : Quest
{
    [SerializeField]
    private Transform m_PlaneTransform;
    [SerializeField]
    private Transform m_HangarTransform;
    [SerializeField]
    private float m_MaxSucceedDistanceSqr;

    private void Update()
    {
        if(IsActive == false)
        {
            return;
        }

        if(Vector3.SqrMagnitude(m_PlaneTransform.position - m_HangarTransform.position) < m_MaxSucceedDistanceSqr)
        {
            Succeed();
        }
    }


}
