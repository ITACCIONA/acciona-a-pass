using Acciona.Domain.Services;
using Android.App;
using Android.OS;
using Android.Runtime;
using Domain.Services;
using Droid.Services;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Acciona.Data;
using Acciona.Domain;
using Acciona.Droid.Navigation;
using Acciona.Presentation;
using Acciona.Presentation.Navigation;
using ServiceLocator;
using System;
using Acciona.Droid.Services;
using Android.Content;
using Android.Net;

namespace Acciona.Droid
{
    [Application]
    public class AccionaApplication : Application, Application.IActivityLifecycleCallbacks
    {
        private Activity CurrentActivity { get; set; }

        public AccionaApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();

            Init();
            PresenterConfiguration.Init();
            DataConfiguration.Init();
            DomainConfiguration.Init();

            RegisterActivityLifecycleCallbacks(this);

            AppCenter.Start("UUID-for-droid", typeof(Analytics), typeof(Crashes));
            Analytics.SetEnabledAsync(DataConstants.SendAnalytics);
        }

        private void Init()
        {
            Locator.CurrentMutable.Register<Activity>(() => CurrentActivity);
            //Services            
            Locator.CurrentMutable.RegisterLazySingleton<ISettingsService>(() => new SettingsService());
            Locator.CurrentMutable.RegisterLazySingleton<IShareService>(() => new ShareService());
            Locator.CurrentMutable.Register<ILocaleService>(() => new LocaleService());
            Locator.CurrentMutable.Register<ILogoutService>(() => new LogoutService());
            //Navigator
            Locator.CurrentMutable.Register<ISplashNavigator>(() => new SplashNavigator());
            Locator.CurrentMutable.Register<ILoginNavigator>(() => new LoginNavigator());
            Locator.CurrentMutable.Register<IMainNavigator>(() => new MainNavigator());
            Locator.CurrentMutable.Register<IPassportNavigator>(() => new PassportNavigator());
            Locator.CurrentMutable.Register<IProfileNavigator>(() => new ProfileNavigator());
            Locator.CurrentMutable.Register<IQRcodeNavigator>(() => new QRcodeNavigator());
            Locator.CurrentMutable.Register<IAlarmNavigator>(() => new AlarmNavigator());
            Locator.CurrentMutable.Register<IAlertsNavigator>(() => new AlertsNavigator());
            Locator.CurrentMutable.Register<IAlertDetailNavigator>(() => new AlertDetailNavigator());
            Locator.CurrentMutable.Register<IHealthNavigator>(() => new HealthNavigator());
            Locator.CurrentMutable.Register<IContactDataNavigator>(() => new ContactDataNavigator());
            Locator.CurrentMutable.Register<IRegisteredNavigator>(() => new RegisteredNavigator());
            Locator.CurrentMutable.Register<IMedicalInfoNavigator>(() => new MedicalInfoNavigator());
            Locator.CurrentMutable.Register<IMedicalTestNavigator>(() => new MedicalTestNavigator());
            Locator.CurrentMutable.Register<IMedicalInfoEditNavigator>(() => new MedicalInfoEditNavigator());
            Locator.CurrentMutable.Register<IWebNavigator>(() => new WebNavigator());
            Locator.CurrentMutable.Register<IOfflineNavigator>(() => new OfflineNavigator());
            Locator.CurrentMutable.Register<ILanguageNavigator>(() => new LanguageNavigator());
            Locator.CurrentMutable.Register<IWorkingCenterNavigator>(() => new WorkingCenterNavigator());
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CurrentActivity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {

        }

        public void OnActivityPaused(Activity activity)
        {

        }

        public void OnActivityResumed(Activity activity)
        {
            CurrentActivity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {

        }

        public void OnActivityStarted(Activity activity)
        {
            CurrentActivity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {

        }
    }
}