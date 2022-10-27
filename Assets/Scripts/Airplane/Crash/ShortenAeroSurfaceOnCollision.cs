using UnityEngine;

public class ShortenAeroSurfaceOnCollision : MonoBehaviour
{
    [SerializeField]
    private PartDestruction m_PartDestruction;
    [SerializeField]
    private AeroSurface[] m_Surfaces;

    private AeroSurfaceConfig[] m_OriginalConfigs;

    private void Start()
    {
        m_OriginalConfigs = new AeroSurfaceConfig[m_Surfaces.Length];
        for(int i =0; i < m_Surfaces.Length; i++)
        {
            m_OriginalConfigs[i] = m_Surfaces[i].Config;
        }

        m_PartDestruction.PartRemaining.Subscribe(DisableSurfaces);
    }

    private void DisableSurfaces(float percentage)
    {
        foreach (AeroSurface ap in m_Surfaces)
        {
            float surfacePercentage = m_PartDestruction.GetPercentageAlongPart(ap.transform.position);
            ap.gameObject.SetActive(surfacePercentage < percentage);
        }
    }
}
