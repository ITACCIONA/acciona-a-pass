using CoreGraphics;
using Foundation;
using ObjCRuntime;
using System;
using UIKit;

namespace BaseIOS
{
    public partial class LoadingView : UIView
    {
        public static readonly NSString Key = new NSString("LoadingView");
        public static readonly UINib Nib;

        static LoadingView()
        {
            Nib = UINib.FromName("LoadingView", NSBundle.MainBundle);
        }

        public static LoadingView Create()
        {
            var arr = NSBundle.MainBundle.LoadNib("LoadingView", null, null);
            var v = Runtime.GetNSObject<LoadingView>(arr.ValueAt(0));

            return v;
        }


        protected LoadingView(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        internal void ConfigureView(CGRect frame)
        {
            this.Frame = frame;
        }

        internal void StartAnimation()
        {
            indicator.StartAnimating();
        }
    }
}