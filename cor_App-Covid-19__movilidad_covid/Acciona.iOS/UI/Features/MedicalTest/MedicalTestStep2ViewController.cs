using System;
using Foundation;
using iOS.UI.Styles;
using UIKit;

namespace Acciona.iOS.UI.Features.MedicalTest
{
    public partial class MedicalTestStep2ViewController : UIViewController
    {
        public event EventHandler<bool?> AskResponded;

        public MedicalTestStep2ViewController() : base("MedicalTestStep2ViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            indicator.Set(4, 2);
            buttonYes.Layer.BorderColor = UIColor.Black.CGColor;
            buttonYes.Layer.BorderWidth = 1.0f;
            buttonYes.Layer.CornerRadius = 4;
            buttonNo.Layer.BorderColor = UIColor.Black.CGColor;
            buttonNo.Layer.BorderWidth = 1.0f;
            buttonNo.Layer.CornerRadius = 4;
            buttonSkip.TitleLabel.AttributedText = Styles.ConvertHTMLStyles(AppDelegate.LanguageBundle.GetLocalizedString("medical_skip"), buttonSkip.TitleLabel.Font.Name, buttonSkip.TitleLabel.Font.PointSize);

            buttonYes.TouchUpInside += ButtonResponse_TouchUpInside;
            buttonNo.TouchUpInside += ButtonResponse_TouchUpInside;
            buttonSkip.TouchUpInside += ButtonResponse_TouchUpInside;
        }

        private void ButtonResponse_TouchUpInside(object sender, EventArgs e)
        {
            if (sender.Equals(buttonYes))
                AskResponded?.Invoke(this, true);
            else if (sender.Equals(buttonNo))
                AskResponded?.Invoke(this, false);
            else if (sender.Equals(buttonSkip))
                AskResponded?.Invoke(this, (bool?)null);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

