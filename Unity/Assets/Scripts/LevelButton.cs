using UnityEngine;
using System.Collections;

public class LevelButton : MonoBehaviour 
{
    public int Index = 0;   
    public UILabel Label;
    public UISlicedSprite Background;
    StringTheoryLevel Level;

    public Color LabelColor = Color.white;
    public Color LabelCompletedColor = Color.red;
    public Color ButtonColor = Color.white;
    public Color ButtonCompletedColor = Color.red;
    
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
            Game.Instance.LoadPackLevel(Index);
        }       
    }

    //============================================================================================================================================//
    public void UpdateButton()
    {
        Color labelColor = LabelColor;
        Color buttonColor = ButtonColor;
        
        if (Index < Game.Instance.CurrentPack.Levels.Count)
        {
            Level = Game.FindLevel(Game.Instance.CurrentPack.Levels[Index]);

            if (Level.Completed)
            {
                labelColor = LabelCompletedColor;
                buttonColor = ButtonCompletedColor;
            }

            if (Label != null)
                Label.text = Level.Name;
        }

        Background.color = buttonColor;
        Label.color = labelColor;
    }
}
