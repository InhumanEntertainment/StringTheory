using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class MobclixAndroidEventListener : MonoBehaviour
{
#if UNITY_ANDROID
	void OnEnable()
	{
		// Listen to all events for illustration purposes
		MobclixAndroidManager.adWasClickedEvent += adWasClickedEvent;
		MobclixAndroidManager.adDidFailEvent += adDidFailEvent;
		MobclixAndroidManager.openAllocationLoadedEvent += openAllocationLoadedEvent;
		MobclixAndroidManager.adDidLoadEvent += adDidLoadEvent;
		MobclixAndroidManager.fullScreenAdShownEvent += fullScreenAdShownEvent;
		MobclixAndroidManager.fullScreenAdDismissedEvent += fullScreenAdDismissedEvent;
		MobclixAndroidManager.fullScreenAdDidFailEvent += fullScreenAdDidFailEvent;
		MobclixAndroidManager.fullScreenAdDidLoadEvent += fullScreenAdDidLoadEvent;
	}


	void OnDisable()
	{
		// Remove all event handlers
		MobclixAndroidManager.adWasClickedEvent -= adWasClickedEvent;
		MobclixAndroidManager.adDidFailEvent -= adDidFailEvent;
		MobclixAndroidManager.openAllocationLoadedEvent -= openAllocationLoadedEvent;
		MobclixAndroidManager.adDidLoadEvent -= adDidLoadEvent;
		MobclixAndroidManager.fullScreenAdShownEvent -= fullScreenAdShownEvent;
		MobclixAndroidManager.fullScreenAdDismissedEvent -= fullScreenAdDismissedEvent;
		MobclixAndroidManager.fullScreenAdDidFailEvent -= fullScreenAdDidFailEvent;
		MobclixAndroidManager.fullScreenAdDidLoadEvent -= fullScreenAdDidLoadEvent;
	}



	void adWasClickedEvent()
	{
		Debug.Log( "adWasClickedEvent" );
	}


	void adDidFailEvent( string error )
	{
		Debug.Log( "adDidFailEvent: " + error );
	}


	void openAllocationLoadedEvent()
	{
		Debug.Log( "openAllocationLoadedEvent" );
	}


	void adDidLoadEvent()
	{
		Debug.Log( "adDidLoadEvent" );
	}


	void fullScreenAdShownEvent()
	{
		Debug.Log( "fullScreenAdShownEvent" );
	}


	void fullScreenAdDismissedEvent()
	{
		Debug.Log( "fullScreenAdDismissedEvent" );
	}


	void fullScreenAdDidFailEvent( string error )
	{
		Debug.Log( "fullScreenAdDidFailEvent: " + error );
	}


	void fullScreenAdDidLoadEvent()
	{
		Debug.Log( "fullScreenAdDidLoadEvent" );
	}
#endif
}


