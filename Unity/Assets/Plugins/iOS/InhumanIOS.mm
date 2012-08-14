#import "InhumanIOS.h"

@implementation InhumanIOS

//============================================================================================================================================//
- (id)init
{
    self = [super init];
    currentViewController = nil;
    
    return self;
}

//============================================================================================================================================//
- (void)dealloc{}

//============================================================================================================================================//
- (void)Popup: (const char *) title: (const char *) message: (const char *) buttonTitle
{
    UIAlertView *alert = [[UIAlertView alloc] initWithTitle:CreateNSString(title) 
                                                    message:CreateNSString(message)
                                                   delegate:nil 
                                          cancelButtonTitle:CreateNSString(buttonTitle)
                                          otherButtonTitles:nil];
    [alert show];
}

//============================================================================================================================================//
-(void)ComposeEmail: (UIViewController *) viewController :(const char *)to: (const char *)subject: (const char *)body;
{
    currentViewController = viewController;
    
    MFMailComposeViewController * picker = [[MFMailComposeViewController alloc] init];
    picker.mailComposeDelegate = self;
    
    if ([MFMailComposeViewController canSendMail])
    {
	    NSArray * toRecipients = [NSArray arrayWithObjects:CreateNSString(to), nil]; 
        
	    [picker setToRecipients:toRecipients];
        [picker setSubject:CreateNSString(subject)];
	    [picker setMessageBody:CreateNSString(body) isHTML:NO];
	    
	    [currentViewController presentModalViewController:picker animated:YES];        
    }
}

//============================================================================================================================================//
- (void)mailComposeController:(MFMailComposeViewController *)controller didFinishWithResult:(MFMailComposeResult)result error:(NSError *)error
{
    [controller dismissModalViewControllerAnimated:YES];
}

//============================================================================================================================================//
NSString* CreateNSString (const char* string)
{
    if (string)
        return [NSString stringWithUTF8String: string];
    else
        return [NSString stringWithUTF8String: ""];
}

//============================================================================================================================================//
char* MakeStringCopy (const char* string)
{
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

@end


//============================================================================================================================================//
//============================================================================================================================================//
//============================================================================================================================================//
#ifndef TEST
// Return Unitys ViewController //
UIViewController * UnityGetGLViewController();

extern "C" 
{
	//============================================================================================================================================//
	void _Popup(const char * title, const char * message, const char * buttonTitle)
	{
        InhumanIOS * inhuman = [InhumanIOS alloc];
        [inhuman Popup: title: message: buttonTitle];
	}
    
    //============================================================================================================================================//
	void _ComposeEmail(const char * to, const char * subject, const char * body)
	{
        InhumanIOS * inhuman = [InhumanIOS alloc];
        [inhuman ComposeEmail: UnityGetGLViewController(): to: subject: body];
	}
	
	//============================================================================================================================================//
	bool _IsMusicPlaying() 
	{
        BOOL isPlaying = NO;
        MPMusicPlayerController * iPodMusicPlayer = [MPMusicPlayerController iPodMusicPlayer];
		
        if (iPodMusicPlayer.playbackState == MPMusicPlaybackStatePlaying) 
		{
            isPlaying = YES;
        }
        NSLog(@"Music is %@.", isPlaying ? @"on" : @"off");
		
        return isPlaying;
    }   

}
#endif



