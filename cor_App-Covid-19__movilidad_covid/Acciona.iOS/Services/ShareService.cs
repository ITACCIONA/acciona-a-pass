using System;
using Acciona.Domain;
using Acciona.Domain.Services;
using Foundation;
using MessageUI;
using UIKit;

namespace Acciona.iOS.Services
{
    public class ShareService : IShareService
    {
        public bool ShareApp(TargetApp targetApp)
        {
            switch (targetApp)
            {
                case TargetApp.SkypeEnterprise:
                    return OpenSkypeEnterprise();
                case TargetApp.Email:
                    return SendEmail();
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetApp), targetApp, null);
            }
        }

        private bool SendEmail()
        {
            if (MFMailComposeViewController.CanSendMail)
            {
                var composeViewController = new MFMailComposeViewController
                {
                    MailComposeDelegate = new MailComposerDelegate()
                };
                composeViewController.SetToRecipients(new string[] { DomainConstants.EMAIL_CONTACT });
                composeViewController.SetSubject("Acciona Covid-19");
                composeViewController.SetMessageBody("", false);

                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(composeViewController, true, null);

                return true;
            }

            return false;
        }

        private class MailComposerDelegate : MFMailComposeViewControllerDelegate
        {
            [Export("mailComposeController:didFinishWithResult:error:")]
            public override void Finished(MFMailComposeViewController controller, MFMailComposeResult result, NSError error)
            {
                controller.DismissViewController(true, null);
            }
        }


        private bool OpenSkypeEnterprise()
        {
            UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("itms://itunes.apple.com/es/app/skype-empresarial/id605841731#"), new NSDictionary(), null);
            return true;


        }
    }
}
