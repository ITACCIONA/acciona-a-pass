using System;

using UIKit;

namespace Acciona.iOS.UI.Features.QRcode
{
    public partial class ChangeStateViewController : UIViewController
    {
        public event EventHandler ShowToDo;

        public ChangeStateViewController() : base("ChangeStateViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            buttonShow.Layer.CornerRadius = 4;
            content.Layer.CornerRadius = 4;

            buttonClose.TouchUpInside += ButtonClose_TouchUpInside;
            buttonShow.TouchUpInside += ButtonShow_TouchUpInside;

            ApplyTranslates();
        }

        private void ApplyTranslates()
        {
            labelTitle.Text = AppDelegate.LanguageBundle.GetLocalizedString("change_status_title");
            labelSubtitle.Text = AppDelegate.LanguageBundle.GetLocalizedString("change_status_subtitle");
            buttonShow.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("change_status_button"),UIControlState.Normal);
        }

        private void ButtonShow_TouchUpInside(object sender, EventArgs e)
        {
            this.DismissViewController(false, ()=>ShowToDo?.Invoke(this,null));
        }

        private void ButtonClose_TouchUpInside(object sender, EventArgs e)
        {
            this.DismissViewController(false, null);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

