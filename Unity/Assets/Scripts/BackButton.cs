using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour 
{
    public KeyCode Key = KeyCode.Escape;
    public string Level;
    public int FrameFreq = 30;

    //============================================================================================================================================//
    void Start()
    {
	
	}

    //============================================================================================================================================//
    void Update()
    {
        if (Input.GetKeyDown(Key))
        {
            if (Level.ToLower() == "exit")
            {
                Application.Quit();
            }
            else
                Application.LoadLevel(Level);
        }

        if (Time.frameCount % FrameFreq == 0)
            System.GC.Collect(); 


	}
}
