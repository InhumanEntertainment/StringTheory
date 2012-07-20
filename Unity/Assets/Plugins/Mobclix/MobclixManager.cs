using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class MobclixManager : MonoBehaviour
{
	// Fired when a new ad is received
	public static event Action adViewDidReceiveAdEvent;
	
	// Fired when an ad banner fails to load
	public static event Action<string> adViewFailedToReceiveAdEvent;
	
	// Fired when a full screen ad is loaded
	public static event Action fullScreenAdViewControllerDidFinishLoadEvent;
	
	// Fired when a full screen ad fails to load
	public static event Action<string> fullScreenAdViewControllerDidFailToLoadEvent;
	
	// Fired when a full screen ad is dismissed
	public static event Action fullScreenAdViewControllerDidDismissAdEvent;
	


	void Awake()
	{
		// Set the GameObject name to the class name for easy access from Obj-C
		gameObject.name = this.GetType().ToString();
		DontDestroyOnLoad( this );
	}


	public void adViewDidReceiveAd( string empty )
	{
		if( adViewDidReceiveAdEvent != null )
			adViewDidReceiveAdEvent();
	}


	public void adViewFailedToReceiveAd( string error )
	{
		if( adViewFailedToReceiveAdEvent != null )
			adViewFailedToReceiveAdEvent( error );
	}


	public void fullScreenAdViewControllerDidFinishLoad( string empty )
	{
		if( fullScreenAdViewControllerDidFinishLoadEvent != null )
			fullScreenAdViewControllerDidFinishLoadEvent();
	}


	public void fullScreenAdViewControllerDidFailToLoad( string error )
	{
		if( fullScreenAdViewControllerDidFailToLoadEvent != null )
			fullScreenAdViewControllerDidFailToLoadEvent( error );
	}
	
	
	public void fullScreenAdViewControllerDidDismissAd( string empty )
	{
		if( fullScreenAdViewControllerDidDismissAdEvent != null )
			fullScreenAdViewControllerDidDismissAdEvent();
	}

}

