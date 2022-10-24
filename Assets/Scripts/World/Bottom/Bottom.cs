using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottom : MonoBehaviour
{
    [SerializeField]
    private float m_StartingYLevel;

    public Topic<float> YLevel { get; } = new();

    private void Awake()
    {
        YLevel.Value = m_StartingYLevel;
    }

    private void OnValidate()
    {
        YLevel.Value = m_StartingYLevel;
    }
}
