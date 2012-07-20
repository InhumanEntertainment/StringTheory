using UnityEngine;
using System.Collections;


public class MobclixAndroidAdapter : MonoBehaviour
{
#if UNITY_ANDROID
	void Start()
	{
		// center the banner.  this is a basic setup with centered banners
		if( isTablet() )
			MobclixAndroid.createBanner( MobclixAndroidAd.Size300x250, true );
		else
			MobclixAndroid.createBanner( MobclixAndroidAd.Size320x50, true );
	}

	
	bool isTablet()
	{
		// check for tablet sized screens
		if( Screen.width >= 1000 || Screen.height >= 1000 )
			return true;
		
		return false;
	}
#endif
}
