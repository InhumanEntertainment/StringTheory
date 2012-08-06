using UnityEngine;
using System.Collections;

public class DimensionPickup : MonoBehaviour 
{
    //============================================================================================================================================//
    void OnTriggerEnter(Collider collider) 
    {
        if (collider.name == "Red" && this.name.Contains("Red"))
        {
            Destroy(this.gameObject); 
        }
        else if (collider.transform.name == "Blue" && this.name.Contains("Blue"))
        {
            Destroy(this.gameObject); 
        }
        else
        {
            print("Death");
        }   
    }
}
