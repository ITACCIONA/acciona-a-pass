using System;
using Acciona.Domain.Model.Employee;
using Acciona.Presentation.UI.Features.DataContact;
using BaseIOS.UI;
using CoreGraphics;
using Foundation;
using iOS.UI.Styles;
using UIKit;

namespace Acciona.iOS.UI.Features.ContactData
{
    public partial class ContactDataViewController : BaseViewController<ContactDataPresenter>, ContactDataUI
    {
        private Ficha ficha;

        public ContactDataViewController(Ficha ficha) : base("ContactDataViewController", null)
        {
            this.ficha = ficha;
        }

        protected override void AssingViews()
        {
            BackButton.TouchUpInside += (o, e) => presenter.BackClicked();
            ModifyButton.TouchUpInside += (o, e) => presenter.SaveClicked(PhoneTextField.Text);

            PhoneTextField.ShouldReturn += (textview) =>
            {
                PhoneTextField.ResignFirstResponder();
                return true;
            };
            var doneToolbar = new UIToolbar(new CGRect(0, 0, 300, 40));
            doneToolbar.BarStyle = UIBarStyle.Default;

            var flexSpace = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace, null, null);
            var done = new UIBarButtonItem(AppDelegate.LanguageBundle.GetLocalizedString("msg_ok"), UIBarButtonItemStyle.Done, this, null);
            done.Clicked += (o, e) =>
            {
                PhoneTextField.ResignFirstResponder();                
            };
            var items = new UIBarButtonItem[2];
            items[0] = flexSpace;
            items[1] = done;

            doneToolbar.Items = items;
            doneToolbar.SizeToFit();

            PhoneTextField.InputAccessoryView = doneToolbar;

            styleView();
            applyTraslations();
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
            presenter.ficha = ficha;
        }

        private void applyTraslations()
        {
            TitleViewLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("contact_data_title");

            EmailLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("contact_data_email");
            PhoneLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("contact_data_phone");
            ModifyButton.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("contact_data_modify") , UIControlState.Normal);
        }

        private void styleView()
        {
            TitleViewLabel.Font = Styles.SetHelveticaBoldFont(17);
            EmailLabel.Font = Styles.SetHelveticaBoldFont(12);
            PhoneLabel.Font = Styles.SetHelveticaBoldFont(12);

            EmailLabel.TextColor = UIColor.Gray;
            PhoneLabel.TextColor = UIColor.Gray;

            ModifyButton.Layer.CornerRadius = 4;

            PhoneTextField.Enabled = true;            
        }

        public void setProfileData(Ficha ficha)
        {
            PhoneTextField.Text = ficha.TelefonoEmpleado;
            EmailTextField.Text = ficha.MailEmpleado;
        }

    }
}

