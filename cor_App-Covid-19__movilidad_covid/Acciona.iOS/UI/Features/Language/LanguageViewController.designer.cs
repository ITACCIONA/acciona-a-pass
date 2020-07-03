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

namespace Acciona.iOS.UI.Features.Language
{
    [Register ("LanguageViewController")]
    partial class LanguageViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BackButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton EnglishButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ModifyButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SpanishButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TitleViewLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BackButton != null) {
                BackButton.Dispose ();
                BackButton = null;
            }

            if (EnglishButton != null) {
                EnglishButton.Dispose ();
                EnglishButton = null;
            }

            if (ModifyButton != null) {
                ModifyButton.Dispose ();
                ModifyButton = null;
            }

            if (SpanishButton != null) {
                SpanishButton.Dispose ();
                SpanishButton = null;
            }

            if (TitleViewLabel != null) {
                TitleViewLabel.Dispose ();
                TitleViewLabel = null;
            }
        }
    }
}