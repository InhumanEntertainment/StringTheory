using UnityEngine;
using System.Collections;

public class AnimEvents : MonoBehaviour 
{
    //=====================================================================================================================================//
    void Destroy() 
    {
        Game.Instance.LevelIsTransitioning = false;
        Destroy(transform.parent.gameObject);       
	}
}
