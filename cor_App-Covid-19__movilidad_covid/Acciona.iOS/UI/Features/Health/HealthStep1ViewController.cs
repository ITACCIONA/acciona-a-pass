using System;
using Foundation;
using iOS.UI.Styles;
using UIKit;

namespace Acciona.iOS.UI.Features.Health
{
    public partial class HealthStep1ViewController : UIViewController
    {
        public event EventHandler<bool[]> NoSymptomsClicked;

        private bool fiebre;
        private bool sintomas;

        public HealthStep1ViewController() : base("HealthStep1ViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            indicator.Set(2, 1);
            buttonConfirm.Layer.CornerRadius = 4;
            buttonConfirm.TouchUpInside += (o, e) => NoSymptomsClicked?.Invoke(this, new bool[] { fiebre, sintomas });

            UITapGestureRecognizer gestureRecognizerSympton1 = new UITapGestureRecognizer(() => selectSympton(SymptonView1));
            SymptonView1.AddGestureRecognizer(gestureRecognizerSympton1);
            UITapGestureRecognizer gestureRecognizerSympton2 = new UITapGestureRecognizer(() => selectSympton(SymptonView2));
            SymptonView2.AddGestureRecognizer(gestureRecognizerSympton2);

            applyTraslations();
            styleView();
        }


        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void applyTraslations()
        {
            labelTitle.Text = AppDelegate.LanguageBundle.GetLocalizedString("health_step1_title");
            DescriptionLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("health_description");
            buttonConfirm.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("health_step1_no_sintoms"), UIControlState.Normal);

            SymptonLabel1.Text = AppDelegate.LanguageBundle.GetLocalizedString("health_step1_sintoms1");
            SymptonLabel2.Text = AppDelegate.LanguageBundle.GetLocalizedString("health_step1_sintoms2");
            labelSymptomsHelp.Text = AppDelegate.LanguageBundle.GetLocalizedString("health_step1_sintomp2_help");
        }

        private void styleView()
        {
            labelTitle.Font = Styles.SetHelveticaBoldFont(19);
            DescriptionLabel.Font = Styles.SetHelveticaFont(19);
            setButtonRed(false);

            SymptonLabel1.Font = Styles.SetHelveticaBoldFont(17);
            SymptonLabel2.Font = Styles.SetHelveticaBoldFont(17);

            SymptonView1.setBorderShadow();
            SymptonView2.setBorderShadow();

            selectSympton(SymptonView1, false);
            selectSympton(SymptonView2, false);
        }

        private void selectSympton(UIView sView, bool selected)
        {
            if (selected)
            {
                sView.Layer.BorderColor = UIColor.LightGray.CGColor;
                sView.BackgroundColor = UIColor.LightGray;
            }
            else
            {
                sView.Layer.BorderColor = UIColor.Gray.CGColor;
                sView.BackgroundColor = UIColor.White;
            }
        }

        private void selectSympton(object sender)
        {
            if (sender == SymptonView1)
            {
                if (fiebre)
                {
                    fiebre = false;
                    SymptonView1.BackgroundColor = UIColor.White;
                }
                else
                {
                    fiebre = true;
                    SymptonView1.BackgroundColor = UIColor.LightGray;
                }
            }
            else if (sender == SymptonView2)
            {
                if (sintomas)
                {
                    sintomas = false;
                    SymptonView2.BackgroundColor = UIColor.White;
                }
                else
                {
                    sintomas = true;
                    SymptonView2.BackgroundColor = UIColor.LightGray;
                }
            }
            setButtonRed(fiebre || sintomas);
        }

        private void setButtonRed(bool active)
        {
            if (active)
            {
                buttonConfirm.Layer.BorderWidth = 0.0f;
                buttonConfirm.BackgroundColor = Colors.primaryRed;
                buttonConfirm.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("health_step1_sintoms_communicate"), UIControlState.Normal);
                buttonConfirm.SetTitleColor(UIColor.White, UIControlState.Normal);
            }
            else
            {
                buttonConfirm.Layer.BorderColor = UIColor.Black.CGColor;
                buttonConfirm.Layer.BorderWidth = 1.0f;
                buttonConfirm.BackgroundColor = UIColor.White;
                buttonConfirm.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("health_step1_no_sintoms"), UIControlState.Normal);
                buttonConfirm.SetTitleColor(UIColor.Black, UIControlState.Normal);
            }
        }
    }
}

