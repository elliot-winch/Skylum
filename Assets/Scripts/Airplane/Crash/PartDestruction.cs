using UnityEngine;

public class PartDestruction : MonoBehaviour
{
    [SerializeField]
    private CollisionDetection m_CollisionDetection;
    [SerializeField]
    private Collider m_Collider;
    [SerializeField]
    private Vector3 m_Close;
    [SerializeField]
    private Vector3 m_Far;
    //If set to non-slideable, any collision will destroy the entire part
    [SerializeField]
    private bool m_Slideable = true;

    public Topic<float> PartRemaining = new(1f);

    private void Start()
    {
        m_CollisionDetection.Subscribe(m_Collider, OnCollision);
    }

    private void OnCollision(Collision collision)
    {
        ContactPoint[] points = new ContactPoint[collision.contactCount];
        collision.GetContacts(points);

        foreach (ContactPoint contact in points)
        {
            float partAmount = GetPercentageAlongPart(contact.point);

            if (partAmount < PartRemaining.Value)
            {
                PartRemaining.Value = m_Slideable ? partAmount : 0f;
                Debug.Log("Setting part remaining to " + partAmount);
            }
        }
    }

    public float GetPercentageAlongPart(Vector3 worldPosition)
    {
        Vector3 localPosition = transform.InverseTransformPoint(worldPosition);
        Vector3 line = m_Far - m_Close;

        Debug.DrawRay(m_Close, localPosition - m_Close, Color.red, 100f);
        Debug.DrawRay(m_Close, m_Far - m_Close, Color.yellow, 100f);

        float amount = Vector3.Dot(line.normalized, localPosition - m_Close) / line.magnitude;
        return amount;
    }
}
