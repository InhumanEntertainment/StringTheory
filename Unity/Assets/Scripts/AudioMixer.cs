using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioMixer : MonoBehaviour 
{
    static public AudioMixer instance = null;
    public List<AudioSource> Sources = new List<AudioSource>();
    public List<AudioClip> Clips = new List<AudioClip>();
    public int NumberOfVoices = 8;

    //============================================================================================================================================//
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);

            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            for (int i = 0; i < NumberOfVoices; i++)
            {
                
            }
        }    
    }

    //============================================================================================================================================//
    public void Destroy()
    {
        instance = null;
        Destroy(this.gameObject);
    }

    //============================================================================================================================================//
    public void Play(AudioClip clip)
    {
        audio.PlayOneShot(clip);
    }
}
