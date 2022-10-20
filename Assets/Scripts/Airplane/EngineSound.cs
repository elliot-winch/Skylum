using UnityEngine;

public class EngineSound : MonoBehaviour
{
    [SerializeField]
    private AircraftPhysics m_Aircraft;
    [SerializeField]
    private AudioSource m_Audio;
    [SerializeField]
    private Vector2 m_PitchBounds;
    [SerializeField]
    private float m_VolumeThreshold;

    private float m_MaxVolume;

    private void Start()
    {
        m_MaxVolume = m_Audio.volume;
        m_Aircraft.ThrustPercent.Subscribe(SetPitchToThrustPercentage);
    }

    private void SetPitchToThrustPercentage(float thrustPercentage)
    {
        m_Audio.volume = Mathf.Lerp(0, m_MaxVolume, thrustPercentage / m_VolumeThreshold);
        m_Audio.pitch = Mathf.Lerp(m_PitchBounds.x, m_PitchBounds.y, thrustPercentage);
    }
}
