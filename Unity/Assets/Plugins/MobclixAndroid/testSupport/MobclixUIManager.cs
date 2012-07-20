using UnityEngine;
using System.Collections.Generic;

#if UNITY_ANDROID

public class MobclixUIManager : MonoBehaviour
{
	void OnGUI()
	{
		float yPos = 5.0f;
		float xPos = 5.0f;
		float width = ( Screen.width >= 800 || Screen.height >= 800 ) ? 320 : 160;
		float height = ( Screen.width >= 800 || Screen.height >= 800 ) ? 70 : 35;
		float heightPlus = height + 10.0f;
		

	
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Create 320x50 banner on Bottom" ) )
		{
			// center it on the bottom
			MobclixAndroid.createBanner( MobclixAndroidAd.Size320x50, true );
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Create 300x250 banner on Top" ) )
		{
			// center it on the top
			MobclixAndroid.createBanner( MobclixAndroidAd.Size300x250, false );
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Destroy Banner" ) )
		{
			MobclixAndroid.destroyBanner();
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Get Ad" ) )
		{
			MobclixAndroid.getAd();
		}

	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Hide Banner" ) )
		{
			MobclixAndroid.hideBanner( true );
		}


	
		xPos = Screen.width - width - 5.0f;
		yPos = 5.0f;
		
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Request Full Screen Ad" ) )
		{
			MobclixAndroid.requestFullScreenAd();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Is Full Screen Ad Ready?" ) )
		{
			var isReady = MobclixAndroid.isFullScreenAdReady();
			Debug.Log( "is full screen ready? " + isReady );
		}
	

		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Display Full Screen Ad" ) )
		{
			MobclixAndroid.displayFullScreenAd();
		}


		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Request/Display Full Screen Ad" ) )
		{
			MobclixAndroid.requestAndDisplayFullScreenAd();
		}


		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Show Banner" ) )
		{
			MobclixAndroid.hideBanner( false );
		}
	}


}

#endif