using UnityEngine;

public class FractureObject : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Original;
    [SerializeField]
    private GameObject m_Fractured;

    private void Awake()
    {
        SetFractured(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            SetFractured(true);
        }
    }

    public void SetFractured(bool fractured)
    {
        m_Original.SetActive(fractured == false);
        m_Fractured.SetActive(fractured);
    }
}
