using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FX
{
    public string Name = "Default";
    public Color Color;

    public Vector3 VelocityRandom = Vector3.zero;
    public Vector3 Velocity = Vector3.zero;
    public Color ColorMin = Color.white;
    public Color ColorMax = Color.cyan;

    //============================================================================================================================================//
    public void Update()
    {

    }

    //============================================================================================================================================//
    public void Emit(int number)
    {
        // return particle //
    }
}
