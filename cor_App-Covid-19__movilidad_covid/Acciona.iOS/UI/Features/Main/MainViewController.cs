using System;
using Acciona.Presentation.UI.Features.Main;
using BaseIOS.UI;
using UIKit;
using Acciona.iOS.Utils;
using CoreGraphics;
using CoreAnimation;
using static Acciona.Presentation.UI.Features.Main.MainPresenter;
using Acciona.iOS.UI.Features.Passport;
using Acciona.iOS.UI.Features.Profile;
using Acciona.iOS.UI.Features.QRcode;
using Foundation;
using Acciona.Presentation.UI.Features.Splash;
using Acciona.Domain;

namespace Acciona.iOS.UI.Features.Main
{
    public partial class MainViewController : BaseViewController<MainPresenter>,MainUI
    {
        private MainState state;
        private UIViewController actualController;
        

        public MainViewController() : base("MainViewController", null)
        {
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override void AssingViews()
        {
            buttonPhone.TouchUpInside += (o, e) => presenter.PhoneClicked();
            buttonBell.TouchUpInside += (o, e) => presenter.BellClicked();
            buttonPassport.TouchUpInside += MainButton_TouchUpInside;
            buttonPanic.TouchUpInside += MainButton_TouchUpInside;
            buttonProfile.TouchUpInside += MainButton_TouchUpInside;

            buttonPanic.TitleLabel.Lines = 0;
            buttonPanic.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
            buttonPanic.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("main_alarm"),UIControlState.Normal);
            buttonProfile.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("main_profile"),UIControlState.Normal);
            buttonPassport.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("main_passport"),UIControlState.Normal);
            buttonPassport.SetImage(buttonPassport.ImageView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);
            buttonProfile.SetImage(buttonProfile.ImageView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            UIBezierPath mPath = UIBezierPath.FromRoundedRect(buttonPanic.Layer.Bounds, (UIRectCorner.TopLeft | UIRectCorner.TopRight), new CGSize(width: 8, height: 8));
            CAShapeLayer maskLayer = new CAShapeLayer();
            maskLayer.Path = mPath.CGPath;
            buttonPanic.Layer.Mask = maskLayer;                                   
        }
        

        private void MainButton_TouchUpInside(object sender, EventArgs e)
        {
            if (sender.Equals(buttonPassport))
            {
                presenter.PassportClicked();
            }
            else if (sender.Equals(buttonPanic))
            {
                presenter.AlarmClicked();
            }
            else if (sender.Equals(buttonProfile))
            {
                presenter.ProfileClicked();
            }
        }

        public void ShowState(MainState state)
        {
            this.state = state;
            if (actualController != null)
            {
                actualController.View.RemoveFromSuperview();
            }
            switch (state)
            {
                case MainState.PASSPORT:
                    actualController = new QRcodeViewController();
                    break;
                case MainState.PROFILE:
                    actualController = new ProfileViewController();
                    break;
                
            }
            actualController.View.Frame = new CGRect(0, 0, contentView.Frame.Width, contentView.Frame.Height);
            contentView.AddSubview(actualController.View);            
        }

        public void ShowNotificationsNotRead(bool notRead)
        {
            if (notRead)
            {
                buttonBell.SetImage(UIImage.FromBundle("bell_advice"), UIControlState.Normal);
            }
            else
            {
                buttonBell.SetImage(UIImage.FromBundle("bell"), UIControlState.Normal);
            }
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

