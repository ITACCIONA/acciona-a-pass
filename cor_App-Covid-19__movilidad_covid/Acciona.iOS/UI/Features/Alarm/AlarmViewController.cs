using System;
using Acciona.Presentation.UI.Features.Alarm;
using BaseIOS.UI;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using iOS.UI.Styles;
using UIKit;

namespace Acciona.iOS.UI.Features.Alarm
{
    public partial class AlarmViewController : BaseViewController<AlarmPresenter>, AlarmUI
    {
        public AlarmViewController() : base("AlarmViewController", null)
        {
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override void AssingViews()
        {
            buttonBack.TouchUpInside += (o, e) => presenter.BackClicked();
            EmailButton.Layer.CornerRadius = 8.0f;
            EmailButton.SetTitle(String.Format(AppDelegate.LanguageBundle.GetLocalizedString("alarm_call_to"), AppDelegate.LanguageBundle.GetLocalizedString("alarm_phone")), UIControlState.Normal);
            
            EmailButton.TouchUpInside += (o, e) => presenter.ContactEmailClicked();
            ButtonMail.TouchUpInside += (o, e) => presenter.ContactEmailClicked();
            DownloadButton.TouchUpInside += (o, e) => presenter.OpenSkypeEnterprise();


            configureView();
            roundedViews();
        }

        private void configureView()
        {
            TitleViewLabel.Font = Styles.SetHelveticaBoldFont(18);
            DescriptionLabel.Font = Styles.SetHelveticaBoldFont(18);
            AlarmRequestLabel1.Font = Styles.SetHelveticaFont(15);
            AlarmRequestLabel2.Font = Styles.SetHelveticaFont(15);
            ConfirmLabel.Font = Styles.SetHelveticaFont(15);
            DownloadLabel.Font = Styles.SetHelveticaFont(15);

            NumberConfirmLabel.Font = Styles.SetHelveticaBoldFont(16);
            NumberRequestLabel.Font = Styles.SetHelveticaBoldFont(16);
            NumberDownloadLabel.Font = Styles.SetHelveticaBoldFont(16);

            DownloadButton.SetTitleColor(Colors.primaryRed, UIControlState.Normal);
            DownloadButton.Layer.BorderColor = UIColor.Black.CGColor;
            DownloadButton.Layer.BorderWidth = 1.0f;
            DownloadButton.Layer.CornerRadius = 4;


            ButtonMail.SetTitleColor(Colors.primaryRed, UIControlState.Normal);

            TitleViewLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("alarm_title");
            DescriptionLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("alarm_description");
            AlarmRequestLabel1.AttributedText = Styles.ConvertHTMLStyles(AppDelegate.LanguageBundle.GetLocalizedString("alarm_request_date_text_1"), AlarmRequestLabel1.Font.Name, AlarmRequestLabel1.Font.PointSize);
            AlarmRequestLabel2.Text = AppDelegate.LanguageBundle.GetLocalizedString("alarm_request_date_text_2");
            ConfirmLabel.AttributedText = Styles.ConvertHTMLStyles(AppDelegate.LanguageBundle.GetLocalizedString("alarm_confirm_date"), ConfirmLabel.Font.Name, ConfirmLabel.Font.PointSize);
            DownloadLabel.AttributedText = Styles.ConvertHTMLStyles(AppDelegate.LanguageBundle.GetLocalizedString("alarm_download_skype"), DownloadLabel.Font.Name, DownloadLabel.Font.PointSize);
            DownloadButton.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("alarm_download_skype_link"), UIControlState.Normal);
            EmailButton.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("alarm_request_button"), UIControlState.Normal);
            ButtonMail.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("alarm_email"), UIControlState.Normal);


        }

        private void roundedViews()
        {
            NumberConfirmView.StyleCircle(UIColor.White, UIColor.White, 0);
            NumberRequestView.StyleCircle(UIColor.White, UIColor.White, 0);
            DownloadNumberView.StyleCircle(UIColor.White, UIColor.White, 0);

            NumberConfirmView.setDownBorderShadow();
            NumberRequestView.setDownBorderShadow();
            DownloadNumberView.setDownBorderShadow();
        }





    }

}

