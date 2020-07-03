// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Acciona.iOS.UI.Features.Alerts
{
    [Register ("AlertsViewController")]
    partial class AlertsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton buttonBack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView TableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TitleViewLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (buttonBack != null) {
                buttonBack.Dispose ();
                buttonBack = null;
            }

            if (TableView != null) {
                TableView.Dispose ();
                TableView = null;
            }

            if (TitleViewLabel != null) {
                TitleViewLabel.Dispose ();
                TitleViewLabel = null;
            }
        }
    }
}