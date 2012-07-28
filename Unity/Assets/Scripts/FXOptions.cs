using UnityEngine;
using System.Collections;

public class FXOptions : MonoBehaviour 
{
    //============================================================================================================================================//
    void LateUpdate() 
    { 
        if (particleSystem != null && !particleSystem.IsAlive())  
            Destroy(this.gameObject); 
    }
}
