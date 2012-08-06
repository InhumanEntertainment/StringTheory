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
			
			Application.OpenURL ("itms-apps://userpub.itunes.apple.com/WebObjects/MZUserPublishing.woa/wa/addUserReview?id=337064413&type=Purple+Software");
			
			if (Application.platform == RuntimePlatform.IPhonePlayer) 
			{
				//InhumanIOS.ComposeEmail("ecl3d@hotmail.com", "Subject", "Body");
			}
		}
	}
}