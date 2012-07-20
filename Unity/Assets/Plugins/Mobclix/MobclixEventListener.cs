using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class MobclixEventListener : MonoBehaviour
{

	void OnEnable()
	{
		// Listen to all events for illustration purposes
		MobclixManager.adViewDidReceiveAdEvent += adViewDidReceiveAdEvent;
		MobclixManager.adViewFailedToReceiveAdEvent += adViewFailedToReceiveAdEvent;
		MobclixManager.fullScreenAdViewControllerDidFinishLoadEvent += fullScreenAdViewControllerDidFinishLoadEvent;
		MobclixManager.fullScreenAdViewControllerDidFailToLoadEvent += fullScreenAdViewControllerDidFailToLoadEvent;
		MobclixManager.fullScreenAdViewControllerDidDismissAdEvent += fullScreenAdViewControllerDidDismissAdEvent;
	}


	void OnDisable()
	{
		// Remove all event handlers
		MobclixManager.adViewDidReceiveAdEvent -= adViewDidReceiveAdEvent;
		MobclixManager.adViewFailedToReceiveAdEvent -= adViewFailedToReceiveAdEvent;
		MobclixManager.fullScreenAdViewControllerDidFinishLoadEvent -= fullScreenAdViewControllerDidFinishLoadEvent;
		MobclixManager.fullScreenAdViewControllerDidFailToLoadEvent -= fullScreenAdViewControllerDidFailToLoadEvent;
		MobclixManager.fullScreenAdViewControllerDidDismissAdEvent -= fullScreenAdViewControllerDidDismissAdEvent;
	}



	void adViewDidReceiveAdEvent()
	{
		Debug.Log( "adViewDidReceiveAdEvent" );
	}


	void adViewFailedToReceiveAdEvent( string error )
	{
		Debug.Log( "adViewFailedToReceiveAdEvent: " + error );
	}


	void fullScreenAdViewControllerDidFinishLoadEvent()
	{
		Debug.Log( "fullScreenAdViewControllerDidFinishLoadEvent" );
	}


	void fullScreenAdViewControllerDidFailToLoadEvent( string error )
	{
		Debug.Log( "fullScreenAdViewControllerDidFailToLoadEvent: " + error );
	}
	
	
	void fullScreenAdViewControllerDidDismissAdEvent()
	{
		Debug.Log( "fullScreenAdViewControllerDidDismissAdEvent" );
	}

}


