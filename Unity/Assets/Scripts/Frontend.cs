using UnityEngine;
using System.Collections;

public class Frontend : MonoBehaviour
{
    public string Command;
    Game Game;
    Animation MenuAnimation;
    Animation AboutAnimation;
    Animation PacksAnimation;
    Animation LevelsAnimation;
    Animation GameAnimation;

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
        LevelsAnimation = GameObject.Find("LevelsPanel").animation;
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
            Game.SetScreen(Game.GameScreen.Packs);
            PacksAnimation.PlayQueued("Menu_Open");
            MenuAnimation.PlayQueued("Menu_Close");
        }
        else if (Command == "Packs_Close")
        {
            Game.SetScreen(Game.GameScreen.Menu);
            PacksAnimation.PlayQueued("Menu_Close");
            MenuAnimation.PlayQueued("Menu_Open");
        }
        else if (Command == "Levels_Open")
        {
            Game.SetScreen(Game.GameScreen.Levels);
            LevelsAnimation.PlayQueued("Menu_Open");
            PacksAnimation.PlayQueued("Menu_Close");
        }
        else if (Command == "Levels_Close")
        {
            Game.SetScreen(Game.GameScreen.Packs);
            PacksAnimation.PlayQueued("Menu_Open");
            LevelsAnimation.PlayQueued("Menu_Close");
        }
        else if (Command == "Game_Open")
        {
            Game.SetScreen(Game.GameScreen.Game);
            // Play "Packs_Open" Animation //
        }
        else if (Command == "Game_Close")
        {
            Game.SetScreen(Game.GameScreen.Levels);
            // Play "Packs_Open" Animation //
        }

        // Level Buttons //============================================================================//
        else if (Command.Contains("Level"))
        {
            LevelsAnimation.PlayQueued("Menu_Close");
            Game.SetScreen(Game.GameScreen.Game);

            if (Game != null)
            {
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
                Game.LoadLevel(Game.CurrentLevel + 1);
            }
        }
        else if (Command == "Prev")
        {
            if (Game.CurrentLevel > 0)
            {
                Game.LoadLevel(Game.CurrentLevel - 1);
            }
        }
        else if (Command == "Retry")
        {
            var curves = GameObject.FindObjectsOfType(typeof(ColorString));
            for (int i = 0; i < curves.Length; i++)
            {
                Destroy(((ColorString)curves[i]).gameObject);
            }
        }
        else if (Command == "levels")
        {
            Game.DestroyCurrentLevel(Game.CurrentLevel);
            Game.DestroyCurrentLevel(Game.LastLevel);
            Game.SetScreen(Game.GameScreen.Menu);
        }

	}
}
