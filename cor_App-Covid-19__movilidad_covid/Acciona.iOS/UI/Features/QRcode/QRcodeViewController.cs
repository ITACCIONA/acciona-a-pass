using System;
using Acciona.iOS.UI.Features.Web;
using Acciona.Presentation.UI.Features.QRcode;
using BaseIOS.UI;
using Foundation;
using iOS.UI.Styles;
using UIKit;

namespace Acciona.iOS.UI.Features.QRcode
{
    public partial class QRcodeViewController : BaseViewController<QRcodePresenter>,QRcodeUI
    {
        private bool caducado;

        public QRcodeViewController() : base("QRcodeViewController", null)
        {
        }        

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override void AssingViews()
        {            

            buttonRenew.TouchUpInside += (o, e) => presenter.RenewClick();
            buttonRenew.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("qrcode_renew"), UIControlState.Normal);
            styleView();
            buttonRenew.Hidden = true;            
            NameLabel.Text = "";
            TimeoutLabel.Text = "";
            AccessLabel.Text = "";
            textQRHelp.Text = "";
            viewState.BackgroundColor = UIColor.White;                               
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            UITapGestureRecognizer gestureRecognizerToDo = new UITapGestureRecognizer(() => presenter.ToDoClicked());
            gestureRecognizerToDo.NumberOfTapsRequired = 1;
            ToDoLabel.UserInteractionEnabled = true;
            ToDoLabel.AddGestureRecognizer(gestureRecognizerToDo);
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

            buttonRenew.Layer.CornerRadius = 8.0f;
            AccessLabel.Font = Styles.SetHelveticaBoldFont(20);
            NameLabel.Font = Styles.SetHelveticaFont(17);
            TimeoutLabel.Font = Styles.SetHelveticaFont(16);            
            textQRHelp.Font = Styles.SetHelveticaFont(15);
            ToDoLabel.Font = Styles.SetHelveticaFont(12);
            textQRHelp.TextColor = UIColor.Gray;

            ToDoLabel.AttributedText = Styles.ConvertHTMLStyles(AppDelegate.LanguageBundle.GetLocalizedString("qrcode_todo"), ToDoLabel.Font.FamilyName, ToDoLabel.Font.PointSize);
        }


        public void SetCaducado(bool caducado)
        {
            this.caducado = caducado;
        }

        public void SetPassportInfo(Domain.Model.Employee.Passport passport, string message,bool offline)
        {
            if(offline)
                NameLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("offline_name");
            else
                NameLabel.Text = passport.NombreEmpleado;
            TimeoutLabel.AttributedText = Styles.ConvertHTMLStyles(message, TimeoutLabel.Font.Name, TimeoutLabel.Font.PointSize);            
            if(passport.HasMessage)
                ToDoLabel.Hidden = false;
            else
                ToDoLabel.Hidden = true;

            switch (passport.ColorPasaporte)
            {
                
                case "Gris":
                    textQRHelp.Text = AppDelegate.LanguageBundle.GetLocalizedString("qrcode_help");
                    imageView.Alpha = 1f;
                    viewState.BackgroundColor = Colors.colorStateCaducado;
                    caducado = true;
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
                    if (caducado)
                    {
                        caducado = false;
                        textQRHelp.Text = AppDelegate.LanguageBundle.GetLocalizedString("qrcode_help_timeout");
                        buttonRenew.Hidden = false;
                    }
                    break;
            }
            if (caducado)
            {
                textQRHelp.Text = AppDelegate.LanguageBundle.GetLocalizedString("qrcode_help_timeout");
                buttonRenew.Hidden = false;
                viewState.BackgroundColor = Colors.colorStateCaducado;
                imageView.Alpha = 0.5f;
                AccessLabel.Text = "";
                ToDoLabel.Hidden = true;
            }
            else
            {
                if (!passport.ColorPasaporte.Equals("Rojo"))
                    buttonRenew.Hidden = true;
            }            
        }

        public void ShowStateChange()
        {
            var rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController as UINavigationController;
            var controller = new ChangeStateViewController();
            controller.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            controller.ShowToDo += (o, e) => presenter.ToDoClicked();
            rootViewController.PresentViewController(controller, false, null);
        }

        public void ShowTodo(string url)
        {
            var rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController as UINavigationController;
            var controller = new  WebViewController(url);
            controller.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            rootViewController.PresentViewController(controller, true, null);
        }

        public string GetString(string text)
        {
            return AppDelegate.LanguageBundle.GetLocalizedString(text);
        }
    }
}

