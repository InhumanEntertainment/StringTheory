using UnityEngine;
using System.Collections;

public class LevelButton : MonoBehaviour 
{
    public string Title = "Title";
    public string Number = "1";
    Game Game;
    
    public UILabel LabelObject;

    //============================================================================================================================================//
    void Awake()
    {
        // Set Values //
        LabelObject.text = Number;
	}
}
