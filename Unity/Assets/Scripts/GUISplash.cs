using UnityEngine;
using System.Collections;

public class GUISplash : MonoBehaviour
{
    public float MinLoadTime = 2;

    //============================================================================================================================================//  
    void Start()
    {
        StartTime = Time.timeSinceLevelLoad;
    }

    //============================================================================================================================================//  
    void Update()
    {
        if (Time.timeSinceLevelLoad - StartTime > MinLoadTime)
        {
            Application.LoadLevel(1);
        }        
    }

    //============================================================================================================================================//
    float StartTime;
    public float FadeTime = 1;
    public Color FadeColor = Color.white;
    public Texture White;

    /*void OnGUI()
    {
        if (Time.realtimeSinceStartup - StartTime < FadeTime)
        {
            GUI.color = new Color(FadeColor.r, FadeColor.g, FadeColor.b, (FadeTime - (Time.realtimeSinceStartup - StartTime)) / FadeTime);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), White, ScaleMode.StretchToFill, true);
            GUI.color = Color.white;
        }
    }*/
}
