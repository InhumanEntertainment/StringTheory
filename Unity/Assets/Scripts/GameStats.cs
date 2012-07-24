using UnityEngine;
using System.Collections;

public class GameStats : MonoBehaviour 
{
    float SmoothFPS = 0;
    public float Smoothness = 0.99f;

    //============================================================================================================================================//
    void OnGUI()
    {
        // Framerate //
        float fps = 1f / Time.deltaTime;
        SmoothFPS = SmoothFPS * Smoothness + fps * (1f - Smoothness);
        GUI.Label(new Rect(0, Screen.height - 20, 100, 20), "FPS: " + (int)SmoothFPS);

        // Strings //
        /*var strings = GameObject.FindObjectsOfType(typeof(ColorString));
        var index = 0;
        for (int i = 0; i < strings.Length; i++)
        {
            index = strings.Length - i;
            GUI.Label(new Rect(0, 20 * (index - 1), 200, 100), "String " + index + ": " + ((ColorString)strings[i]).Tail.Count);
        }   */    
	}
}
