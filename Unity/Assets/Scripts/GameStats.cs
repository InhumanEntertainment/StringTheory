using UnityEngine;
using System.Collections;

public class GameStats : MonoBehaviour 
{
    float SmoothFPS = 0;
    public float Smoothness = 0.99f;
    UILabel Label;

    //============================================================================================================================================//
    void Awake()
    {
        Label = GetComponent<UILabel>();
    }

    //============================================================================================================================================//
    void Update()
    {
        if (!Game.Instance.Paused)
        {
            float fps = 1f / Time.deltaTime;
            SmoothFPS = SmoothFPS * Smoothness + fps * (1f - Smoothness);
            if (float.IsInfinity(SmoothFPS))
            {
                SmoothFPS = fps;
            }
            Label.text = SmoothFPS.ToString("N0");// +"fps";
        }  
	}
}
