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

namespace Acciona.iOS.UI.Features.Main
{
    [Register ("MainViewController")]
    partial class MainViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton buttonBell { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton buttonPanic { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Acciona.iOS.UI.Controls.MainButtonView buttonPassport { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton buttonPhone { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Acciona.iOS.UI.Controls.MainButtonView buttonProfile { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView contentView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView tabControlContent { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (buttonBell != null) {
                buttonBell.Dispose ();
                buttonBell = null;
            }

            if (buttonPanic != null) {
                buttonPanic.Dispose ();
                buttonPanic = null;
            }

            if (buttonPassport != null) {
                buttonPassport.Dispose ();
                buttonPassport = null;
            }

            if (buttonPhone != null) {
                buttonPhone.Dispose ();
                buttonPhone = null;
            }

            if (buttonProfile != null) {
                buttonProfile.Dispose ();
                buttonProfile = null;
            }

            if (contentView != null) {
                contentView.Dispose ();
                contentView = null;
            }

            if (tabControlContent != null) {
                tabControlContent.Dispose ();
                tabControlContent = null;
            }
        }
    }
}