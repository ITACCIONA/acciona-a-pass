using System;
using Foundation;
using iOS.UI.Styles;
using UIKit;

namespace Acciona.iOS.UI.Features.MedicalTest
{
    public partial class MedicalTestStep4ViewController : UIViewController
    {
        public event EventHandler ConfirmedClicked;
        private bool?[] responses;

        public MedicalTestStep4ViewController(bool?[] responses) : base("MedicalTestStep4ViewController", null)
        {
            this.responses = responses;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            indicator.Set(4, 4);
            buttonConfirm.Layer.CornerRadius = 4;
            buttonConfirm.TouchUpInside += (o, e) => ConfirmedClicked?.Invoke(this, null);

            applyTraslations();
            styleView();
            setResponses();
        }


        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void applyTraslations()
        {
            labelTitle.Text = AppDelegate.LanguageBundle.GetLocalizedString("medical_step4_title");
            DescriptionLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("medical_step4_description");
            RiskLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("medical_step4_risk");
            CovidLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("medical_step4_covid");
            MedicalLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("medical_step4_medical");
            buttonConfirm.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("medical_step4_button_confirm"), UIControlState.Normal);
        }

        private void styleView()
        {
            labelTitle.Font = Styles.SetHelveticaBoldFont(20);
            DescriptionLabel.Font = Styles.SetHelveticaFont(17);
            RiskLabel.Font = Styles.SetHelveticaFont(15);
            MedicalLabel.Font = Styles.SetHelveticaFont(15);
            CovidLabel.Font = Styles.SetHelveticaFont(15);
            RiskLabelState.Font = Styles.SetHelveticaBoldFont(15);
            CovidLabelState.Font = Styles.SetHelveticaBoldFont(15);
            MedicalLabelState.Font = Styles.SetHelveticaBoldFont(15);

            RiskLabel.TextColor = UIColor.Gray;
            MedicalLabel.TextColor = UIColor.Gray;
            CovidLabel.TextColor = UIColor.Gray;
        }

        private void setResponses()
        {
            RiskLabelState.Text = responses[0] == null ? AppDelegate.LanguageBundle.GetLocalizedString("msg_not_known") : AppDelegate.LanguageBundle.GetLocalizedString(responses[0] == true ? "msg_yes" : "msg_no");
            CovidLabelState.Text = responses[1] == null ? AppDelegate.LanguageBundle.GetLocalizedString("msg_not_known") : AppDelegate.LanguageBundle.GetLocalizedString(responses[1] == true ? "msg_yes" : "msg_no");
            MedicalLabelState.Text = responses[2] == null ? AppDelegate.LanguageBundle.GetLocalizedString("msg_not_known") : AppDelegate.LanguageBundle.GetLocalizedString(responses[2] == true ? "msg_yes" : "msg_no");
        }
    }
}

