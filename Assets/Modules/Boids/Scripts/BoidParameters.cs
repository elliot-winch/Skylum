using UnityEngine;
using System;

[Serializable]
public class BoidParameters 
{
    [SerializeField]
    public float SpeedLimit = 1f;
    //[SerializeField]
    //public float VisionRange = 1f;
    //[Tooltip("In degrees from 0 to 180")]
    //[SerializeField]
    //public float VisionFieldOfView = 45f;
    [SerializeField]
    public int VisionNumber = 5;
    [SerializeField]
    public float AvoidanceWeight = 0.5f;
    [SerializeField]
    public float AlignmentWeight = 1f;
    [SerializeField]
    public float CohensionWeight = 1f;
}
