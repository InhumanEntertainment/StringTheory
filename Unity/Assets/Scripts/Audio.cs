using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour 
{
    public static Audio Instance = null;
    public static AudioSource Music = null;
    public static AudioSource Sound = null;

    public Color ColorEnabled = new Color(1, 1, 1);
    public Color ColorDisabled = new Color(0.5f, 0.5f, 0.5f);

    public UISlider MusicSlider;
    public UILabel MusicLabel;
    public UISlider SoundSlider;
    public UILabel SoundLabel;
   
    public static bool _SoundMute = false;
    public static bool SoundMute
    {
        get { return _SoundMute; }
        set
        {
            _SoundMute = value;
            Sound.mute = value;
            Game.Instance.Data.Settings.SoundMute = value;
            Instance.SoundLabel.color = value ? Instance.ColorDisabled : Instance.ColorEnabled;

            if (value)
            {
                Instance.SoundSlider.sliderValue = 0;
                Instance.SoundSlider.collider.enabled = false;
            }
            else
            {
                Instance.SoundSlider.sliderValue = Sound.volume;
                Instance.SoundSlider.collider.enabled = true;
            }
        }
    }

    public static bool _MusicMute = false;
    public static bool MusicMute
    {
        get { return _MusicMute; }
        set
        {
            _MusicMute = value;
            Music.mute = value;
            Game.Instance.Data.Settings.MusicMute = value;
            Instance.MusicLabel.color = value ? Instance.ColorDisabled : Instance.ColorEnabled;

            if (value)
            {
                Instance.MusicSlider.sliderValue = 0;
                Instance.MusicSlider.collider.enabled = false;
            }
            else
            {
                Instance.MusicSlider.sliderValue = Music.volume;
                Instance.MusicSlider.collider.enabled = true;
            }
        }
    }
    
    public static float _SoundVolume = 1;
    public static float SoundVolume
    {
        get { return _SoundVolume; }
        set
        {
            if (!SoundMute)
            {
                _SoundVolume = value;
                Sound.volume = value;
                Game.Instance.Data.Settings.SoundVolume = value;
                Instance.SoundSlider.sliderValue = value;
            }          
        }
    }

    public static float _MusicVolume = 1;
    public static float MusicVolume
    {
        get { return _MusicVolume; }
        set
        {
            if (!MusicMute)
            {
                _MusicVolume = value;
                Music.volume = value;
                Game.Instance.Data.Settings.MusicVolume = value;
                Instance.MusicSlider.sliderValue = value;
            }
        }
    }
    public AudioClip[] Tracks;
    public int CurrentTrack = 0;

    //============================================================================================================================================//
    void Awake()
    {
        if (Music != null && Music != this)
        {           
            Destroy(this.gameObject);
        }
        else
        {
            var sources = GetComponents<AudioSource>();
            Music = sources[0];
            Sound = sources[1];
            Instance = this;

            DontDestroyOnLoad(this.gameObject);
        }       
    }

    //============================================================================================================================================//
    void Start()
    {
        MusicVolume = Game.Instance.Data.Settings.MusicVolume;
        SoundVolume = Game.Instance.Data.Settings.SoundVolume;
        MusicMute = Game.Instance.Data.Settings.MusicMute;
        SoundMute = Game.Instance.Data.Settings.SoundMute;
    }

    //============================================================================================================================================//
    public static void Play(AudioClip clip)
    {
        if (Sound != null)
	    {
            Sound.PlayOneShot(clip);
	    }       
    }

    //============================================================================================================================================//
    public void Destroy()
    {
        Music = null;
        Destroy(this.gameObject);
    }

    //============================================================================================================================================//
    public void Update()
    {
        // Switch tracks //
        if (Music.time >= Music.clip.length || !Music.isPlaying)
        {
            print("Audio: Next Track");

            CurrentTrack = (CurrentTrack < Tracks.Length - 1) ? CurrentTrack + 1 : 0;
            Music.clip = Tracks[CurrentTrack];
            Music.Play();
        }
    }
}