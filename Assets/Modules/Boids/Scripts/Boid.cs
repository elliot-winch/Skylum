using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public BoidParameters Parameters;
    public Vector3 Velocity;

    public List<Func<Boid, Vector3>> AdjustAcceleration { get; set; } = new List<Func<Boid, Vector3>>();

    public void FrameCalculate(IEnumerable<Boid> otherBoids)
    {

        //Continue travelling in current direction

        List<Boid> seenBoids = SeenBoids(otherBoids);

        Vector3 acceleration = Vector3.zero;

        acceleration += Parameters.AvoidanceWeight * Avoidance(seenBoids);
        acceleration += Parameters.AlignmentWeight * Alignment(seenBoids);
        acceleration += Parameters.CohensionWeight * Cohension(seenBoids);

        foreach(var func in AdjustAcceleration)
        {
            acceleration += func(this);
        }
        
        Debug.DrawRay(transform.position, Parameters.AvoidanceWeight * Avoidance(seenBoids), Color.blue, 0.1f);
        Debug.DrawRay(transform.position, Parameters.AlignmentWeight * Alignment(seenBoids), Color.green, 0.1f);
        Debug.DrawRay(transform.position, Parameters.CohensionWeight * Cohension(seenBoids), Color.yellow, 0.1f);

        Velocity += Time.deltaTime * acceleration;
        Velocity = SpeedLimit(Velocity);
    }

    public void FrameMove()
    {
        transform.position += Time.deltaTime * Velocity;
        transform.up = Velocity;
    }

    private List<Boid> SeenBoids(IEnumerable<Boid> boids)
    {
        return boids
            .OrderBy(boid => (boid.transform.position - transform.position).sqrMagnitude)
            .Take(Parameters.VisionNumber)
            .ToList();
    }

    private Vector3 Avoidance(List<Boid> boids)
    {
        if(boids.Count > 0)
        {
            return -boids.Select(boid => boid.transform.position - transform.position)
                .Aggregate((v1, v2) => v1 + v2);
        }

        return Vector3.zero;
    }

    private Vector3 Alignment(List<Boid> boids)
    {
        if(boids.Count > 0)
        {
            return Average(boids.Select(boid => boid.Velocity)) - Velocity;
        }

        return Vector3.zero;
    }

    private Vector3 Cohension(List<Boid> boids)
    {
        if (boids.Count > 0)
        {
            return Average(boids.Select(boid => boid.transform.position)) - transform.position;
        }

        return Vector3.zero;
    }

    private Vector3 SpeedLimit(Vector3 velocity)
    {
        if(velocity.sqrMagnitude > Parameters.SpeedLimit * Parameters.SpeedLimit)
        {
            return velocity.normalized * Parameters.SpeedLimit;
        }

        return velocity;
    }

    private Vector3 Average(IEnumerable<Vector3> vectors)
    {
        if(vectors.Count() > 0)
        {
            Vector3 sum = Vector3.zero;

            foreach (Vector3 v in vectors)
            {
                sum += v;
            }

            return sum / vectors.Count();
        }

        return Vector3.zero;
    }
}
