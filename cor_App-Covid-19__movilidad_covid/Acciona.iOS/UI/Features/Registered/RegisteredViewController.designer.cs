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

namespace Acciona.iOS.UI.Features.Registered
{
    [Register ("RegisteredViewController")]
    partial class RegisteredViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton button { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (button != null) {
                button.Dispose ();
                button = null;
            }
        }
    }
}