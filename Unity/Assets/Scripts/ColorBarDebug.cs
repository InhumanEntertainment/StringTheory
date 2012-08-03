using UnityEngine;
using System.Collections;

public class ColorBarDebug : MonoBehaviour {

	public ColorBar ColorBar;
	
	void OnGUI() 
    {
		if (GUI.Button (new Rect (0,0,250,100), " Reset ColorBar")) 
		{
			ColorBar.ResetColorBar();
		}
		
	}
}
