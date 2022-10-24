using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartCollision : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_Close;
    [SerializeField]
    private Vector3 m_Far;
    [SerializeField]
    private Transform[] m_Fractures;

    private float m_PartRemaining = 1;


    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] points = new ContactPoint[collision.contactCount];
        collision.GetContacts(points);

        foreach (ContactPoint contact in points)
        {
            float partAmount = GetPercentageAlongPart(contact.point);

            Debug.Log(partAmount);

            if(partAmount < m_PartRemaining)
            {
                m_PartRemaining = partAmount;
                ReleaseFractures(partAmount);
            }
        }
    }

    private void ReleaseFractures(float partAmount)
    {
        foreach(Transform piece in m_Fractures)
        {
            if (piece.gameObject.activeInHierarchy == false)
            {
                float piecePlace = GetPercentageAlongPart(piece.position);
                Debug.Log(piece.name + " " + piece.position + " " + piecePlace);

                if (piecePlace > partAmount)
                {
                    piece.gameObject.SetActive(true);
                }
            }
        }
    }

    private float GetPercentageAlongPart(Vector3 worldPosition)
    {
        //Vector3 localPosition = transform.InverseTransformVector(worldPosition);
        Vector3 line = m_Far - m_Close;

        Debug.DrawRay(m_Close, worldPosition - m_Close, Color.red, 100f);
        Debug.DrawRay(m_Close, m_Far - m_Close, Color.yellow, 100f);

        float amount = Vector3.Dot(line.normalized, worldPosition - m_Close) / line.magnitude;
        Debug.Log(amount);
        return amount;
    }
}

