using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorBase : MonoBehaviour {
	
	GameObject curve;	
	
	//============================================================================================================================================//
	void Update () 
	{
		bool hasTouchStarted = (Input.GetMouseButtonDown(0));
		
		if (hasTouchStarted) {

			if (HasTouchedMe(Input.mousePosition)) 
			{
				if (!curve)
				{
					curve = (GameObject) Instantiate(Resources.Load("ColorString"));
				}
				else
				{
					Destroy(curve);
				}
			}
		}
	
	}
	
	//============================================================================================================================================//
	bool HasTouchedMe(Vector3 touchPosition)
    {
		Ray ray = Camera.main.ScreenPointToRay(touchPosition);
		RaycastHit hit;
		if (gameObject.collider.Raycast(ray,out hit,50.0f)) 
        {
			return true;
		}
        else
        {
			return false;
		}
	}
}
