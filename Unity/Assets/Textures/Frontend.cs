using UnityEngine;
using System.Collections;

public class Frontend : MonoBehaviour
{
    public string Command;

    //=======================================================================================================================================================================//
    void OnPress()
    {
        if (Command == "Play")
        {
            Application.LoadLevel("Prototype");
        }
	}
}
