#import "InhumanIOS.h"

// Return Unity ViewController //
UIViewController * UnityGetGLViewController();

@implementation InhumanIOS
	
	//============================================================================================================================================//
	- (id)init
	{
	    self = [super init];
	    currentViewController = nil;
	     
	    return self;
	}
	
	//============================================================================================================================================//
	- (void)dealloc
	{	
	}
	
	//============================================================================================================================================//
	- (void)Popup: (const char *) text
	{
	    UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Inhuman" 
	                                                    message: CreateNSString(text)
	                                                   delegate:nil 
	                                          cancelButtonTitle:@"OK"
	                                          otherButtonTitles:nil];
	    [alert show];
	}
	
	//============================================================================================================================================//
	-(void)ComposeEmail: (UIViewController *) viewController :(const char *)to: (const char *)subject: (const char *)body;
	{
	    currentViewController = viewController;
	    
	    MFMailComposeViewController * picker = [[MFMailComposeViewController alloc] init];
	    picker.mailComposeDelegate = self;
	    
	    //[picker setSubject:CreateNSString(subject)];
	    if ([MFMailComposeViewController canSendMail])
	    {
	    
	    
	    // Set up the recipients.
	    NSArray * toRecipients = [NSArray arrayWithObjects:CreateNSString(to), nil];    
	    [picker setToRecipients:toRecipients];
	
	    // Fill out the email body text.
	    NSString * emailBody = CreateNSString(body);
	    [picker setMessageBody:emailBody isHTML:NO];
	    
	
	    // Present the mail composition interface.
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
extern "C" 
{
	//============================================================================================================================================//
	void _Popup (const char * text)
	{
        InhumanIOS * inhuman = [InhumanIOS alloc];
        [inhuman Popup:text];
	}
    
    //============================================================================================================================================//
	void _ComposeEmail (const char * to, const char * subject, const char * body)
	{
        InhumanIOS * inhuman = [InhumanIOS alloc];
        [inhuman ComposeEmail: UnityGetGLViewController(): to: subject: body];
	}
}



