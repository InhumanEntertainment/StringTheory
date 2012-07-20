//
//  MobClixBinding.m
//  MobClixTest
//
//  Created by Mike on 8/22/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import "MobclixManager.h"


// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]


void _mobclixStart( const char * applicactionId )
{
	[[MobclixManager sharedManager] startWithApplicationId:GetStringParam( applicactionId )];
}


void _mobclixSetRefreshTime( float refreshTime )
{
	[[MobclixManager sharedManager] setRefreshTime:refreshTime];
}


void _mobclixCreateBanner( int bannerType, bool bannerOnBottom )
{
	MobclixBannerType type = bannerType;
	
	[[MobclixManager sharedManager] createBanner:type bannerOnBottom:bannerOnBottom];
	[[MobclixManager sharedManager] showBanner];
}


void _mobclixShowBanner()
{
	[[MobclixManager sharedManager] showBanner];
}


void _mobclixHideBanner( bool shouldDestroy )
{
	[[MobclixManager sharedManager] hideBanner:shouldDestroy];
}


void _mobclixRequestFullScreenAd()
{
	[[MobclixManager sharedManager] requestFullScreenAd];
}


void _mobclixDisplayFullScreenAd()
{
	[[MobclixManager sharedManager] displayFullScreenAd];
}


void _mobclixRequestAndDisplayFullScreenAd()
{
	[[MobclixManager sharedManager] requestAndDisplayFullScreenAd];
}


bool _mobclixIsFullScreenAdReady()
{
	return [[MobclixManager sharedManager] isFullScreenAdReady];
}

