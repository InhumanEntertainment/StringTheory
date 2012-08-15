using UnityEngine;
using System.Collections;

public class LevelButton : MonoBehaviour 
{
    public int Index = 0;   
    public UILabel Label;
    public UISlicedSprite Background;
    string LevelName;

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
            Game.Instance.LoadLevel(LevelName);
        }       
    }

    //============================================================================================================================================//
    public void UpdateButton()
    {
        Color color = Color.white;
        if (Index < Game.Instance.CurrentPack.Levels.Count)
        {
            LevelName = Game.Instance.CurrentPack.Levels[Index];
            StringTheoryLevel Level = Game.Instance.FindLevel(LevelName);

            if (Level.Completed)
                color = CompletedColor;

            if (Label != null)
                Label.text = Level.Name;
        }

        Background.color = color;
        Label.color = color * LabelColor;
    }    
}
