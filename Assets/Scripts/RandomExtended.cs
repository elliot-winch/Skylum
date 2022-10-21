using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomExtended 
{
    public static Vector3 RandomVector3()
    {
        return new Vector3()
        {
            x = Random.value,
            y = Random.value,
            z = Random.value
        };
    }   
}
