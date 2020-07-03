using System;
using Foundation;
using iOS.UI.Styles;
using UIKit;

namespace Acciona.iOS.UI.Features.Health
{
    public partial class HealthStep2ViewController : UIViewController
    {
        public event EventHandler<bool> AskResponded;

        public HealthStep2ViewController() : base("HealthStep2ViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            indicator.Set(2, 2);
            buttonYes.Layer.BorderColor = UIColor.Black.CGColor;
            buttonYes.Layer.BorderWidth = 1.0f;
            buttonYes.Layer.CornerRadius = 4;
            buttonNo.Layer.BorderColor = UIColor.Black.CGColor;
            buttonNo.Layer.BorderWidth = 1.0f;
            buttonNo.Layer.CornerRadius = 4;

            buttonYes.TouchUpInside += ButtonResponse_TouchUpInside;
            buttonNo.TouchUpInside += ButtonResponse_TouchUpInside;

            labelTitle.Text = AppDelegate.LanguageBundle.GetLocalizedString("health_step2_title");
            labelTitle.Font = Styles.SetHelveticaBoldFont(18);

            labelSubtitle.AttributedText = Styles.ConvertHTMLStyles(AppDelegate.LanguageBundle.GetLocalizedString("health_step2_subtitle"), labelSubtitle.Font.FamilyName, labelSubtitle.Font.PointSize);
            labelHelp1.AttributedText = Styles.ConvertHTMLStyles(AppDelegate.LanguageBundle.GetLocalizedString("health_step2_help1"), labelHelp1.Font.FamilyName, labelHelp1.Font.PointSize);
            labelHelp2.AttributedText = Styles.ConvertHTMLStyles(AppDelegate.LanguageBundle.GetLocalizedString("health_step2_help2"), labelHelp2.Font.FamilyName, labelHelp2.Font.PointSize);
        }

        private void ButtonResponse_TouchUpInside(object sender, EventArgs e)
        {
            if(sender.Equals(buttonYes))
                AskResponded?.Invoke(this, true);
            else if (sender.Equals(buttonNo))
                AskResponded?.Invoke(this, false);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

