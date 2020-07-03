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

namespace Acciona.iOS.UI.Features.MedicalTest
{
    [Register ("MedicalTestViewController")]
    partial class MedicalTestViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton buttonBack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView contentView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel labelTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (buttonBack != null) {
                buttonBack.Dispose ();
                buttonBack = null;
            }

            if (contentView != null) {
                contentView.Dispose ();
                contentView = null;
            }

            if (labelTitle != null) {
                labelTitle.Dispose ();
                labelTitle = null;
            }
        }
    }
}