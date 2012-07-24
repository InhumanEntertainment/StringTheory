using UnityEngine;
using System.Collections;

public class AnimEvents : MonoBehaviour 
{
    //=====================================================================================================================================//
    void Destroy() 
    {
        Destroy(transform.parent.gameObject);
	}
}
