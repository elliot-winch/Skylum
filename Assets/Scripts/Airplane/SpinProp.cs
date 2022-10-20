using UnityEngine;

//TODO: subscribe to topic of engine on, lerp up and down etc.
public class SpinProp : MonoBehaviour
{
    [SerializeField]
    private float m_SpinRate;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, m_SpinRate, Space.Self);
    }
}
