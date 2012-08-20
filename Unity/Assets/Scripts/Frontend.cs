using UnityEngine;
using UnityEngine.SocialPlatforms;
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
            Game.SetPack("Starter");
        }
        else if (Command == "Vortex_Open")
        {
            Game.SetScreen("Vortex");
            Game.SetPack("Vortex");           
        }
        else if (Command == "Chaos_Open")
        {
            Game.SetScreen("Chaos");
            Game.SetPack("Chaos");
        }
        else if (Command == "Pack_Close" || Command == "Starter_Close" || Command == "Vortex_Close" || Command == "Chaos_Close")
        {
            Game.SetScreen("Packs");
        }

        else if (Command == "Game_Open")
        {
            Game.SetScreen("Game");
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
            Game.FX.SetEffect("Game");
        }
        else if (Command == "Prev")
        {
            Game.PrevLevel();
            ResetPauseSlider();
            Game.FX.SetEffect("Game");
        }
        else if (Command == "Retry")
        {
            Game.Retry();
            ResetPauseSlider();
            Game.FX.SetEffect("Game");
        }
        else if (Command == "Resume")
        {
            ResetPauseSlider();
        }
        else if (Command == "Game_Close")
        {
            ResetPauseSlider();
            Game.Instance.UpdateButtons();
            Game.SetPack(Game.Instance.CurrentPack.Name);          
            Game.DestroyLevel(Game.CurrentLevel);
            Game.SetScreen(Game.LastScreen.Name);          
        }

        // Menus //============================================================================//
        else if (Command == "Inhuman")
        {
            Application.OpenURL("http://www.inhumanentertainment.com");
        }
        else if (Command == "Sompom")
        {
            Application.OpenURL("http://www.sompon.com");
        }
        else if (Command == "Reset")
        {
            Game.Reset();
        }
        else if (Command == "Email")
        { 
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                InhumanIOS.ComposeEmail("ecl3d@hotmail.com", "String Theory Support", "");
            }
            else
            { 
                
            }
            
        }
        else if (Command == "Rate")
        {
            Application.OpenURL("itms-apps://ax.itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id=337064413");
        }
        else if (Command == "SoundMute")
        {
            Audio.SoundMute = !Audio.SoundMute;
        }
        else if (Command == "MusicMute")
        {
            Audio.MusicMute = !Audio.MusicMute;
        } 
		else if (Command == "Achievements")
        {
			IAchievement a1 = Social.CreateAchievement ();
			a1.id = "firstplay";
			a1.percentCompleted = 100;
			a1.ReportProgress(ReportCallback);
			
			//Social.ReportScore (5345.97, "");
            Social.ShowAchievementsUI();
        }
	}
	
	//=======================================================================================================================================================================//
    void ReportCallback(bool success)
	{
		//InhumanIOS.Popup ("Report", success.ToString (), "Ok");	
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