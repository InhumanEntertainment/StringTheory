using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Looper : MonoBehaviour 
{
    public int MinLoopLength = 10;
    public float MinDistance = 0.1f;
    public GameObject Glow;

    //============================================================================================================================================//
    void Update()
    {
        Glow.active = false;

        object[] objects = GameObject.FindObjectsOfType(typeof(ColorString));
        ColorString[] curves = new ColorString[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            curves[i] = (ColorString)objects[i];

            Vector3[] loop = GetLoop(curves[i]);

            if (loop.Length > 0)
            {
                Game.Log("Loop Found");
                CheckLoop(loop);
            }
        }

        // Test With Mouse Position //
        Vector3 start = LastMouse;
        Vector3 end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 point = transform.position;
        //Debug.DrawLine(start, end, Color.red);

        CheckLine(start, end, point);
        LastMouse = end;
    }
	Vector3 LastMouse;

    //============================================================================================================================================//
    Vector3[] GetLoop(ColorString curve)
    {
        List<Vector3> loop = new List<Vector3>();
        int start = 0;
        int end = 0;

        for (int i = 0; i < curve.Tail.Count; i++)
        {
            for (int c = i + MinLoopLength; c < curve.Tail.Count; c++)
            {
                // Found Intersection //
                if (Vector3.Distance(curve.Tail[i], curve.Tail[c] ) < MinDistance)
                {
                    start = i;
                    end = c;

                    for (int x = start; x <= end; x++)
                    {
                        loop.Add(curve.Tail[x]); 
                    }
                    break;
                }
            }
        }

        return loop.ToArray();
    }

    //============================================================================================================================================//
    bool CheckLoop(Vector3[] loop)
    {
        bool result = true;

        // For each line check to see what side the point is on //
        int side = 0;

        for (int i = 0; i < loop.Length - 1; i++)
        {
            Vector3 start = loop[i];
            Vector3 end = loop[i + 1];
            Vector3 point = transform.position;

            int s = CheckLine(start, end, point);

            /*if (s <= 0)
                Debug.DrawLine(start, end, Color.green);
            else
                Debug.DrawLine(start, end, Color.red);*/


            if (i == 0)
                side = s;
            else
            {
                if (s != side)
                {
                    result = false;
                }
            }          
        }

        //Color color = Color.red;
        /*if (result)
        {
            Glow.active = true;

            //color = Color.green;

            for (int i = 0; i < loop.Length - 1; i++)
            {
                Debug.DrawLine(loop[i] + new Vector3(0, 0.3f, 0), loop[i + 1] + new Vector3(0, 0.3f, 0), color);
            }
        }*/
        //else
            //Glow.active = false;

        return result;
    }

    //============================================================================================================================================//
    // Returns -1 or 1 depending on which side of the line the point is on //
    //============================================================================================================================================//
    int CheckLine(Vector3 start, Vector3 end, Vector3 point)
    {
        int result = 0;

        // Local Space//
        Vector3 localPoint = point - start;
        Vector3 localEnd = end - start;

        // Rotate Point //
        float rotation = -Mathf.Atan2(localEnd.y, localEnd.x);
        //float rotation = -Mathf.Atan(localEnd.y / localEnd.x);

        //float x = Mathf.Cos(rotation) * localPoint.x - Mathf.Sin(rotation) * localPoint.y;
        float y = Mathf.Sin(rotation) * localPoint.x + Mathf.Cos(rotation) * localPoint.y;
        //Game.Log("X: " + x + " Y: " + y + " Rotation: " + (rotation * Mathf.Rad2Deg));
        result = y > 0 ? 1 : -1;
        //Game.Log(result);

        //Debug.DrawLine(transform.position, transform.position + (new Vector3(x, y, 0).normalized * 1), Color.yellow);
        //Debug.DrawLine(start, start - (localPoint.normalized * 1), Color.red);
        //Debug.DrawLine(start, start + (localEnd.normalized * 1), Color.red);
        //Debug.DrawLine(start, end, Color.yellow);

        return result;
    }
}