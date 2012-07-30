using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GamePack
{
    public string Name = "Default";
    public string[] Levels;
    public Color Color = Color.white;
    public GameAnim[] AnimationsOpen;
    public GameAnim[] AnimationsClose;
    public Animation AnimationObject;

    public FXStars.ParticleMode FXMode;

    //============================================================================================================================================//
    // Plays all of the open animations and sets current fx mode //
    //============================================================================================================================================//
    public void Open(Game game)
    {
        game.FX.Mode = FXMode;
        game.SetScreen(Game.GameScreen.Packs);
        
        for (int i = 0; i < AnimationsOpen.Length; i++)
        {
            AnimationsOpen[i].Play();
        }
    }

    //============================================================================================================================================//
    public void Close(Game game)
    {
        for (int i = 0; i < AnimationsOpen.Length; i++)
        {
            AnimationsClose[i].Play();
        }
    }
}
