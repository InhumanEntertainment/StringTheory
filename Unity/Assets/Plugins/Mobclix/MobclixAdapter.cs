using UnityEngine;


public class MobclixAdapter : MonoBehaviour
{
	public bool bannerOnBottom = true;
	public float refreshRate = 30.0f;
	public string applicationId = string.Empty;
	

#if UNITY_IPHONE
	
	void Start()
	{
		MobclixBinding.start( applicationId );

		// center the banner.  this is a basic setup with centered banners
		if( isPad() )
			MobclixBinding.createBanner( MobclixBannerType.iPad_468x60, bannerOnBottom );
		else
			MobclixBinding.createBanner( MobclixBannerType.iPhone_320x50, bannerOnBottom );

		MobclixBinding.setRefreshTime( refreshRate );
		Destroy( gameObject );
	}

	
	bool isPad()
	{
		if( iPhone.generation == iPhoneGeneration.iPad1Gen || iPhone.generation == iPhoneGeneration.iPad2Gen || iPhone.generation == iPhoneGeneration.iPad3Gen )
			return true;
		
		return false;
	}
	
#endif
	
}
