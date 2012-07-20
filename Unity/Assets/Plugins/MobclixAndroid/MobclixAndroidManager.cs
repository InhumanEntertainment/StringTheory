using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class MobclixAndroidManager : MonoBehaviour
{
#if UNITY_ANDROID
	public delegate void MobclixAndroidEmptyDelegate();
	public delegate void MobclixAndroidErrorDelegate( string error );

	// Fired when the user touches an ad
	public static event MobclixAndroidEmptyDelegate adWasClickedEvent;
	
	// Fired when an ad failes to be loaded
	public static event MobclixAndroidErrorDelegate adDidFailEvent;
	
	// Fired when an open allocation is loaded
	public static event MobclixAndroidEmptyDelegate openAllocationLoadedEvent;
	
	// Fired when an ad loads
	public static event MobclixAndroidEmptyDelegate adDidLoadEvent;
	
	// Fired when a full screen ad is shown
	public static event MobclixAndroidEmptyDelegate fullScreenAdShownEvent;
	
	// Fired when a full screen ad is dismissed
	public static event MobclixAndroidEmptyDelegate fullScreenAdDismissedEvent;
	
	// Fired when a full screen ad fails to load
	public static event MobclixAndroidErrorDelegate fullScreenAdDidFailEvent;
	
	// Fired when a full screen ad is loaded and ready to be displayed
	public static event MobclixAndroidEmptyDelegate fullScreenAdDidLoadEvent;


	void Awake()
	{
		// Set the GameObject name to the class name for easy access from Obj-C
		gameObject.name = this.GetType().ToString();
		DontDestroyOnLoad( this );
	}


	public void adWasClicked( string empty )
	{
		if( adWasClickedEvent != null )
			adWasClickedEvent();
	}


	public void adDidFail( string error )
	{
		if( adDidFailEvent != null )
			adDidFailEvent( error );
	}


	public void openAllocationLoaded( string empty )
	{
		if( openAllocationLoadedEvent != null )
			openAllocationLoadedEvent();
	}


	public void adDidLoad( string empty )
	{
		if( adDidLoadEvent != null )
			adDidLoadEvent();
	}


	public void fullScreenAdShown( string empty )
	{
		if( fullScreenAdShownEvent != null )
			fullScreenAdShownEvent();
	}


	public void fullScreenAdDismissed( string empty )
	{
		if( fullScreenAdDismissedEvent != null )
			fullScreenAdDismissedEvent();
	}


	public void fullScreenAdDidFail( string error )
	{
		if( fullScreenAdDidFailEvent != null )
			fullScreenAdDidFailEvent( error );
	}


	public void fullScreenAdDidLoad( string empty )
	{
		if( fullScreenAdDidLoadEvent != null )
			fullScreenAdDidLoadEvent();
	}
#endif
}

