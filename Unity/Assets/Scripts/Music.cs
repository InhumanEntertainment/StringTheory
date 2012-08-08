using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour 
{
    private static Music instance = null;
    public static Music Instance
    {
        get { return instance; }
    }

    public AudioClip ButtonClick;

    //============================================================================================================================================//
    void Awake()
    {
        if (instance != null && instance != this)
        {
            AudioSource audio1 = instance.GetComponent<AudioSource>();
            AudioSource audio2 = GetComponent<AudioSource>();


            if (audio1.clip != audio2.clip)
            {
                Destroy(instance.gameObject);
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }

            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    //============================================================================================================================================//
    public void Destroy()
    {
        instance = null;
        Destroy(this.gameObject);
    }

    //============================================================================================================================================//
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Full Screen");
            Screen.fullScreen = !Screen.fullScreen;
        }
    }
    
}
