using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameScreen
{
    public string Name = "Default";
    //public string[] Levels;
    //public Color Color = Color.white;
    public GameAnim[] AnimationsOpen;
    public GameAnim[] AnimationsClose;
    //public Animation AnimationObject;

    public string FXMode;

    //============================================================================================================================================//
    // Plays all of the open animations and sets current fx mode //
    //============================================================================================================================================//
    public void Open(Game game)
    {
        if (Game.Instance.FX != null)
        {
            Game.Instance.FX.SetEffect(FXMode);

            for (int i = 0; i < AnimationsOpen.Length; i++)
            {
                AnimationsOpen[i].Play();
            }
        } 
    }

    //============================================================================================================================================//
    public void Close(Game game)
    {
        if (Game.Instance.FX != null)
        {
            for (int i = 0; i < AnimationsOpen.Length; i++)
            {
                AnimationsClose[i].Play();
            }
        }
    }
}
