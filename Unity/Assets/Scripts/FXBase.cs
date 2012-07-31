using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FXBase
{
    public string Name = "Default";

    public enum EmitterType {Point, Sphere, Box, Border};
    public EmitterType Emitter = EmitterType.Box;
    public Vector2 PositionMin, PositionMax;
    public Vector3 VelocityRandom = Vector3.zero;
    public Vector3 Velocity = Vector3.zero;
    public Color ColorMin = Color.white;
    public Color ColorMax = Color.cyan;
    public float LifeTimeMin = 1;
    public float LifeTimeMax = 2;
    public float StartSizeMin = 1;
    public float StartSizeMax = 2;
    public Color StartColor = Color.white;

    // Vortex Force //
    public bool VortexForce = false;
    public float VortexStength = 5;
    public float VortexRadius = 10;
    public float VortexExponent = 6;
    
    // Radial Force //
    public bool RadialForce = false;
    public float RadialInnerRadius = 2;
    public float RadialOuterRadius = 7;
    public float RadialStrength = 5;

    // Jitter Force //
    public bool JitterForce = false;
    public float JitterStrength = 50;

    //============================================================================================================================================//
    public virtual void Update()
    {

    }

    //============================================================================================================================================//
    public virtual void Emit(int number)
    {

    }
}
