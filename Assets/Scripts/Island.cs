using System;
using UnityEngine;

//TODO: make Scriptable Object?
//NB mirror of Island sturct in World.compute, do not edit without
//editting compute shader
[Serializable]
public struct Island
{
    public Vector4 Base;
    public Vector4 Tip;

    public static int SizeOf => sizeof(float) * 8;
} 