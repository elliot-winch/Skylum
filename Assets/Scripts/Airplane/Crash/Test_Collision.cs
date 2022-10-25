using UnityEngine;

public class Test_Collision : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_RB;
    [SerializeField]
    private Vector3 m_ForceDirection;
    [SerializeField]
    private float m_ForceMagnitude = 1f;

    private void Update()
    {
        m_RB.AddForce(m_ForceDirection.normalized * m_ForceMagnitude * Time.deltaTime);
    }
}
