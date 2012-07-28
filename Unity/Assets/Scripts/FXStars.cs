using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FXStars : MonoBehaviour 
{
    List<ParticleSystem.Particle> ParticleList = new List<ParticleSystem.Particle>();
    ParticleSystem.Particle[] Particles;
    List<float> ParticleStartSize = new List<float>();
    
    int NumberOfParticles = 0;

    public float LifeTimeMin = 1;
    public float LifeTimeMax = 2;
    public float StartSizeMin = 1;
    public float StartSizeMax = 2;
    public Color StartColor = Color.white;

    // Stats //
    public int Count = 0;

    //============================================================================================================================================//
    void Awake()
    {
        Particles = new ParticleSystem.Particle[4096];
        particleSystem.Play();
	}

    //============================================================================================================================================//
    void Update()
    {
        Emit(8);

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

                if(Mode == ParticleMode.Vortex)
                {
                    // Get Angle //
                    // Rotate Faster Close to Center //
                    float angle = Mathf.Atan(p.position.y / p.position.x);
                    float distance = p.position.magnitude;
                    float strength = Mathf.Pow(1f - (distance / 10f), 6) * 5 * Time.deltaTime;

                    angle += strength;

                    Vector3 newPos = new Vector3();
                    newPos.x = Mathf.Cos(strength) * p.position.x + Mathf.Sin(strength) * p.position.y;
                    newPos.y = -Mathf.Sin(strength) * p.position.x + Mathf.Cos(strength) * p.position.y;

                    p.position = newPos;
                    p.position += (p.velocity * Time.deltaTime);
                }
                if (Mode == ParticleMode.BlackHole)
                {                  
                    float radius = 2;

                    p.position += (p.velocity * Time.deltaTime);
                    Vector3 radiusPoint = p.position.normalized * radius;
                    Vector3 dir = radiusPoint - p.position;
                    float distance = Vector3.Distance(p.position, radiusPoint);
                    float strength = Mathf.Pow(1 - (distance / 7f), 2);

                    p.position += dir * strength * 5f * Time.deltaTime;             
                }
                else if (Mode == ParticleMode.Chaos)
                {
                    float amount = 10;
                    Vector3 rnd = new Vector3(amount * Random.value, amount * Random.value, 0) - new Vector3(amount * 0.5f, amount * 0.5f, 0);
                    p.position += ((p.velocity  + rnd)* Time.deltaTime);
                }
                else
                {
                    p.position += (p.velocity * Time.deltaTime);
                }
                
                ParticleList[i] = p;
            }
        }

        particleSystem.SetParticles(ParticleList.ToArray(), ParticleList.Count);

        Count = particleSystem.particleCount;
	}

    //============================================================================================================================================//
    public Vector3 VelocityRandom = Vector3.zero;
    public Vector3 Velocity = Vector3.zero;
    public Color ColorMin = Color.white;
    public Color ColorMax = Color.white;
    public enum ParticleMode {Chaos, Border, Horizontal, Game, GreenPack, Vortex, BlackHole};
    public ParticleMode Mode = ParticleMode.Border;

    void Emit(int number)
    {
        for (int i = 0; i < number; i++)
        {
            ParticleSystem.Particle particle = new ParticleSystem.Particle();

            if (Mode == ParticleMode.GreenPack)
                particle.color = Color.Lerp(Color.green, Color.white, Random.value);
            else
                particle.color = Color.Lerp(ColorMin, ColorMax, Random.value);

            if (Mode == ParticleMode.Game)
            {
                particle.color = new Color(particle.color.r, particle.color.g, particle.color.b, Random.value * 0.3f);
            }

            particle.startLifetime = Time.timeSinceLevelLoad;
            particle.lifetime = Mathf.Lerp(LifeTimeMin, LifeTimeMax, Random.value);
            particle.size = Mathf.Lerp(StartSizeMin, StartSizeMax, Random.value);

            if (Mode == ParticleMode.Border)
            {
                particle.position = GetPointOnRectangle();           
            }
            else
            {
                particle.position = new Vector3(Random.value * 10, Random.value * 10, 0) - new Vector3(5, 5, 0);
            }

            if (Mode == ParticleMode.Horizontal || Mode == ParticleMode.GreenPack)
            {
                particle.velocity = new Vector3(Random.value * VelocityRandom.x, 0, 0) - new Vector3(VelocityRandom.x * 0.5f, 0, 0) + Velocity;
            }
            else
            {
                particle.velocity = new Vector3(Random.value * VelocityRandom.x, Random.value * VelocityRandom.y, 0) - new Vector3(VelocityRandom.x * 0.5f, VelocityRandom.y * 0.5f, 0) + Velocity;
            }
            if (Mode == ParticleMode.Game)
            {
                particle.velocity *= 0.2f;
            }

            
            ParticleStartSize.Add(particle.size);
            //Particles[NumberOfParticles] = particle;
            ParticleList.Add(particle);

            NumberOfParticles++;
        }
    }

    //============================================================================================================================================//
    Vector3 GetPointOnRectangle()
    {
        float rnd = Random.value;
        Vector3 result = Vector3.zero;
        Rect rect = new Rect(-5, -5, 10, 10);

        if (rnd < 0.25f)
        {
            result = new Vector3(rect.left, rect.height * Random.value + rect.top);
        }
        else if (rnd < 0.5f)
        {
            result = new Vector3(rect.right, rect.height * Random.value + rect.top);
        }
        else if (rnd < 0.75f)
        {
            result = new Vector3(rect.width * Random.value + rect.left, rect.top);
        }
        else
        {
            result = new Vector3(rect.width * Random.value + rect.left, rect.bottom);
        }

        return result;
    }
}


// Spawn at location //
// Set random Velocity //
// Set Size Fade //