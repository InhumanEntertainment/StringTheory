using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class InhumanIOS
{

	//============================================================================================================================================//
    [DllImport ("__Internal")]
	private static extern void _Popup (string text);
	
	[DllImport ("__Internal")]
	private static extern void _ComposeEmail (string to, string subject, string body);
	
	//============================================================================================================================================//
    public static void Popup(string text)
	{
		//if (Application.platform == RuntimePlatform.IPhonePlayer;
			_Popup(text);
	}
	
	//============================================================================================================================================//
    public static void ComposeEmail(string to, string subject, string body)
	{
		//if (Application.platform == RuntimePlatform.IPhonePlayer;
			_ComposeEmail(to, subject, body);
	}
}
