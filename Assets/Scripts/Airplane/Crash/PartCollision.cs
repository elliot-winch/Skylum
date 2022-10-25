using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartCollision : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_Close;
    [SerializeField]
    private Vector3 m_Far;

    public Topic<float> PartRemaining = new(1f);

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] points = new ContactPoint[collision.contactCount];
        collision.GetContacts(points);

        foreach (ContactPoint contact in points)
        {
            float partAmount = GetPercentageAlongPart(contact.point);

            if (partAmount < PartRemaining.Value)
            {
                PartRemaining.Value = partAmount;
            }
        }
    }

    public float GetPercentageAlongPart(Vector3 worldPosition)
    {
        //TODO: put into local space

        //Vector3 localPosition = transform.InverseTransformVector(worldPosition);
        Vector3 line = m_Far - m_Close;

        Debug.DrawRay(m_Close, worldPosition - m_Close, Color.red, 100f);
        Debug.DrawRay(m_Close, m_Far - m_Close, Color.yellow, 100f);

        float amount = Vector3.Dot(line.normalized, worldPosition - m_Close) / line.magnitude;
        return amount;
    }
}
