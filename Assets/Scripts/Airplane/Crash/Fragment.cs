using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Fragment : MonoBehaviour
{
    public bool Fractured { get; private set; }
    public Rigidbody Rigidbody { get; private set; }


    private Transform m_StartingParent;

    private void Awake()
    {
        m_StartingParent = transform.parent;
        Rigidbody = GetComponent<Rigidbody>();
        SetFractured(false);

    }

    //If the FragmentHolder doesn't fracture this fragment, it can call itself
    //The initial velocity will be applied by the collision
    private void OnCollisionEnter(Collision collision)
    {
        SetFractured(true);
    }

    public void SetFractured(bool fractured)
    {
        Fractured = fractured;
        //Removes control from parent position and hands it over the physics engine
        Rigidbody.isKinematic = fractured == false;
        Rigidbody.useGravity = fractured;
        transform.parent = fractured ? null : m_StartingParent;
    }
}
