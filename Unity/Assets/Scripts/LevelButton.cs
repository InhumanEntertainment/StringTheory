using UnityEngine;
using System.Collections;

public class LevelButton : MonoBehaviour 
{
    public string Level = "Level";
    public string Title = "Title";
    public string Number = "1";

    public UILabel LabelObject;

    //============================================================================================================================================//
    void Awake()
    {
	    // If level completed change texture //

        // Set Values //
        LabelObject.text = Number;
	}

    //============================================================================================================================================//
    void OnClick()
    {
        Levels levels = (Levels)GameObject.FindObjectOfType(typeof(Levels));
        levels.LoadLevel(Level);

        // Transition out of levels screen //
	}
}
