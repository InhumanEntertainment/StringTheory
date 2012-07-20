using UnityEngine;
using System.Collections;
using System.Collections.Generic;



#if UNITY_ANDROID
public enum MobclixAndroidAd
{
	Size320x50,
	Size300x250
}


public class MobclixAndroid
{
	private static AndroidJavaObject _mobclixPlugin;
	
		
	static MobclixAndroid()
	{
		// find the plugin instance
		using( var pluginClass = new AndroidJavaClass( "com.prime31.MobclixPlugin" ) )
			_mobclixPlugin = pluginClass.CallStatic<AndroidJavaObject>( "instance" );
	}
	

	// Creates a banner either on the top or bottom of the screen
	public static void createBanner( MobclixAndroidAd type, bool bannerOnBottom )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_mobclixPlugin.Call( "createBanner", (int)type, bannerOnBottom );
	}


	// Destroys the banner if it is showing
	public static void destroyBanner()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_mobclixPlugin.Call( "destroyBanner" );
	}
	

	// Gets a new ad for the banner
	public static void getAd()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_mobclixPlugin.Call( "getAd" );
	}


	// Pauses the refreshing of the banner
	public static void pause()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_mobclixPlugin.Call( "pause" );
	}


	// Resumes refreshing of the banner if it was paused
	public static void resume()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_mobclixPlugin.Call( "resume" );
	}


	// Sets the refresh time for the banner
	public static void setRefreshTime( long refreshTime )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_mobclixPlugin.Call( "setRefreshTime", refreshTime );
	}	


	// Sets whether auto play is allowed
	public static void setAllowAutoplay( bool allowAutoplay )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_mobclixPlugin.Call( "setAllowAutoplay", allowAutoplay );
	}
	

	// Hides/shows the ad banner
	public static void hideBanner( bool shouldHide )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_mobclixPlugin.Call( "hideBanner", shouldHide );
	}


	// Rotates the ad banner about it's center
	public static void rotateAdView( float degrees )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_mobclixPlugin.Call( "rotateAdView", degrees );
	}	
	

	// Requests a full screen ad.  When it is loaded, the fullScreenAdDidLoadEvent event will be fired
	public static void requestFullScreenAd()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_mobclixPlugin.Call( "requestFullScreenAd" );
	}


	// Check to see if a full screen ad is ready to be displayed
	public static bool isFullScreenAdReady()
	{
		if( Application.platform != RuntimePlatform.Android )
			return false;
		
		return _mobclixPlugin.Call<bool>( "isFullScreenAdReady" );
	}


	// Displays a full screen ad
	public static void displayFullScreenAd()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_mobclixPlugin.Call( "displayFullScreenAd" );
	}


	// Requests a full screen ad and displays it as soon as it is loaded
	public static void requestAndDisplayFullScreenAd()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_mobclixPlugin.Call( "requestAndDisplayFullScreenAd" );
	}


}
#endif
