using System;
using System.Linq;
using System.Threading.Tasks;
using Acciona.iOS.UI.Features.Web;
using Acciona.iOS.Utils;
using Acciona.Presentation.UI.Features.Login;
using BaseIOS.UI;
using Foundation;
using iOS.UI.Styles;
using UIKit;

namespace Acciona.iOS.UI.Features.Login
{
    public partial class LoginViewController : BaseViewController<LoginPresenter>,LoginUI
    {
        private UIViewController oAuthController;

        public LoginViewController() : base("LoginViewController", null)
        {
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override void AssingViews()
        {
            LoginStateButton.TouchUpInside += (o, e) => GetResult();
            LoginStateButton.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("msg_retry"), UIControlState.Normal);
            
            showLoadingLogin();
            styleViews();

            GetResult();
        }

        private void styleViews()
        {
            LoginStateLabel.Font = Styles.SetHelveticaFont(16);
            LoginStateButton.Layer.CornerRadius = 4;
        }

        public bool CheckInternet()
        {
            return true;
        }

        public void SetLastUser(string oldUser)
        {
         
        }

        private async Task GetResult()
        {
            try
            {

                var rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController as UINavigationController;
                var controller = new WebViewController(null);
                controller.ErrorEvent += Controller_ErrorEvent;
                controller.OkEvent += Controller_OkEvent;
                controller.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
                rootViewController.PresentViewController(controller, true, null);
            }
            catch (Exception e)
            {
                ShowDialog(e.Message, "msg_ok", null);
            }
        }

        private void Controller_OkEvent(object sender, System.Collections.Generic.IDictionary<string, string> dictionary)
        {
            showLoadingLogin();
            if (dictionary.ContainsKey("token_type") && dictionary.ContainsKey("access_token"))
                presenter.Authorized(dictionary["token_type"] + " " + dictionary["access_token"]);
            else
                ShowErrorLogin();
        }

        private void Controller_ErrorEvent(object sender, EventArgs e)
        {
            ShowErrorLogin();
        }


        public void showLoadingLogin()
        {
            LoginStateLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("login_loading");
            LoginStateImgView.Image = UIImage.FromBundle("splash_waiting");
            LoginStateButton.Hidden = true;
        }

        public void ShowErrorLogin()
        {
            LoginStateLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("login_error");
            LoginStateImgView.Image = UIImage.FromBundle("splash_error");
            LoginStateButton.Hidden = false;
        }

        public void ShowRetry()
        {
            ShowErrorLogin();
        }
    }
}

