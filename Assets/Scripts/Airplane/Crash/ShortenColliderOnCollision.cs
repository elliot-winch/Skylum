using UnityEngine;

public class ShortenColliderOnCollision : MonoBehaviour
{
    public enum Direction
    {
        X, 
        Y,
        Z
    }

    [SerializeField]
    private PartDestruction m_PartCollision;
    [SerializeField]
    private BoxCollider m_BoxCollider;
    [SerializeField]
    private Direction m_Direction; //TODO

    private Vector3 m_StartingCenter;
    private Vector3 m_StartingSize;

    private void Start()
    {
        m_StartingCenter = m_BoxCollider.center;
        m_StartingSize = m_BoxCollider.size;
        m_PartCollision.PartRemaining.Subscribe(ShortenCollider);
    }

    private void ShortenCollider(float partPercentage)
    {
        float center = Mathf.Lerp(0, m_StartingCenter.x, partPercentage);
        float size = Mathf.Lerp(0, m_StartingSize.x, partPercentage);

        m_BoxCollider.center = new Vector3(center, m_BoxCollider.center.y, m_BoxCollider.center.z);
        m_BoxCollider.size = new Vector3(size, m_BoxCollider.size.y, m_BoxCollider.size.z);
    }
}
