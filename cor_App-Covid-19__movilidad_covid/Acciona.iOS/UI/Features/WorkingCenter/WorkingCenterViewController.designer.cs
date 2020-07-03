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

namespace Acciona.iOS.UI.Features.WorkingCenter
{
    [Register ("WorkingCenterViewController")]
    partial class WorkingCenterViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BackButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Acciona.iOS.UI.Controls.ListTextfield CenterListTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Acciona.iOS.UI.Controls.ListStringTextfield CiudadListTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Label1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Label2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Label3 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel mandatoryLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ModifyButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Acciona.iOS.UI.Controls.ListStringTextfield PaisListTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TitleViewLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BackButton != null) {
                BackButton.Dispose ();
                BackButton = null;
            }

            if (CenterListTextField != null) {
                CenterListTextField.Dispose ();
                CenterListTextField = null;
            }

            if (CiudadListTextField != null) {
                CiudadListTextField.Dispose ();
                CiudadListTextField = null;
            }

            if (Label1 != null) {
                Label1.Dispose ();
                Label1 = null;
            }

            if (Label2 != null) {
                Label2.Dispose ();
                Label2 = null;
            }

            if (Label3 != null) {
                Label3.Dispose ();
                Label3 = null;
            }

            if (mandatoryLabel != null) {
                mandatoryLabel.Dispose ();
                mandatoryLabel = null;
            }

            if (ModifyButton != null) {
                ModifyButton.Dispose ();
                ModifyButton = null;
            }

            if (PaisListTextField != null) {
                PaisListTextField.Dispose ();
                PaisListTextField = null;
            }

            if (TitleViewLabel != null) {
                TitleViewLabel.Dispose ();
                TitleViewLabel = null;
            }
        }
    }
}