using System;
using Acciona.Domain.Model.Employee;
using Acciona.Presentation.UI.Features.AlertDetail;
using BaseIOS.UI;
using Foundation;
using iOS.UI.Styles;
using UIKit;

namespace Acciona.iOS.UI.Features.AlertDetail
{
    public partial class AlertDetailViewController : BaseViewController<AlertDetailPresenter>,AlertDetailUI
    {
        private Alert alert;

        public AlertDetailViewController() : base("AlertDetailViewController", null)
        {
        }        

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override void AssingViews()
        {
            buttonBack.TouchUpInside += (o, e) => presenter.BackClicked();

            TitleViewLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("alerts_notifications");

            styleView();
        }

        public void SetAlert(Alert alert)
        {
            this.alert = alert;
            configureView();
        }

        private void configureView()
        {
            TitleLabel.Text = alert.Title;
            DateLabel.Text = alert.FechaNotificacion.ToString(AppDelegate.LanguageBundle.GetLocalizedString("filter_date_format")) + " - " + alert.FechaNotificacion.ToString("HH:mm");
            DescriptionTextView.Text = alert.Comment;
        }

        private void styleView()
        {
            TitleViewLabel.Font = Styles.SetHelveticaBoldFont(17);
            TitleLabel.Font = Styles.SetHelveticaBoldFont(17);
            DateLabel.Font = Styles.SetHelveticaFont(16);
            DescriptionTextView.Font = Styles.SetHelveticaFont(17);
            DateLabel.TextColor = UIColor.LightGray;
        }
    }
}

