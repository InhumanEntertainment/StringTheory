using UnityEngine;
using System.Collections;

public class GUISplash : MonoBehaviour
{
    AsyncOperation async;
    public float MinLoadTime = 2;
    public float progress = 0;
    public string[] LevelsToLoad = {"Menu", "Game"};
    int LoadIndex = 0;
    bool FinishedLoading = false;

    void Start()
    {
        StartTime = Time.timeSinceLevelLoad;
    }

    //============================================================================================================================================//  
    void Update()
    {
        UISlider slider = GetComponent<UISlider>();

        if (!Application.isLoadingLevel)
        {
            if (LoadIndex < LevelsToLoad.Length)
            {
                async = Application.LoadLevelAdditiveAsync(LevelsToLoad[LoadIndex]);
                print("Loading Level:" + LevelsToLoad[LoadIndex]);
                LoadIndex++; 
            }                   
        }

        if (async != null)
        {
            progress = ((float)LoadIndex / LevelsToLoad.Length) + async.progress * (1 / LevelsToLoad.Length);
            print("Progress: " + progress);          
        }
        else
        {
            //progress = Mathf.Min(1f, progress + (Mathf.Max(0, Random.Range(-8, 2)) * 2.5f) * Time.deltaTime / MinLoadTime);
            progress = 0;
        }

        if (!FinishedLoading && progress == 1)
        {
            FinishedLoading = true;
        }

        if (Time.timeSinceLevelLoad - StartTime > MinLoadTime)
        {
            // Destroy Splash Objects //
            Destroy(GameObject.Find("Splash").gameObject);
        }        

        slider.sliderValue = progress;
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
