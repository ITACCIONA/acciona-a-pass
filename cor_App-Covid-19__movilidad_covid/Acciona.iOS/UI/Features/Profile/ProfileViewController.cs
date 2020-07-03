using System;
using Acciona.Domain.Model.Employee;
using Acciona.Presentation.UI.Features.Profile;
using BaseIOS.UI;
using Foundation;
using iOS.UI.Styles;
using UIKit;

namespace Acciona.iOS.UI.Features.Profile
{
    public partial class ProfileViewController : BaseViewController<ProfilePresenter>,ProfileUI
    {
        private Ficha ficha;

        public ProfileViewController() : base("ProfileViewController", null)
        {

        }        

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override void AssingViews()
        {
            styleView();
            applyTraslations();

            UITapGestureRecognizer gestureRecognizerContact = new UITapGestureRecognizer(() => presenter.OpenContactData());
            UITapGestureRecognizer gestureRecognizerHealth = new UITapGestureRecognizer(() => presenter.OpenMedicalInfo());
            UITapGestureRecognizer gestureRecognizerCenter = new UITapGestureRecognizer(() => presenter.OpenCenter());
            HealthDataView.AddGestureRecognizer(gestureRecognizerHealth);
            ContactDataView.AddGestureRecognizer(gestureRecognizerContact);
            CenterView.AddGestureRecognizer(gestureRecognizerCenter);

            CloseSessionButton.TouchUpInside += (sender, e) => presenter.LogoutClicked();
            LanguageButton.TouchUpInside += (sender, e) => presenter.LanguageClicked();


            HelloLabel.Text = "";
            CloseSessionLabel.Text = "";

            CloseSessionButton.Layer.CornerRadius = 8.0f;

            labeVersion.Text = String.Format(AppDelegate.LanguageBundle.GetLocalizedString("version"), NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"]);
        }

        public void SetFicha(Ficha ficha)
        {
            this.ficha = ficha;
            configureView();
        }

        private void styleView()
        {
            HelloLabel.Font = Styles.SetHelveticaFont(25);
            ContactDataTitleLabel.Font = Styles.SetHelveticaBoldFont(15);
            ContactDataDescripLabel.Font = Styles.SetHelveticaFont(15);
            HealthDataTitleLabel.Font = Styles.SetHelveticaBoldFont(15);
            HealthDataDescripLabel.Font = Styles.SetHelveticaFont(15);
            CloseSessionLabel.Font = Styles.SetHelveticaFont(15);
            CenterTitle.Font = Styles.SetHelveticaBoldFont(15);
            CenterDesc.Font = Styles.SetHelveticaFont(15);

            ContactDataDescripLabel.TextColor = UIColor.LightGray;
            HealthDataDescripLabel.TextColor = UIColor.LightGray;
            CenterDesc.TextColor = UIColor.LightGray;

            HealthDataView.setBorderShadow();
            ContactDataView.setBorderShadow();
            CenterView.setBorderShadow();
        }

        private void applyTraslations()
        {
            ContactDataTitleLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("profile_contact_data");
            ContactDataDescripLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("profile_contact_data_descrip");
            HealthDataTitleLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("profile_health_data");
            HealthDataDescripLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("profile_health_data_descrip");
            CenterTitle.Text = AppDelegate.LanguageBundle.GetLocalizedString("profile_center_data_title");
            CenterDesc.Text = "-";

            CloseSessionButton.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("profile_close_session"), UIControlState.Normal);
            LanguageButton.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("language_title"), UIControlState.Normal);
        }

        private void configureView()
        {
            var hellotitle = String.Format(AppDelegate.LanguageBundle.GetLocalizedString("profile_hello"), ficha.NombreEmpleado);
            HelloLabel.AttributedText = Styles.ConvertHTMLStyles(hellotitle, HelloLabel.Font.Name, HelloLabel.Font.PointSize);
            CloseSessionLabel.Text = String.Format(AppDelegate.LanguageBundle.GetLocalizedString("profile_close_name"), ficha.NombreEmpleado);
            if (ficha.IdLocalizacion.HasValue)
                CenterDesc.Text = ficha.Localizacion;
            else
                CenterDesc.Text = AppDelegate.LanguageBundle.GetLocalizedString("center_other");
        }

        
    }
}

