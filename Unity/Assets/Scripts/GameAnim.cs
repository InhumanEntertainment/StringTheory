using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameAnim
{
    public string Animation = "Idle";
    public Animation Target;

    //============================================================================================================================================//
    public void Play()
    {
        Target.PlayQueued(Animation);
    }
}
