using UnityEngine;
using System.Collections.Generic;


public class MobclixGUIManager : MonoBehaviour
{
	void OnStart()
	{
		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}
	
	
	void OnGUI()
	{
		float yPos = 5.0f;
		float xPos = 20.0f;
		float width = 210.0f;
		float height = 40.0f;
		float heightPlus = 45.0f;
		
		
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Initialize Mobclix" ) )
		{
			MobclixBinding.start( "insert-your-application-key" );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Show Banner" ) )
		{
			MobclixBinding.showBanner();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Hide Banner" ) )
		{
			MobclixBinding.hideBanner( false );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Request Full Screen Ad" ) )
		{
			MobclixBinding.requestFullScreenAd();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Is Full Screen Ad Loaded?" ) )
		{
			Debug.Log( "is full screen ad ready? " + MobclixBinding.isFullScreenAdReady() );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Display Full Screen Ad" ) )
		{
			MobclixBinding.displayFullScreenAd();
		}


		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Request and Display Full Screen" ) )
		{
			MobclixBinding.requestAndDisplayFullScreenAd();
		}
		
		
		xPos = Screen.width - width - 20.0f;
		yPos = 5.0f;
		
		if( iPhone.generation != iPhoneGeneration.iPad1Gen && iPhone.generation != iPhoneGeneration.iPad2Gen && iPhone.generation != iPhoneGeneration.iPad3Gen )
		{
			if( GUI.Button( new Rect( xPos, yPos, width, height ), "320x50 Banner on Bottom" ) )
			{
				MobclixBinding.createBanner( MobclixBannerType.iPhone_320x50, true );
			}
		
			if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "320x250 Banner on Top" ) )
			{
				MobclixBinding.createBanner( MobclixBannerType.iPhone_300x250, false );
			}
		}
		else
		{
			if( GUI.Button( new Rect( xPos, yPos, width, height ), "120x600 Banner" ) )
			{
				MobclixBinding.createBanner( MobclixBannerType.iPad_120x600, false );
			}
			
			
			if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "468x60 Banner" ) )
			{
				MobclixBinding.createBanner( MobclixBannerType.iPad_468x60, false );
			}
			
			
			if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "728x90 Banner" ) )
			{
				MobclixBinding.createBanner( MobclixBannerType.iPad_728x90, false );
			}
			
			
			if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "300x250 Banner" ) )
			{
				MobclixBinding.createBanner( MobclixBannerType.iPad_300x250, false );
			}
		}
	}


}
