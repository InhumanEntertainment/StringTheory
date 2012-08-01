using UnityEngine;
using System.Collections;

public class Frontend : MonoBehaviour
{
    public string Command;
    Game Game;
    Animation MenuAnimation;
    Animation AboutAnimation;
    Animation PacksAnimation;
    Animation GameAnimation;

    // Packs //
    Animation ChaosAnimation;
    Animation VortexAnimation;
    
    //=======================================================================================================================================================================//
    void Awake()
    {
        GameObject obj = GameObject.Find("Game");
        if (obj != null)
        {
            Game = obj.GetComponent<Game>();
        }

        MenuAnimation = GameObject.Find("MenuPanel").animation;
        PacksAnimation = GameObject.Find("PacksPanel").animation;
        ChaosAnimation = GameObject.Find("LevelsChaos").animation;
        VortexAnimation = GameObject.Find("LevelsVortex").animation;
        GameAnimation = GameObject.Find("GamePanel").animation;
    }

    //=======================================================================================================================================================================//
    void SwitchScreens(Game.GameScreen screen, Animation from, Animation to, string effect)
    {
        Game.SetScreen(screen);

        to.PlayQueued("Menu_Open");
        from.PlayQueued("Menu_Close");

        Game.FX.SetEffect(effect);
    }

    //=======================================================================================================================================================================//
    void OnClick()
    {
        if (Command == "About_Open")
        {
            Game.SetScreen(Game.GameScreen.About);
            // Play "Packs_Open" Animation //
        }
        else if (Command == "About_Close")
        {
            Game.SetScreen(Game.GameScreen.Menu);
            // Play "Packs_Open" Animation //
        }

        else if (Command == "Packs_Open")
        {
            ResetAboutSlider();

            Game.SetScreen(Game.GameScreen.Packs);
            PacksAnimation.PlayQueued("Menu_Open");
            MenuAnimation.PlayQueued("Menu_Close");

            Game.FX.SetEffect("Horizontal");
        }
        else if (Command == "Packs_Close")
        {
            Game.SetScreen(Game.GameScreen.Menu);
            PacksAnimation.PlayQueued("Menu_Close");
            MenuAnimation.PlayQueued("Menu_Open");

            Game.FX.SetEffect("BlackHole");
        }

        else if (Command == "Vortex_Open")
        {
            LoadScreen("Vortex");
        }
        else if (Command == "Pack_Close")
        {
            Game.SetScreen(Game.GameScreen.Packs);
            Game.CurrentPack.Close(Game);
            PacksAnimation.PlayQueued("Menu_Open");

            Game.FX.SetEffect("Horizontal");
        }
        else if (Command == "Vortex_Close")
        {
            SwitchScreens(Game.GameScreen.Packs, VortexAnimation, PacksAnimation, "Horizontal");
        }
        else if (Command == "Chaos_Open")
        {
            LoadScreen("Chaos");
        }
        else if (Command == "Chaos_Close")
        {
            SwitchScreens(Game.GameScreen.Packs, ChaosAnimation, PacksAnimation, "Horizontal");
        }
        else if (Command == "Starter_Open")
        {
            LoadScreen("Starter");
        }
        else if (Command == "Starter_Close")
        {
            SwitchScreens(Game.GameScreen.Packs, Game.CurrentPack.AnimationObject, PacksAnimation, "Horizontal");
        }
        else if (Command == "Game_Open")
        {
            SwitchScreens(Game.GameScreen.Game, Game.CurrentPack.AnimationObject, GameAnimation, "Game");
        }
        

        // Level Buttons //============================================================================//
        else if (Game.LevelList.Contains(Command))
        {
            Game.CurrentPack.Close(Game);
            Game.SetScreen(Game.GameScreen.Game);
            GameAnimation.PlayQueued("Menu_Open");

            if (Game != null)
            {
                Game.FX.SetEffect("Game");
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
            if (Game.CurrentLevel + 1 < Game.LevelList.Count)
            {
                ResetPauseSlider();
                Game.LoadLevel(Game.CurrentLevel + 1);
            }
        }
        else if (Command == "Prev")
        {
            if (Game.CurrentLevel > 0)
            {
                ResetPauseSlider();
                Game.LoadLevel(Game.CurrentLevel - 1);
            }
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
            Game.CurrentPack.Open(Game);
            Game.DestroyLevel(Game.CurrentLevel);
            GameAnimation.PlayQueued("Menu_Close");

            Game.FX.SetEffect(Game.CurrentPack.FXMode);
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

        Game.Log("Frontend Command: " + Command);
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

    //=======================================================================================================================================================================//
    void LoadScreen(string level)
    {
        // If pack name exists set current //
        for (int i = 0; i < Game.Packs.Length; i++)
        {
            if (Game.Packs[i].Name == level)
            {
                //Game.CurrentPack.Close(Game);   
                PacksAnimation.PlayQueued("Menu_Close");

                Game.CurrentPack = Game.Packs[i];
                Game.Packs[i].Open(Game);
            }
        }
    }
}