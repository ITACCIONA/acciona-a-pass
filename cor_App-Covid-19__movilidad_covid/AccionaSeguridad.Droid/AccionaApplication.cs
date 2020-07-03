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
using AccionaSeguridad.Droid.Navigation;
using AccionaSeguridad.Presentation;
using AccionaSeguridad.Presentation.Navigation;
using ServiceLocator;
using System;
using Android.Content;
using Android.Net;
using ZXing.Mobile;
using AccionaSeguridad.Droid.Services;
using AccionaSeguridad.Droid.Receiver;

namespace AccionaSeguridad.Droid
{
    [Application]
    public class AccionaSeguridadApplication : Application, Application.IActivityLifecycleCallbacks
    {
        private Activity CurrentActivity { get; set; }

        public AccionaSeguridadApplication(IntPtr javaReference, JniHandleOwnership transfer)
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

            AppCenter.Start("UUID-for-security", typeof(Analytics), typeof(Crashes));
            Analytics.SetEnabledAsync(DataConstants.SendAnalytics);
        }

        private void Init()
        {
            Locator.CurrentMutable.Register<Activity>(() => CurrentActivity);
            //Services            
            Locator.CurrentMutable.RegisterLazySingleton<ISettingsService>(() => new SettingsService());
            Locator.CurrentMutable.RegisterLazySingleton<INetworkService>(() => new NetworkService());
            Locator.CurrentMutable.RegisterLazySingleton<ILocaleService>(() => new LocaleService());
            //Navigator
            Locator.CurrentMutable.Register<ISplashNavigator>(() => new SplashNavigator());
            Locator.CurrentMutable.Register<ILoginNavigator>(() => new LoginNavigator());
            Locator.CurrentMutable.Register<IMainNavigator>(() => new MainNavigator());
            Locator.CurrentMutable.Register<IResultNavigator>(() => new ResultNavigator());
            Locator.CurrentMutable.Register<IManualNavigator>(() => new ManualNavigator());
            Locator.CurrentMutable.Register<IManualResultNavigator>(() => new ManualResultNavigator());
            Locator.CurrentMutable.Register<IConfigNavigator>(() => new ConfigNavigator());
            Locator.CurrentMutable.Register<ILanguageNavigator>(() => new LanguageNavigator());
            Locator.CurrentMutable.Register<IWorkingCenterNavigator>(() => new WorkingCenterNavigator());

            MobileBarcodeScanner.Initialize(this);

            RegisterReceiver(new NetworkChangeReceiver(), new IntentFilter(ConnectivityManager.ConnectivityAction));

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