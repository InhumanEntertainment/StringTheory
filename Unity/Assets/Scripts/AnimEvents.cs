using UnityEngine;
using System.Collections;

public class AnimEvents : MonoBehaviour 
{
    //=====================================================================================================================================//
    void Destroy() 
    {
        Destroy(transform.parent.gameObject);       
	}

    //=====================================================================================================================================//
    void Disable()
    {
        gameObject.SetActiveRecursively(false);
    }

    //=====================================================================================================================================//
    void Enable()
    {
        gameObject.SetActiveRecursively(true);
    }
}
