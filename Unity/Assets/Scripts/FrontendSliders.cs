using UnityEngine;
using System.Collections;

public class FrontendSliders : MonoBehaviour 
{
    public string Command;
    public AudioClip SoundTest;

    //=======================================================================================================================================================================//
    void OnSliderChange()
    {
        if (Command == "Music")
        {
            Audio.MusicVolume = GetComponent<UISlider>().sliderValue;
        }
        else if (Command == "Sound")
        {
            Audio.SoundVolume = GetComponent<UISlider>().sliderValue;
        }
    }

    //=======================================================================================================================================================================//
    void OnPress(bool pressed)
    {
        if (!pressed)
        {
            Audio.Play(SoundTest);
        }
    }
}
