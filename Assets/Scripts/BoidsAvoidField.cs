using UnityEngine;

public class BoidsAvoidField : BoidBehaviourController
{
    [SerializeField]
    private FieldBuilder m_FieldBuilder;
    [SerializeField]
    private float m_DeflectionFieldValue;
    [SerializeField]
    private float m_DeflectionForceFactor;

    public override Vector3 AdjustAcceleration(Boid boid)
    {
        Field field = m_FieldBuilder.Field.Value;
        float fieldValue = field.GetNearestValue(boid.transform.position);
        Vector3 deflectionAcceleration = Vector3.zero;

        //Larger field values means closer / inside an island
        if(fieldValue > m_DeflectionFieldValue)
        {
            //Gradient points towards islands
            Vector3 fieldGradient = field.GetNearestGradient(boid.transform.position);
            deflectionAcceleration = -fieldGradient * (fieldValue - m_DeflectionFieldValue) * m_DeflectionForceFactor;
            Debug.Log(deflectionAcceleration);
            Debug.DrawRay(boid.transform.position, deflectionAcceleration, Color.yellow, 0.1f);
        }

        return deflectionAcceleration;
    }
}
