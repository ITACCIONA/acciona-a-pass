using System;
using Acciona.Presentation.UI.Features.Offline;
using BaseIOS.UI;
using Foundation;
using iOS.UI.Styles;
using UIKit;

namespace Acciona.iOS.UI.Features.Offline
{
    public partial class OfflineViewController : BaseViewController<OfflinePresenter>, OfflineUI
    {

        public OfflineViewController() : base("OfflineViewController", null)
        {
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override void AssingViews()
        {

            buttonContinue.TouchUpInside += (o, e) => presenter.ContinueClick();
            buttonContinue.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("offline_continue"), UIControlState.Normal);
            styleView();
            TimeoutLabel.Text = "";
            AccessLabel.Text = "";
            textQRHelp.Text = "";
            viewState.BackgroundColor = UIColor.White;
            NameLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("offline_name");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public void SetQRInfo(string info)
        {
            var barcodeWriter = new ZXing.Mobile.BarcodeWriter
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 1000,
                    Height = 1000,
                    Margin = 0
                }
            };
            barcodeWriter.Renderer = new ZXing.Mobile.BitmapRenderer();
            var bitmap = barcodeWriter.Write(info);
            imageView.Image = bitmap;
        }

        private void styleView()
        {

            buttonContinue.Layer.CornerRadius = 8.0f;
            AccessLabel.Font = Styles.SetHelveticaBoldFont(20);
            NameLabel.Font = Styles.SetHelveticaFont(17);
            TimeoutLabel.Font = Styles.SetHelveticaFont(16);
            textQRHelp.Font = Styles.SetHelveticaFont(15);
            textQRHelp.TextColor = UIColor.Gray;
        }



        public void SetPassportInfo(Domain.Model.Employee.Passport passport, string message)
        {
            TimeoutLabel.AttributedText = Styles.ConvertHTMLStyles(message, TimeoutLabel.Font.Name, TimeoutLabel.Font.PointSize);

            switch (passport.ColorPasaporte)
            {

                case "Gris":
                    textQRHelp.Text = AppDelegate.LanguageBundle.GetLocalizedString("offline_qr_timeout");
                    imageView.Alpha = 1f;
                    viewState.BackgroundColor = Colors.colorStateCaducado;                    
                    break;

                case "Verde":
                    textQRHelp.Text = AppDelegate.LanguageBundle.GetLocalizedString("qrcode_help");
                    imageView.Alpha = 1f;
                    viewState.BackgroundColor = Colors.colorStateInmune;
                    AccessLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("access_allowed");
                    break;

                case "Rojo":
                    imageView.Alpha = 1f;
                    textQRHelp.Text = AppDelegate.LanguageBundle.GetLocalizedString("qrcode_help");
                    viewState.BackgroundColor = Colors.colorStateSintomas;
                    AccessLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("access_not_allowed");
                    TimeoutLabel.Text = "";
                    break;
            }
        }

        public string GetString(string text)
        {
            return AppDelegate.LanguageBundle.GetLocalizedString(text);
        }
    }
}

