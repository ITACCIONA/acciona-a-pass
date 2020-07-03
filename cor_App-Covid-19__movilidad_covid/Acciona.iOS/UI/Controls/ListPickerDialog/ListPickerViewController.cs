using System;
using System.Collections.Generic;
using System.Linq;
using Acciona.Domain.Model.Base;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using iOS.UI.Styles;
using UIKit;
using Acciona.Domain.Utils;

namespace Acciona.iOS.UI.Controls
{

    /*  Using example
     * 
     *      List<string> items = new List<string>
            {
                "+351",
                "+34",
                "+32"
            };

            ListPickerViewController<string> list = new ListPickerViewController<string>(items);

            list.ModalInPopover = true;
            list.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;

            list.completion = (item) =>
            {

            };

            this.PresentViewController(list, true, () => {

            });
     * 
     * 
     * 
     * 
     * */

    public partial class ListPickerViewController : UIViewController
    {
        static readonly NSString CELL_IDENTIFIER = new NSString("ITEM_CELL");

        public event EventHandler<ListableObject> OnSelected;
        public event EventHandler OnDismissEvent;
        public List<ListableObject> tableData;

        public ListPickerViewController(List<ListableObject> tableData) : base("ListPickerViewController", null)
        {
            this.tableData = tableData;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            TableView.Source = new ItemsDataSource(tableData, SelectedAction);
            TableView.ShowsVerticalScrollIndicator = true;
            TableView.RegisterClassForCellReuse(typeof(UITableViewCell), CELL_IDENTIFIER);

            UITapGestureRecognizer tapOutside = new UITapGestureRecognizer();

            CancelButton.AddTarget(Button_TouchUpInside, UIControlEvent.TouchUpInside);
            CancelButton.Layer.CornerRadius = 3;

            PopUpView.Layer.CornerRadius = 3;

            UIView padding = new UIView(new CGRect(0, 0, 30, 20));
            var RightButton = new UIButton();
            RightButton.SetImage(UIImage.FromBundle("search"), UIControlState.Normal);
            padding.AddSubview(RightButton);
            RightButton.Frame = new CGRect(5, 0, 20, 20);
            RightButton.TintColor = UIColor.Red;
            searchTextField.RightView = padding;
            searchTextField.RightViewMode = UITextFieldViewMode.Always;

            searchTextField.AddTarget(SearchTextField_ValueChanged, UIControlEvent.EditingChanged);

        }

        private void SearchTextField_ValueChanged(object sender, EventArgs e)
        {
            string filterSearch = searchTextField.Text;
            if (tableData == null)
                return;
            if (filterSearch.Trim().Length > 0)
            {
                var filtered = tableData.Where(x => x.GetListText().IgnoreContains(filterSearch));
                TableView.Source = new ItemsDataSource(filtered.ToList(), SelectedAction);
                TableView.ReloadData();
            }
            else
            {
                TableView.Source = new ItemsDataSource(tableData, SelectedAction);
                TableView.ReloadData();
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            TableView.FlashScrollIndicators();
        }

        void SelectedAction(ListableObject selectedItem)
        {
            OnSelected?.Invoke(this, selectedItem);
            DismissView();
        }

        void Button_TouchUpInside(object sender, EventArgs e)
        {
            OnDismissEvent?.Invoke(this, null);
            DismissView();
        }

        void DismissView()
        {
            DispatchQueue.MainQueue.DispatchAsync(() =>
            {
                base.DismissViewControllerAsync(true);
            });
        }

        class ItemsDataSource : UITableViewSource
        {
            static readonly NSString CELL_IDENTIFIER = new NSString("ITEM_CELL");

            Action<ListableObject> selectedAction;
            List<ListableObject> tableData;

            public ItemsDataSource(List<ListableObject> tableData, Action<ListableObject> selectedAction)
            {
                this.tableData = tableData;
                this.selectedAction = selectedAction;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {

                if (tableData != null)
                {
                    return tableData.Count;
                }
                else
                {
                    return 0;
                }
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                var selectedItem = tableData[indexPath.Row];
                selectedAction?.Invoke(selectedItem);
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell(CELL_IDENTIFIER, indexPath);
                string item = tableData[indexPath.Row].GetListText();
                cell.TextLabel.Text = item;
                cell.TextLabel.Lines = 0;
                cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
                cell.LayoutMargins = UIEdgeInsets.Zero;
                return cell;

            }
        }
    }

}