using CoreGraphics;
using Foundation;
using ObjCRuntime;
using System;
using System.ComponentModel;
using UIKit;

namespace Acciona.iOS.UI.Controls
{
    [DesignTimeVisible(true)]
    public partial class MainButtonView : UIButton
    {
        protected MainButtonView(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public static MainButtonView Create()
        {
            var arr = NSBundle.MainBundle.LoadNib("MainButtonView", null, null);
            var v = Runtime.GetNSObject<MainButtonView>(arr.ValueAt(0));            
            return v;
        }



        public MainButtonView(NSCoder coder) : base(coder)
        {
            var arr = NSBundle.MainBundle.LoadNib("MainButtonView", null, null);
            Runtime.GetNSObject<MainButtonView>(arr.ValueAt(0));
            Initialize();
        }


        public override void AwakeFromNib()
        {
            // Called when loaded from xib or storyboard.
            Initialize();
        }

        void Initialize()
        {            
            CGSize imageSize = ImageView.Frame.Size;
            CGSize titleSize = TitleLabel.Frame.Size;

            nfloat totalHeight = (imageSize.Height + titleSize.Height + 2.0f);

            ImageEdgeInsets = new UIEdgeInsets(-(totalHeight - imageSize.Height), 0.0f, 0.0f, -titleSize.Width);
            TitleEdgeInsets = new UIEdgeInsets(0.0f, -imageSize.Width, -(totalHeight - titleSize.Height), 0.0f);

            TitleLabel.MinimumScaleFactor = 0.7f;
            TitleLabel.Lines = 0;
            TitleLabel.AdjustsFontSizeToFitWidth = true;
            //TitleLabel.Font = UIFont.BoldSystemFontOfSize(TitleLabel.Font.PointSize);            
        }

    }
}
