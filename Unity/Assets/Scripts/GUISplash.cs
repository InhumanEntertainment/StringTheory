using UnityEngine;
using System.Collections;

public class GUISplash : MonoBehaviour
{
    AsyncOperation async;
    public float MinLoadTime = 2;
    public float progress = 0;

    //============================================================================================================================================//
    void Start()
    {
        StartTime = Time.realtimeSinceStartup;
    }

    //============================================================================================================================================//  
    void Update()
    {
        UISlider slider = GetComponent<UISlider>();

        if (!Application.isLoadingLevel && Time.timeSinceLevelLoad > MinLoadTime)
        {
            StartLevel();
        }

        if (async != null)
        {
            //progress = Mathf.Min(1, Time.timeSinceLevelLoad / MinLoadTime) * 0.5f + async.progress * 0.5f;
            progress = async.progress;
        }
        else
        {
            //progress = Mathf.Min(1f, progress + (Mathf.Max(0, Random.Range(-8, 2)) * 2.5f) * Time.deltaTime / MinLoadTime);
            progress = 0;
        }

        slider.sliderValue = progress;

    }

    //============================================================================================================================================//  
    void StartLevel()
    {
        async = Application.LoadLevelAsync("Menu");
    }

    //============================================================================================================================================//
    float StartTime;
    public float FadeTime = 1;
    public Color FadeColor = Color.white;
    public Texture White;

    void OnGUI()
    {
        if (Time.realtimeSinceStartup - StartTime < FadeTime)
        {
            GUI.color = new Color(FadeColor.r, FadeColor.g, FadeColor.b, (FadeTime - (Time.realtimeSinceStartup - StartTime)) / FadeTime);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), White, ScaleMode.StretchToFill, true);
            GUI.color = Color.white;
        }
    }
}
