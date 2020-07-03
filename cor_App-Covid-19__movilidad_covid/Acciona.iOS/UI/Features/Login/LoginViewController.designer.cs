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

namespace Acciona.iOS.UI.Features.Login
{
    [Register ("LoginViewController")]
    partial class LoginViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton LoginStateButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView LoginStateImgView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LoginStateLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView LogoImgView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (LoginStateButton != null) {
                LoginStateButton.Dispose ();
                LoginStateButton = null;
            }

            if (LoginStateImgView != null) {
                LoginStateImgView.Dispose ();
                LoginStateImgView = null;
            }

            if (LoginStateLabel != null) {
                LoginStateLabel.Dispose ();
                LoginStateLabel = null;
            }

            if (LogoImgView != null) {
                LogoImgView.Dispose ();
                LogoImgView = null;
            }
        }
    }
}