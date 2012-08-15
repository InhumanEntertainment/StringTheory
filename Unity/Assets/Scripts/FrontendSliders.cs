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
            //GameObject music = GameObject.Find("Music");
            //music.audio.volume = GetComponent<UISlider>().sliderValue;
            Audio.MusicVolume = GetComponent<UISlider>().sliderValue;
        }
        else if (Command == "Sound")
        {
            Audio.SoundVolume = GetComponent<UISlider>().sliderValue;
            //Audio.Play();
            // Play Sound Effect //
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
