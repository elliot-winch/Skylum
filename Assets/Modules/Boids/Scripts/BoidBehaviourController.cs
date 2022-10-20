using UnityEngine;

public abstract class BoidBehaviourController : MonoBehaviour
{
    public abstract Vector3 AdjustAcceleration(Boid boid);
}
