using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Since OnCollisionEnter is called on the Rigidbody, and the aricraft only has
 * one Rigidbody, we don't want many components on the root Airplane object. This
 * scripts allows scripts elsewhere in the scene to respond to the collision event.
 */ 
public class CollisionDetection : MonoBehaviour
{
    private Dictionary<Collider, Action<Collision>> OnCollision = new();

    public void Subscribe(Collider collider, Action<Collision> action)
    {
        if(OnCollision.ContainsKey(collider) == false)
        {
            OnCollision.Add(collider, action);
        }
        else
        {
            OnCollision[collider] += action;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        foreach(Collider collider in OnCollision.Keys)
        {
            if(collision.GetContact(0).thisCollider == collider)
            {
                OnCollision[collider]?.Invoke(collision);
            }
        }
    }

}
