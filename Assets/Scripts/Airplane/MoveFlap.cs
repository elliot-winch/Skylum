using UnityEngine;

public class MoveFlap : MonoBehaviour
{
    private enum Axis
    {
        X, Y, Z
    }

    [SerializeField]
    private AeroSurface m_Aerosurface;
    [SerializeField]
    private Transform m_Transform;
    [SerializeField]
    private Axis m_Axis;

    private void Start()
    {
        m_Aerosurface.FlapAngle.Subscribe(AlignFlap);
    }

    private void AlignFlap(float angle)
    {
        Debug.Log(gameObject.name + " : " + angle);

        Vector3 rotation = m_Transform.localEulerAngles;

        angle *= Mathf.Rad2Deg;

        switch (m_Axis)
        {
            case Axis.X:
                rotation.x = angle;
                break;
            case Axis.Y:
                rotation.y = angle;
                break;
            case Axis.Z:
                rotation.z = angle;
                break;
        }

        m_Transform.localEulerAngles = rotation;
    }
}
