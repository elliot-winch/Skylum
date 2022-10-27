using UnityEngine;

public class FragmentHolder : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_RB;
    [SerializeField]
    private PartDestruction m_PartCollision;
    [SerializeField]
    private Fragment[] m_Fractures;

    private void Start()
    {
        m_PartCollision.PartRemaining.Subscribe(ReleaseFractures);
    }

    private void ReleaseFractures(float partAmount)
    {
        foreach(Fragment piece in m_Fractures)
        {
            if (piece.Fractured == false)
            {
                float piecePlace = m_PartCollision.GetPercentageAlongPart(piece.transform.position);

                if (partAmount < piecePlace)
                {
                    piece.SetFractured(true);
                    //SetFractured is moving control of the piece from parent to physics
                    //We need to init the physics with the same velocity as the parent
                    //If we don't, pieces not involved in the collision simply drop straight down
                    piece.Rigidbody.velocity = m_RB.velocity;
                }
            }
        }
    }
}

