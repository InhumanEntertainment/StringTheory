using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FXStars : MonoBehaviour 
{
    List<ParticleSystem.Particle> ParticleList = new List<ParticleSystem.Particle>();
    ParticleSystem.Particle[] Particles;
    List<float> ParticleStartSize = new List<float>();

    public int Rate = 5;
    public int NumberOfParticles = 0;
    public enum ParticleMode { Chaos, Border, Horizontal, Game, GreenPack, Vortex, BlackHole };
    public ParticleMode Mode = ParticleMode.Border;

    public FXBase[] FX;
    public int FXIndex = 0;
    public Dictionary<string, int> FXNames = new Dictionary<string, int>();

    //============================================================================================================================================//
    void Awake()
    {
        // Store Names //
        for (int i = 0; i < FX.Length; i++)
        {
            FXNames.Add(FX[i].Name, i);
        }

        particleSystem.Play();
	}

    //============================================================================================================================================//
    public void SetEffect(string name)
    {
        if (FXNames.ContainsKey(name))
        {
            FXIndex = FXNames[name];
        }
        else
            FXIndex = 0;       
    }

    //============================================================================================================================================//
    void Update()
    {
        if (!Game.Instance.Paused)
        {
            Emit(Rate);

            for (int i = 0; i < ParticleList.Count; i++)
            {
                var life = ((Time.timeSinceLevelLoad - ParticleList[i].startLifetime) / ParticleList[i].lifetime);

                if (life > 1)
                {
                    ParticleList.RemoveAt(i);
                    ParticleStartSize.RemoveAt(i);             
                }
                else
                {
                    ParticleSystem.Particle p = ParticleList[i];
                    //p.size = ParticleStartSize[i] * (1f - life);
                    p.size = ParticleStartSize[i] * (0.5f - Mathf.Abs(life - 0.5f)) * 2;

                    // Vortex //
                    if (FX[FXIndex].VortexForce)
                    {
                        float distance = p.position.magnitude;
                        float strength = Mathf.Pow(1f - (distance / FX[FXIndex].VortexRadius), FX[FXIndex].VortexExponent) * FX[FXIndex].VortexStength * Time.deltaTime;

                        Vector3 newPos = new Vector3();
                        newPos.x = Mathf.Cos(strength) * p.position.x + Mathf.Sin(strength) * p.position.y;
                        newPos.y = -Mathf.Sin(strength) * p.position.x + Mathf.Cos(strength) * p.position.y;

                        //p.velocity += newPos - p.position;
                        p.position = newPos;
                    }

                    // Radial //
                    if (FX[FXIndex].RadialForce && FX[FXIndex].RadialOuterRadius > 0)
                    {
                        Vector3 radiusPoint = p.position.normalized * FX[FXIndex].RadialInnerRadius;
                        Vector3 dir = radiusPoint - p.position;
                        float distance = Vector3.Distance(p.position, radiusPoint);

                        float strength = Mathf.Pow(1 - (distance / FX[FXIndex].RadialOuterRadius), 2);

                        //p.velocity += dir * strength * FX[FXIndex].RadialStrength;
                        p.position += dir * strength * FX[FXIndex].RadialStrength * Time.deltaTime;
                    }

                    // Jitter //
                    if (FX[FXIndex].JitterForce)
                    {
                        float amount = FX[FXIndex].JitterStrength;
                        Vector3 rnd = new Vector3(amount * Random.value, amount * Random.value, 0) - new Vector3(amount * 0.5f, amount * 0.5f, 0);

                        p.velocity = p.velocity * 0.8f + rnd * Time.deltaTime;
                    }
                    
                    p.position += (p.velocity * Time.deltaTime);

                    ParticleList[i] = p;
                }
            }

            particleSystem.SetParticles(ParticleList.ToArray(), ParticleList.Count);
            NumberOfParticles = ParticleList.Count;
        }
	}

    //============================================================================================================================================//
    void Emit(int number)
    {
        for (int i = 0; i < number; i++)
        {
            ParticleSystem.Particle particle = new ParticleSystem.Particle();

            particle.color = Color.Lerp(FX[FXIndex].ColorMin, FX[FXIndex].ColorMax, Random.value);
            particle.startLifetime = Time.timeSinceLevelLoad;
            particle.lifetime = Mathf.Lerp(FX[FXIndex].LifeTimeMin, FX[FXIndex].LifeTimeMax, Random.value);
            particle.size = Mathf.Lerp(FX[FXIndex].StartSizeMin, FX[FXIndex].StartSizeMax, Random.value);
            particle.velocity = new Vector3(Random.value * FX[FXIndex].VelocityRandom.x, Random.value * FX[FXIndex].VelocityRandom.y, 0) - new Vector3(FX[FXIndex].VelocityRandom.x * 0.5f, FX[FXIndex].VelocityRandom.y * 0.5f, 0) + FX[FXIndex].Velocity;
            
            // Position //
            if (FX[FXIndex].Emitter == FXBase.EmitterType.Border)
            {
                particle.position = GetPointOnRectangle(FX[FXIndex].PositionMin, FX[FXIndex].PositionMax);           
            }
            else if (FX[FXIndex].Emitter == FXBase.EmitterType.Box)
            {
                Vector2 size = FX[FXIndex].PositionMax - FX[FXIndex].PositionMin;
                particle.position = new Vector3(Random.value * size.x, Random.value * size.y, 0) + new Vector3(FX[FXIndex].PositionMin.x, FX[FXIndex].PositionMin.y, 0);
            }
            else if (FX[FXIndex].Emitter == FXBase.EmitterType.Point)
            {
                particle.position = new Vector3(FX[FXIndex].PositionMin.x, FX[FXIndex].PositionMin.y, 0);
            }
            else
            {
                particle.position = Vector3.zero;
            }
            particle.position = particle.position + new Vector3(0, 0, 1);
            
            ParticleStartSize.Add(particle.size);
            ParticleList.Add(particle);
        }
    }

    //============================================================================================================================================//
    Vector3 GetPointOnRectangle(Vector2 min, Vector2 max)
    {
        float rnd = Random.value;
        Vector3 result = Vector3.zero;
        Vector2 size = max - min;
                
        if (rnd < 0.25f)
        {
            result = new Vector3(min.x, size.y * Random.value + min.y);
        }
        else if (rnd < 0.5f)
        {
            result = new Vector3(max.x, size.y * Random.value + min.y);
        }
        else if (rnd < 0.75f)
        {
            result = new Vector3(size.x * Random.value + min.x, min.y);
        }
        else
        {
            result = new Vector3(size.x * Random.value + min.x, max.y);
        }

        return result;
    }
}