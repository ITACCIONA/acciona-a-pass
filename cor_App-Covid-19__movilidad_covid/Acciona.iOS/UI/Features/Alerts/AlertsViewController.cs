using System;
using System.Collections.Generic;
using System.Linq;
using Acciona.Domain.Model.Employee;
using Acciona.iOS.UI.Controls.AlertTableViewCell;
using Acciona.Presentation.UI.Features.Alerts;
using BaseIOS.UI;
using Foundation;
using iOS.UI.Styles;
using UIKit;

namespace Acciona.iOS.UI.Features.Alerts
{
    public partial class AlertsViewController : BaseViewController<AlertsPresenter>,AlertsUI
    {

        private ItemsDataSource tableViewDataSource = new ItemsDataSource(cellIds);
        private List<Alert> elements;
        private static readonly string[] cellIds = { "alert-cell"};

        public AlertsViewController() : base("AlertsViewController", null)
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
            TitleViewLabel.Font = Styles.SetHelveticaBoldFont(17);

            TableView.Source = tableViewDataSource;
            tableViewDataSource.CellTapped = CellSelected;

            TableView.RegisterNibForCellReuse(UINib.FromName("AlertTableViewCell", NSBundle.MainBundle), cellIds[0]);

        }
        
        #region Presenter y UI

        public void CellSelected(NSIndexPath position)
        {
            var alert = getAlertFromPosition(position);
            presenter.OpenAlert(alert);
        }

        public void SetAlerts(IEnumerable<Alert> alerts)
        {
            elements = alerts.ToList();
            tableViewDataSource.SetAlerts(elements);
            TableView.ReloadData();
        }

        #endregion

        private Alert getAlertFromPosition (NSIndexPath position)
        {
            Alert alert;
            if (position.Section == 0)
            {
                alert = elements.Where(x => !x.Read).ToList()[position.Row];
            }
            else
            {
                alert = elements.Where(x => x.Read).ToList()[position.Row];
            }
            return alert;
        }

        private class ItemsDataSource : UITableViewSource
        {
            private List<Alert> alerts;
            private readonly string[] cellIds;

            public Action<NSIndexPath> CellTapped;
            string HeaderIdentifier = "TableCell";

            private Alert getAlertFromPosition(NSIndexPath position)
            {
                Alert alert;
                if (position.Section == 0)
                {
                    alert = alerts.Where(x => !x.Read).ToList()[position.Row];
                }
                else
                {
                    alert = alerts.Where(x => x.Read).ToList()[position.Row];
                }
                return alert;
            }

            public ItemsDataSource(string[] cellIds)
            {
                this.cellIds = cellIds;
                alerts = new List<Alert>();
            }

            public void SetAlerts(List<Alert> alerts)
            {
                this.alerts = alerts;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var alert = getAlertFromPosition(indexPath);
                var cell = tableView.DequeueReusableCell(cellIds[0], indexPath) as AlertTableViewCell;
                cell.SetAlert(alert);
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                return cell;
            }

            public override nint RowsInSection(UITableView tableView, nint section)
            {
                var a = alerts.Where(x => !x.Read).Count();
                var b = alerts.Where(x => x.Read).Count();
                if (section == 0)
                {
                    return alerts.Where(x => !x.Read).Count();
                }
                else
                {
                    return alerts.Where(x => x.Read).Count();
                }
            }

            public override nint NumberOfSections(UITableView tableView)
            {
                return 2;
            }

            #region Delegate
            [Export("tableView:didSelectRowAtIndexPath:")]
            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                CellTapped?.Invoke(indexPath);
            }

            [Export("tableView:viewForHeaderInSection:")]
            public override UIView GetViewForHeader(UITableView tableView, nint section)
            {
                UITableViewCell header = new UITableViewCell(UITableViewCellStyle.Default, HeaderIdentifier);
                header.TextLabel.Font = Styles.SetHelveticaFont(17);


                var attr = new NSAttributedStringDocumentAttributes();
                var nsError = new NSError();
                attr.DocumentType = NSDocumentType.HTML;

                if (section == 0)
                {
                    var html = String.Format(AppDelegate.LanguageBundle.GetLocalizedString("alerts_not_read"), "<b><font color='red'>" + alerts.Where(x => !x.Read).Count().ToString() + "</font></b>");
                    var htmlStyle = string.Format("<style>body{{font-family:'{0}'; font-size:{1}px;}}</style>",
                            header.TextLabel.Font.FamilyName,
                            header.TextLabel.Font.PointSize);
                    var modifiedFont = String.Format("{0}{1}", htmlStyle, html);
                    header.TextLabel.AttributedText = new NSAttributedString(new NSString(modifiedFont).Encode(NSStringEncoding.Unicode, true), attr, ref nsError);
                }
                else
                {
                    var html = String.Format(AppDelegate.LanguageBundle.GetLocalizedString("alerts_read"), "<b><font color='red'>" + alerts.Where(x => x.Read).Count().ToString() + "</font></b>");
                    var htmlStyle = string.Format("<style>body{{font-family:'{0}'; font-size:{1}px;}}</style>",
                            header.TextLabel.Font.FamilyName,
                            header.TextLabel.Font.PointSize);
                    var modifiedFont = String.Format("{0}{1}", htmlStyle, html);
                    header.TextLabel.AttributedText = new NSAttributedString(new NSString(modifiedFont).Encode(NSStringEncoding.Unicode, true), attr, ref nsError);
                }

                return header;
            }

            [Export("tableView:heightForHeaderInSection:")]
            public override nfloat GetHeightForHeader(UITableView tableView, nint section)
            {
                return 60;
            }
            #endregion
        }

    }

}

