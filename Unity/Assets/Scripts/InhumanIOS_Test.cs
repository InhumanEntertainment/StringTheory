using UnityEngine;
using System.Collections;

public class InhumanIOS_Test : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		InhumanIOS.Popup ("Startup");		
	}
	
	void Update () 
	{
		//InhumanIOS.Popup ("Time: " + Time.timeSinceLevelLoad);
		if (Input.GetMouseButtonDown(0)) 
		{
			Debug.Log ("Compose Email");
			
			if (Application.platform == RuntimePlatform.IPhonePlayer) 
			{
				InhumanIOS.ComposeEmail("ecl3d@hotmail.com", "Subject", "Body");
			}
		}
	}
}