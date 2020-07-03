using System;
using Acciona.Domain.Model.Employee;
using Acciona.Presentation.UI.Features.MedicalInfo;
using BaseIOS.UI;
using Foundation;
using iOS.UI.Styles;
using UIKit;

namespace Acciona.iOS.UI.Features.MedicalInfo
{
    public partial class MedicalInfoViewController : BaseViewController<MedicalInfoPresenter>, MedicalInfoUI
    {
        private Ficha ficha;

        public MedicalInfoViewController(Ficha ficha) : base("MedicalInfoViewController", null)
        {
            this.ficha = ficha;
        }

        public void setResponsesValues(bool?[] responses)
        {
            setResponses(responses);
        }

        public void ShowMedicalInfoStep()
        {
        }

        public void showRiskModification()
        {
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
            presenter.ficha = ficha;
        }

        protected override void AssingViews()
        {
            buttonBack.TouchUpInside += (o, e) => presenter.BackClicked();
            //ContinueButton.TouchUpInside += (o, e) => presenter.ModifyDataClicked();
            ContinueButton.Hidden = true;

            UITapGestureRecognizer gestureRecognizerRisk = new UITapGestureRecognizer(() => presenter.ModifyRiskClicked());
            RiskView.AddGestureRecognizer(gestureRecognizerRisk);

            applyTraslations();
            styleView();
        }

        private void applyTraslations()
        {
            TitleLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("medical_data_title");
            DescriptionLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("medical_data_description");
            RiskLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("medical_step4_risk");
            CovidLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("medical_step4_covid");
            MedicalLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("medical_step4_medical");
            ContinueButton.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("medical_data_button"), UIControlState.Normal);
        }

        private void styleView()
        {
            TitleLabel.Font = Styles.SetHelveticaBoldFont(17);
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

            ContinueButton.Layer.BorderColor = UIColor.Black.CGColor;
            ContinueButton.Layer.BorderWidth = 1f;
            ContinueButton.Layer.CornerRadius = 3f;
        }

        private void setResponses(bool?[] responses)
        {
            RiskLabelState.Text = responses[0] == null
                ? AppDelegate.LanguageBundle.GetLocalizedString("msg_not_known")
                : AppDelegate.LanguageBundle.GetLocalizedString(responses[0] == true ? "msg_yes" : "msg_no");
            CovidLabelState.Text = responses[1] == null
                ? AppDelegate.LanguageBundle.GetLocalizedString("msg_not_known")
                : AppDelegate.LanguageBundle.GetLocalizedString(responses[1] == true ? "msg_yes" : "msg_no");
            MedicalLabelState.Text = responses[2] == null
                ? AppDelegate.LanguageBundle.GetLocalizedString("msg_not_known")
                : AppDelegate.LanguageBundle.GetLocalizedString(responses[2] == true ? "msg_yes" : "msg_no");
        }
    }
}