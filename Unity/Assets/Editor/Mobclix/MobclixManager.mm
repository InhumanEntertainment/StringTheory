//
//  MobClixManager.m
//  MobClixTest
//
//  Created by Mike on 8/22/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import "MobclixManager.h"
#import "Mobclix.h"


void UnityPause( bool pause );

UIViewController *UnityGetGLViewController();



@implementation MobclixManager

@synthesize adView = _adView, fullScreenAdViewController = _fullScreenAdViewController;

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSObject

+ (MobclixManager*)sharedManager
{
	static MobclixManager *sharedSingleton;
	
	if( !sharedSingleton )
		sharedSingleton = [[MobclixManager alloc] init];
		
		return sharedSingleton;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark MobclixAdViewDelegate

- (void)adViewDidFinishLoad:(MobclixAdView*)adView
{
	UnitySendMessage( "MobclixManager", "adViewDidReceiveAd", "" );
}


- (void)adView:(MobclixAdView*)adView didFailLoadWithError:(NSError*)error
{
	UnitySendMessage( "MobclixManager", "adViewFailedToReceiveAd", [error localizedDescription].UTF8String );
}


- (void)adViewWillTouchThrough:(MobclixAdView*)adView
{
	UnityPause( true );
}


- (void)adViewDidFinishTouchThrough:(MobclixAdView*)adView
{
	UnityPause( false );
}


- (NSString*)adView:(MobclixAdView*)adView publisherKeyForSuballocationRequest:(MCAdsSuballocationType)suballocationType
{
	static NSString *adMobPublisherId = nil;
	
	// are we setup for AdMob?
	if( adMobPublisherId == nil )
	{
		NSString *infoPath = [[NSBundle mainBundle] pathForResource:@"Info.plist" ofType:nil];
		NSDictionary *dict = [NSDictionary dictionaryWithContentsOfFile:infoPath];
		
		// if we are iPad, attempt to load up that publisherId first
		if( UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad )
		{
			// if we have the key and it isn't empty load it up
			if( [[dict allKeys] containsObject:@"AMiPadPublisherId"] && ![[dict objectForKey:@"AMiPadPublisherId"] isEqualToString:@""] )
			{
				adMobPublisherId = [[dict objectForKey:@"AMiPadPublisherId"] retain];
				NSLog( @"using iPad publisher Id: %@", adMobPublisherId );
			}
		}
		
		// we either are not an iPad or we didnt find a key for iPad
		if( adMobPublisherId == nil )
		{
			if( [[dict allKeys] containsObject:@"AMPublisherId"] )
			{
				adMobPublisherId = [[dict objectForKey:@"AMPublisherId"] retain];
				NSLog( @"using iPhone publisher Id: %@", adMobPublisherId );
			}
			else
			{
				adMobPublisherId = [@"" retain];
			}
		}
	}
	
	NSLog( @"suballocate: %i", suballocationType );
	
	// AdMob integration
	if( suballocationType == kMCAdsSuballocationAdMob && ![adMobPublisherId isEqual:@""] )
		return adMobPublisherId;
	
	return @"";
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark MobclixFullScreenAdDelegate

- (void)fullScreenAdViewControllerDidFinishLoad:(MobclixFullScreenAdViewController*)fullScreenAdViewController
{
	UnitySendMessage( "MobclixManager", "fullScreenAdViewControllerDidFinishLoad", "" );
}


// This is called when an ad failed to load.  The error codes will match up with the codes in MobclixAdView.h
- (void)fullScreenAdViewController:(MobclixFullScreenAdViewController*)fullScreenAdViewController didFailToLoadWithError:(NSError*)error
{
	UnitySendMessage( "MobclixManager", "fullScreenAdViewControllerDidFailToLoad", [error localizedDescription].UTF8String );
}


- (void)fullScreenAdViewControllerWillPresentAd:(MobclixFullScreenAdViewController*)fullScreenAdViewController
{
	UnityPause( true );
}


- (void)fullScreenAdViewControllerDidDismissAd:(MobclixFullScreenAdViewController*)fullScreenAdViewController
{
	// fix the view controller position
	UnityPause( false );
	UnitySendMessage( "MobclixManager", "fullScreenAdViewControllerDidDismissAd", "" );
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Private

- (void)adjustAdViewFrameToShowAdView
{
	// handle y-axis
	CGRect origFrame = _adView.frame;
	
	CGFloat screenHeight = [UIScreen mainScreen].bounds.size.height;
	CGFloat screenWidth = [UIScreen mainScreen].bounds.size.width;
	
	if( UIInterfaceOrientationIsLandscape( UnityGetGLViewController().interfaceOrientation ) )
	{
		screenWidth = screenHeight;
		screenHeight = [UIScreen mainScreen].bounds.size.width;
	}
	
	
	if( _adBannerOnBottom )
	{		
		origFrame.origin = CGPointMake( 0, screenHeight - origFrame.size.height );
	}
	else
	{
		origFrame.origin = CGPointMake( 0, 0 );
	}
	
	// handle x-axis
	origFrame.origin.x = ( screenWidth / 2 ) - ( _adView.frame.size.width / 2 );
	
	_adView.frame = origFrame;
}


- (MobclixFullScreenAdViewController*)fullScreenAdViewController
{
	// create the view controller if we dont have one
	if( !_fullScreenAdViewController )
	{
		_fullScreenAdViewController = [[MobclixFullScreenAdViewController alloc] init];
		_fullScreenAdViewController.delegate = self;
	}
	
	return _fullScreenAdViewController;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Public

- (void)startWithApplicationId:(NSString*)applicationId
{
	[Mobclix startWithApplicationId:applicationId];
}


- (void)setRefreshTime:(CGFloat)refreshTime
{
	_adView.refreshTime = refreshTime;
}


- (void)createBanner:(MobclixBannerType)bannerType bannerOnBottom:(BOOL)bannerOnBottom
{
	// kill the current adView if we have one
	if( _adView )
		[self hideBanner:YES];
	
	_bannerType = bannerType;
	_adBannerOnBottom = bannerOnBottom;
	
	switch( bannerType )
	{
		case MobclixBannerTypeiPad_120x600:
		{
			_adView = [[MobclixAdViewiPad_120x600 alloc] initWithFrame:CGRectMake( 0, 0, 120, 600 )];
			break;
		}
		case MobclixBannerTypeiPad_300x250:
		{
			_adView = [[MobclixAdViewiPad_300x250 alloc] initWithFrame:CGRectMake( 0, 0, 300, 250 )];
			break;
		}
		case MobclixBannerTypeiPad_468x60:
		{
			_adView = [[MobclixAdViewiPad_468x60 alloc] initWithFrame:CGRectMake( 0, 0, 468, 60 )];
			break;
		}
		case MobclixBannerTypeiPad_728x90:
		{
			_adView = [[MobclixAdViewiPad_728x90 alloc] initWithFrame:CGRectMake( 0, 0, 728, 90 )];
			break;
		}
		case MobclixBannerTypeiPhone_300x250:
		{
			_adView = [[MobclixAdViewiPhone_300x250 alloc] initWithFrame:CGRectMake( 0, 0, 320, 250 )];
			break;
		}
		case MobclixBannerTypeiPhone_320x50:
		{
			_adView = [[MobclixAdViewiPhone_320x50 alloc] initWithFrame:CGRectMake( 0, 0, 320, 50 )];
			break;
		}
	}

	_adView.delegate = self;
	[self adjustAdViewFrameToShowAdView];
	
	if( _adBannerOnBottom )
		_adView.autoresizingMask = ( UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleTopMargin );
	else
		_adView.autoresizingMask = ( UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleBottomMargin );
	
	[UnityGetGLViewController().view addSubview:_adView];
}


- (void)showBanner
{
	_adView.hidden = NO;
	[_adView resumeAdAutoRefresh];
}


- (void)hideBanner:(BOOL)shouldDestroy
{
	_adView.hidden = YES;
	[_adView pauseAdAutoRefresh];
	
	if( shouldDestroy )
	{
		[_adView removeFromSuperview];
		[_adView release];
		_adView = nil;
	}
}


- (void)requestFullScreenAd
{
	[self.fullScreenAdViewController requestAd];
}


- (void)displayFullScreenAd
{
	if( !_fullScreenAdViewController || !_fullScreenAdViewController.hasAd )
	{
		NSLog( @"fullScreenViewController does not have an ad ready" );
		return;
	}
	
	if( ![_fullScreenAdViewController displayRequestedAdFromViewController:UnityGetGLViewController()] )
		NSLog( @"problem with displayRequestedAdFromViewController" );
}


- (void)requestAndDisplayFullScreenAd
{
	[self.fullScreenAdViewController requestAndDisplayAdFromViewController:UnityGetGLViewController()];
}


- (BOOL)isFullScreenAdReady
{
	if( _fullScreenAdViewController )
		return _fullScreenAdViewController.hasAd;
	return NO;
}


@end
