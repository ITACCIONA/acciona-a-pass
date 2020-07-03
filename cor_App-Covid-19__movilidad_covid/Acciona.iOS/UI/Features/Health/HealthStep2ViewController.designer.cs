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
    [Register ("HealthStep2ViewController")]
    partial class HealthStep2ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton buttonNo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton buttonYes { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Acciona.iOS.UI.Controls.IndicatorBar indicator { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel labelHelp1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel labelHelp2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel labelSubtitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel labelTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (buttonNo != null) {
                buttonNo.Dispose ();
                buttonNo = null;
            }

            if (buttonYes != null) {
                buttonYes.Dispose ();
                buttonYes = null;
            }

            if (indicator != null) {
                indicator.Dispose ();
                indicator = null;
            }

            if (labelHelp1 != null) {
                labelHelp1.Dispose ();
                labelHelp1 = null;
            }

            if (labelHelp2 != null) {
                labelHelp2.Dispose ();
                labelHelp2 = null;
            }

            if (labelSubtitle != null) {
                labelSubtitle.Dispose ();
                labelSubtitle = null;
            }

            if (labelTitle != null) {
                labelTitle.Dispose ();
                labelTitle = null;
            }
        }
    }
}