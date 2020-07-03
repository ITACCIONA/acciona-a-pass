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

namespace Acciona.iOS.UI.Features.Health
{
    [Register ("HealthStep1ViewController")]
    partial class HealthStep1ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton buttonConfirm { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DescriptionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Acciona.iOS.UI.Controls.IndicatorBar indicator { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel labelSymptomsHelp { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel labelTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel SymptonLabel1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel SymptonLabel2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView SymptonView1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView SymptonView2 { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (buttonConfirm != null) {
                buttonConfirm.Dispose ();
                buttonConfirm = null;
            }

            if (DescriptionLabel != null) {
                DescriptionLabel.Dispose ();
                DescriptionLabel = null;
            }

            if (indicator != null) {
                indicator.Dispose ();
                indicator = null;
            }

            if (labelSymptomsHelp != null) {
                labelSymptomsHelp.Dispose ();
                labelSymptomsHelp = null;
            }

            if (labelTitle != null) {
                labelTitle.Dispose ();
                labelTitle = null;
            }

            if (SymptonLabel1 != null) {
                SymptonLabel1.Dispose ();
                SymptonLabel1 = null;
            }

            if (SymptonLabel2 != null) {
                SymptonLabel2.Dispose ();
                SymptonLabel2 = null;
            }

            if (SymptonView1 != null) {
                SymptonView1.Dispose ();
                SymptonView1 = null;
            }

            if (SymptonView2 != null) {
                SymptonView2.Dispose ();
                SymptonView2 = null;
            }
        }
    }
}