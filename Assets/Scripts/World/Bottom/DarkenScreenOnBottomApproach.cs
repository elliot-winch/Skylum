using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class DarkenScreenOnBottomApproach : MonoBehaviour
{
    [SerializeField]
    private Transform m_PlaneTransform;
    [SerializeField]
    private Bottom m_Bottom;
    [Header("Vignette")]
    [SerializeField]
    private PostProcessVolume m_Effect;
    [SerializeField]
    private float m_VignetteDistance;
    [Header("Fade To Black")]
    [SerializeField]
    private Image m_Image;
    [SerializeField]
    private float m_FadeToBlackDistance;

    private void Update()
    {
        m_Effect.weight = 1 - Mathf.InverseLerp(m_Bottom.YLevel.Value, m_VignetteDistance, m_PlaneTransform.position.y);
        Color c = m_Image.color;
        c.a = 1 - Mathf.InverseLerp(m_Bottom.YLevel.Value, m_FadeToBlackDistance, m_PlaneTransform.position.y);
        m_Image.color = c;
    }
}
