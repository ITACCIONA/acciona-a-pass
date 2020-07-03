using System;
using Acciona.Domain.Services;
using Acciona.Droid.UI.Features.Login;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using ServiceLocator;

namespace Acciona.Droid.Services
{
    public class LogoutService : ILogoutService
    {
        public void LogoutExpired()
        {
            var activity = Locator.Current.GetService<Activity>();
            Intent intent = new Intent(activity, typeof(LoginActivity));
            ActivityCompat.FinishAffinity(activity);
            activity.StartActivity(intent);            
        }
    }
}
