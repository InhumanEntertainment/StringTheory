#import <UIKit/UIKit.h>
#import <Foundation/Foundation.h>
#import <MessageUI/MessageUI.h>
#import <MessageUI/MFMailComposeViewController.h>

@interface InhumanIOS : NSObject <MFMailComposeViewControllerDelegate>
{
    UIViewController * currentViewController;  
}

- (void)Popup: (const char *) text;
- (void)ComposeEmail: (UIViewController *)viewController :(const char *)to: (const char *)subject: (const char *)body;

@end

