using System;
using Acciona.Domain;
using Acciona.Presentation.UI.Features.Splash;
using BaseIOS.UI;
using Foundation;
using UIKit;

namespace Acciona.iOS.UI.Features.Splash
{
    public partial class SplashViewController : BaseViewController<SplashPresenter>,SplashUI
    {
        public SplashViewController() : base("SplashViewController", null)
        {
        }        

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override void AssingViews()
        {
            
        }

        public SplashPresenter.Platform GetPlatform()
        {
            return SplashPresenter.Platform.iOS;
        }

        public int GetVersion()
        {
            return Convert.ToInt32(((NSString)NSBundle.MainBundle.InfoDictionary["CFBundleVersion"]).ToString());
        }

        public void DownloadApp()
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl(DomainConstants.iOSURL));
        }
    }
}

