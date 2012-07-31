using UnityEngine;
using System.Collections;

public class GameStats : MonoBehaviour 
{
    float SmoothFPS = 0;
    public float Smoothness = 0.99f;

    //============================================================================================================================================//
    void OnGUI()
    {
        if (!Game.Instance.Paused)
        {
            // Framerate //
            float fps = 1f / Time.deltaTime;
            SmoothFPS = SmoothFPS * Smoothness + fps * (1f - Smoothness);
            if (float.IsInfinity(SmoothFPS))
            {
                SmoothFPS = fps;
            }
            GUI.Label(new Rect(0, Screen.height - 20, 200, 20), "FPS: " + (int)SmoothFPS);
        }  
	}
}
