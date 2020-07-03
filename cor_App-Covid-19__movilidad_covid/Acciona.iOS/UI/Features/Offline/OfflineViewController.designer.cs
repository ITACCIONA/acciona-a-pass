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

namespace Acciona.iOS.UI.Features.Offline
{
    [Register ("OfflineViewController")]
    partial class OfflineViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel AccessLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton buttonContinue { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel textQRHelp { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TimeoutLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView viewState { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AccessLabel != null) {
                AccessLabel.Dispose ();
                AccessLabel = null;
            }

            if (buttonContinue != null) {
                buttonContinue.Dispose ();
                buttonContinue = null;
            }

            if (imageView != null) {
                imageView.Dispose ();
                imageView = null;
            }

            if (NameLabel != null) {
                NameLabel.Dispose ();
                NameLabel = null;
            }

            if (textQRHelp != null) {
                textQRHelp.Dispose ();
                textQRHelp = null;
            }

            if (TimeoutLabel != null) {
                TimeoutLabel.Dispose ();
                TimeoutLabel = null;
            }

            if (viewState != null) {
                viewState.Dispose ();
                viewState = null;
            }
        }
    }
}