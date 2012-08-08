using UnityEngine;
using System.Collections;

public class Frontend : MonoBehaviour
{
    public string Command;
    Game Game;
    
    //=======================================================================================================================================================================//
    void Awake()
    {
        GameObject obj = GameObject.Find("Game");
        if (obj != null)
        {
            Game = obj.GetComponent<Game>();
        }
    }

    //=======================================================================================================================================================================//
    void OnClick()
    {
        Debug.Log("Frontend Command: " + Command);

        // Level Buttons //============================================================================//
        if (Command == "About_Open")
        {
            Game.SetScreen("About");
        }
        else if (Command == "About_Close")
        {
            Game.SetScreen("Menu");
        }

        // Level Buttons //============================================================================//
        else if (Command == "Packs_Open")
        {
            ResetAboutSlider();
            Game.SetScreen("Packs");
        }
        else if (Command == "Packs_Close")
        {
            Game.SetScreen("Menu");
        }

        // Level Packs //============================================================================//
        else if (Command == "Starter_Open")
        {
            Game.SetScreen("Starter");
        }
        else if (Command == "Vortex_Open")
        {
            Game.SetScreen("Vortex");
        }
        else if (Command == "Chaos_Open")
        {
            Game.SetScreen("Chaos");
        }
        else if (Command == "Pack_Close" || Command == "Starter_Close" || Command == "Vortex_Close" || Command == "Chaos_Close")
        {
            Game.SetScreen("Packs");
        }

        else if (Command == "Game_Open")
        {
            Game.SetScreen("Game");
        }
        
        // Level Buttons //============================================================================//
        else if (Game.LevelList.Contains(Command))
        {           
            if (Game != null)
            {
                Game.SetScreen("Game");
                Game.LoadLevel(Command);
            }  
        }

        // Social Buttons //============================================================================//
        else if (Command == "Facebook")
        {
            Application.OpenURL("http://www.facebook.com/larssontech/");
        }
        else if (Command == "Twitter")
        {
            Application.OpenURL("http://www.twitter.com/larssontech/");
        }

        // Game Buttons //============================================================================//
        else if (Command == "Next")
        {
            Game.NextLevel();
            ResetPauseSlider();
        }
        else if (Command == "Prev")
        {
            Game.PrevLevel();
            ResetPauseSlider();
        }
        else if (Command == "Retry")
        {
            Game.Retry();
            ResetPauseSlider();

            // Destroy Curves // 
        }
        else if (Command == "Resume")
        {
            ResetPauseSlider();
        }
        else if (Command == "Game_Close")
        {
            ResetPauseSlider();
            Game.DestroyLevel(Game.CurrentLevel);
            Game.SetScreen(Game.LastScreen.Name);          
        }

        // Level Completed //============================================================================//
        else if (Command == "Completed_Next")
        {
            Game.NextLevel();
            ResetPauseSlider();
            Game.FX.SetEffect("Game");
        }
        else if (Command == "Completed_Retry")
        {
            Game.Retry();
            ResetPauseSlider();
            Game.FX.SetEffect("Game");
        }

        // Menus //============================================================================//
        else if (Command == "InhumanEntertainment")
        {
            Application.OpenURL("http://www.inhumanentertainment.com");
        }
        else if (Command == "Sompom")
        {
            Application.OpenURL("http://www.sompon.com");
        }
        else if (Command == "Email")
        {
            Application.OpenURL("email:ecl3d@hotmail.com");
        }
        else if (Command == "Rate")
        {
            
        }     
	}

    //=======================================================================================================================================================================//
    public void ResetPauseSlider()
    {
        var obj = GameObject.Find("PauseSlider");
        var slider = obj.GetComponent<GameSlider>();
        slider.Target = slider.StartPosition;
        slider.MoveToTarget();
    }

    //=======================================================================================================================================================================//
    public void ResetAboutSlider()
    {
        var obj = GameObject.Find("AboutSlider");
        var slider = obj.GetComponent<GameSlider>();
        slider.Target = slider.StartPosition;
        slider.MoveToTarget();
    }
}