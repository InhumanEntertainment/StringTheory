using UnityEngine;
using System.Collections;

public class LevelButton : MonoBehaviour 
{
    public int Index = 0;   
    public UILabel Label;
    public UISlicedSprite Background;
    StringTheoryLevel Level;

    public Color LabelColor = Color.white;
    public Color CompletedColor = Color.red;

    //============================================================================================================================================//
    void Awake()
    {
        
	}

    //============================================================================================================================================//
    void OnPress(bool pressed)
    {
        if (pressed && Index < Game.Instance.CurrentPack.Levels.Count)
        {
            Game.Instance.SetScreen("Game");
            Game.Instance.LoadLevel(Level);
        }       
    }

    //============================================================================================================================================//
    public void UpdateButton()
    {
        Color color = Color.white;
        if (Index < Game.Instance.CurrentPack.Levels.Count)
        {
            Level = Game.FindLevel(Game.Instance.CurrentPack.Levels[Index]);

            if (Level.Completed)
                color = CompletedColor;

            if (Label != null)
                Label.text = Level.Name;
        }

        Background.color = color;
        Label.color = color * LabelColor;
    }
}
