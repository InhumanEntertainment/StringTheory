using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;


public enum MobclixLogLevel
{
	Debug = 0,
	Info = 1,
	Warn = 2,
	Error = 3,
	Fatal = 4
}


public enum MobclixBannerType
{
	iPhone_320x50,
	iPhone_300x250,
	iPad_300x250,
	iPad_728x90,
	iPad_120x600,
	iPad_468x60
}


public class MobclixBinding
{
    [DllImport("__Internal")]
    private static extern void _mobclixStart( string applicationId );

	// Starts up the Mobclix integration
    public static void start( string applicationId )
    {
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_mobclixStart( applicationId );
    }


    [DllImport("__Internal")]
    private static extern void _mobclixSetRefreshTime( float refreshTime );

	// Sets the rate at which ads refresh.
    public static void setRefreshTime( float refreshTime )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_mobclixSetRefreshTime( refreshTime );
    }


    [DllImport("__Internal")]
    private static extern void _mobclixCreateBanner( int bannerType, bool bannerOnBottom );

	// Creates a banner of the given type either on the top or bottom of the screen
    public static void createBanner( MobclixBannerType bannerType, bool bannerOnBottom )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_mobclixCreateBanner( (int)bannerType, bannerOnBottom );
    }


    [DllImport("__Internal")]
    private static extern void _mobclixShowBanner();
 
 	// Shows the banner if it is hidden.  The banner must first be created with createBanner
    public static void showBanner()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_mobclixShowBanner();
    }
	
	
    [DllImport("__Internal")]
    private static extern void _mobclixHideBanner( bool shouldDestroy );

	// Hides the banner optionally destroying it completely.  The banner must be destroyed if you want to change
	// the MobclixBannerType
    public static void hideBanner( bool shouldDestroy )
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_mobclixHideBanner( shouldDestroy );
    }
	
	
    [DllImport("__Internal")]
    private static extern void _mobclixRequestFullScreenAd();
 
 	// Starts loading a new full screen ad
    public static void requestFullScreenAd()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_mobclixRequestFullScreenAd();
    }
    
    
    [DllImport("__Internal")]
    private static extern void _mobclixDisplayFullScreenAd();
 
 	// Displays a full screen ad if one has been loaded
    public static void displayFullScreenAd()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_mobclixDisplayFullScreenAd();
    }


    [DllImport("__Internal")]
    private static extern void _mobclixRequestAndDisplayFullScreenAd();
 
 	// Requests and displays a full screen ad as soon as it is loaded
    public static void requestAndDisplayFullScreenAd()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_mobclixRequestAndDisplayFullScreenAd();
    }


    [DllImport("__Internal")]
    private static extern bool _mobclixIsFullScreenAdReady();
 
 	// Checks to see if a full screen ad is ready to be displayed
    public static bool isFullScreenAdReady()
    {
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _mobclixIsFullScreenAdReady();
		return false;
    }
}