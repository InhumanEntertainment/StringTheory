using UnityEngine;
using System.Collections;

public class TargetValue
{
    public float Target = 0;
    //public float Min;
    //public float Max;
    public float Value = 0;
    public float Speed = 0.5f;
    public float Inertia = 0.9f;
    public float Velocity = 0;

    //============================================================================================================================================//
    public TargetValue(float target, float value, float speed, float inertia)
    {
        Target = target;
        Value = value;
        Speed = speed;
        Inertia = inertia;
    }

    //============================================================================================================================================//
    public void Update()
    {
        float distance = (Target - Value);
        float strength = distance * Speed;
        
        Velocity = Velocity * Inertia + strength * Time.fixedDeltaTime;

        //Velocity = Mathf.Clamp(Velocity, distance * 1f, -distance * 1f);

        Value = Value + Velocity;
    }
}
