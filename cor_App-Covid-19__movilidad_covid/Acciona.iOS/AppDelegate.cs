using Acciona.Data;
using Acciona.Domain;
using Acciona.Domain.Services;
using Acciona.iOS.Navigation;
using Acciona.iOS.Services;
using Acciona.iOS.UI.Features.Splash;
using Acciona.Presentation;
using Acciona.Presentation.Navigation;
using BaseIOS.Services;
using Domain.Services;
using Foundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using ServiceLocator;
using UIKit;

namespace Acciona.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register ("AppDelegate")]
    public class AppDelegate : UIResponder, IUIApplicationDelegate
    {       
        [Export("window")]
        public UIWindow Window { get; set; }
        public static NSBundle LanguageBundle { get; set; }
        private UINavigationController navController;

        [Export ("application:didFinishLaunchingWithOptions:")]
        public bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
        {            
            InitConfig();
            PresenterConfiguration.Init();
            DataConfiguration.Init();
            DomainConfiguration.Init();         
            AppCenter.Start("UUID-for-ios", typeof(Analytics), typeof(Crashes));
            Analytics.SetEnabledAsync(DataConstants.SendAnalytics);
            return true;
        }

        private void InitConfig()
        {
            //Services
            Locator.CurrentMutable.RegisterLazySingleton<ISettingsService>(() => new SettingsService());
            Locator.CurrentMutable.RegisterLazySingleton<IShareService>(() => new ShareService());
            Locator.CurrentMutable.RegisterLazySingleton<ILocaleService>(() => new LocaleService());
            Locator.CurrentMutable.Register<ILogoutService>(() => new LogoutService());
            Locator.CurrentMutable.Register<NSBundle>(() => LanguageBundle != null ? LanguageBundle : NSBundle.MainBundle);
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

            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {

            }
            else
            {
                Window = new UIWindow(UIScreen.MainScreen.Bounds);
                //Window.WindowScene = (UIWindowScene)scene;
                navController = new UINavigationController(new SplashViewController());
                navController.NavigationBarHidden = true;

                Window.RootViewController = navController;
                Window.MakeKeyAndVisible();
            }
        }

        // UISceneSession Lifecycle

        [Export ("application:configurationForConnectingSceneSession:options:")]
        public UISceneConfiguration GetConfiguration (UIApplication application, UISceneSession connectingSceneSession, UISceneConnectionOptions options)
        {
            // Called when a new scene session is being created.
            // Use this method to select a configuration to create the new scene with.
            return UISceneConfiguration.Create ("Default Configuration", connectingSceneSession.Role);
        }

        [Export ("application:didDiscardSceneSessions:")]
        public void DidDiscardSceneSessions (UIApplication application, NSSet<UISceneSession> sceneSessions)
        {
            // Called when the user discards a scene session.
            // If any sessions were discarded while the application was not running, this will be called shortly after `FinishedLaunching`.
            // Use this method to release any resources that were specific to the discarded scenes, as they will not return.
        }
    }
}

