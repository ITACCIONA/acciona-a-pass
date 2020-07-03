using System;
using Acciona.Domain.Model.Employee;
using Foundation;
using UIKit;
using iOS.UI.Styles;

namespace Acciona.iOS.UI.Controls.AlertTableViewCell
{
    public partial class AlertTableViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("AlertTableViewCell");
        public static readonly UINib Nib;

        static AlertTableViewCell()
        {
            Nib = UINib.FromName("AlertTableViewCell", NSBundle.MainBundle);
        }

        protected AlertTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        internal void SetAlert(Alert alert)
        {
            styleView();

            TitleLabel.Text = alert.Title;
            DateLabel.Text = alert.FechaNotificacion.ToString(AppDelegate.LanguageBundle.GetLocalizedString("filter_date_format")) + " - " + alert.FechaNotificacion.ToString("HH:mm");
            DescriptionLabel.Text = alert.Comment;

            if (!alert.Read)
            {
                ContainerView.BackgroundColor = Colors.colorNotRead;
            }
            else
            {
                ContainerView.BackgroundColor = UIColor.White;
            }
        }

        private void styleView()
        {
            TitleLabel.Font = Styles.SetBoldFont(13);
            DateLabel.Font = Styles.SetRegularFont(13);
            DescriptionLabel.Font = Styles.SetRegularFont(13);
            DateLabel.TextColor = UIColor.LightGray;

            ContainerView.setBorderShadow();
        }

    }
}
