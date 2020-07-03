using System;
using Foundation;
using iOS.UI.Styles;
using UIKit;

namespace Acciona.iOS.UI.Features.MedicalTest
{
    public partial class MedicalTestStep1ViewController : UIViewController
    {
        public event EventHandler<bool?> AskResponded;

        public MedicalTestStep1ViewController() : base("MedicalTestStep1ViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
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

            applyTraslations();
            styleView();
        }

        private void ButtonResponse_TouchUpInside(object sender, EventArgs e)
        {
            if(sender.Equals(buttonYes))
                AskResponded?.Invoke(this, true);
            else if (sender.Equals(buttonNo))
                AskResponded?.Invoke(this, false);
            else if (sender.Equals(buttonSkip))
                AskResponded?.Invoke(this, (bool?)null);
        }

        public void ShowIndicator(bool show)
        {
            indicator.Hidden = !show;
        }

        private void applyTraslations()
        {
            TextLabel1.Text = AppDelegate.LanguageBundle.GetLocalizedString("medical_text1");
            TextLabel2.Text = AppDelegate.LanguageBundle.GetLocalizedString("medical_text2");
            TextLabel3.Text = AppDelegate.LanguageBundle.GetLocalizedString("medical_text3");
            TextLabel4.Text = AppDelegate.LanguageBundle.GetLocalizedString("medical_text4");
            TextLabel5.Text = AppDelegate.LanguageBundle.GetLocalizedString("medical_text5");
            TextLabel6.Text = AppDelegate.LanguageBundle.GetLocalizedString("medical_text6");
            TextLabel7.Text = AppDelegate.LanguageBundle.GetLocalizedString("medical_text7");
        }

        private void styleView()
        {
            TextLabel1.Font = Styles.SetHelveticaFont(15);
            TextLabel2.Font = Styles.SetHelveticaFont(15);
            TextLabel3.Font = Styles.SetHelveticaFont(15);
            TextLabel4.Font = Styles.SetHelveticaFont(15);
            TextLabel5.Font = Styles.SetHelveticaFont(15);
            TextLabel6.Font = Styles.SetHelveticaFont(15);
            TextLabel7.Font = Styles.SetHelveticaFont(15);

            TextLabel1.TextColor = UIColor.Gray;
            TextLabel2.TextColor = UIColor.Gray;
            TextLabel3.TextColor = UIColor.Gray;
            TextLabel4.TextColor = UIColor.Gray;
            TextLabel5.TextColor = UIColor.Gray;
            TextLabel6.TextColor = UIColor.Gray;
            TextLabel7.TextColor = UIColor.Gray;

            ImageText1.Image = ImageText1.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            ImageText1.TintColor = Colors.primaryRed;
            ImageText2.Image = ImageText1.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            ImageText2.TintColor = Colors.primaryRed;
            ImageText3.Image = ImageText1.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            ImageText3.TintColor = Colors.primaryRed;
            ImageText4.Image = ImageText1.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            ImageText4.TintColor = Colors.primaryRed;
            ImageText5.Image = ImageText1.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            ImageText5.TintColor = Colors.primaryRed;
            ImageText6.Image = ImageText1.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            ImageText6.TintColor = Colors.primaryRed;
            ImageText7.Image = ImageText1.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            ImageText7.TintColor = Colors.primaryRed;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

