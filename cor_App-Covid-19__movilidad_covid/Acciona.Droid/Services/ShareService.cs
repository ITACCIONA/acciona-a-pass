using System;
using Acciona.Domain;
using Acciona.Domain.Services;
using Android.App;
using Android.Content;
using Android.Content.PM;
using ServiceLocator;

namespace Acciona.Droid.Services
{
    public class ShareService : IShareService
    {
            public bool ShareApp(TargetApp targetApp)
            {

                switch (targetApp)
                {
                    case TargetApp.SkypeEnterprise:
                        return openSkypeEnterprise();
                        break;

                    case TargetApp.Email:
                        return openMail();
                        break;

                    default:
                        return false;
                        break;

                }
            }


            public bool openSkypeEnterprise()
            {
                var activity = Locator.CurrentMutable.GetService<Activity>();

                    try
                    {
                        Intent intent = new Intent(Intent.ActionView);

                        intent.SetData(Android.Net.Uri.Parse("market://details?id=com.microsoft.office.lync15&gl=ES"));
                        intent.AddFlags(ActivityFlags.NewTask);

                        Application.Context.StartActivity(intent);

                        return true;
                    }
                    catch (ActivityNotFoundException)
                    {
                        return false;
                    }

                

            }

            private bool openMail()
            {

                var activity = Locator.CurrentMutable.GetService<Activity>();
                Intent mailClient = new Intent(Intent.ActionSendto);
                mailClient.PutExtra(Intent.ExtraSubject, activity.GetString(Resource.String.app_name));
                mailClient.SetType("message/rfc822");
                mailClient.SetData(Android.Net.Uri.Parse("mailto:" + DomainConstants.EMAIL_CONTACT.ToString()));
                activity.StartActivity(Intent.CreateChooser(mailClient, activity.GetString(Resource.String.send_email)));

                return true;

            }
        }
    
}
