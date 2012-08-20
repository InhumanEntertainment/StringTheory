using UnityEngine;
using System.Collections;

public class FrontendSliders : MonoBehaviour 
{
    public string Command;
    public AudioClip SoundTest;
    UISlider Slider;

    //=======================================================================================================================================================================//
    void Awake()
    {
        Slider = GetComponent<UISlider>();
    }

    //=======================================================================================================================================================================//
    void OnSliderChange()
    {
        if (Command == "Music" && Slider)
        {
            Audio.MusicVolume = Slider.sliderValue;
        }
        else if (Command == "Sound" && Slider)
        {
            Audio.SoundVolume = Slider.sliderValue;
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
