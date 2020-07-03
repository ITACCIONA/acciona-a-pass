using System;
using System.ComponentModel;
using System.Linq;
using Acciona.iOS.Utils;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Acciona.iOS.UI.Controls
{
    [Register("IndicatorBar"), DesignTimeVisible(true)]
    public class IndicatorBar : UIView
    {
        private UIColor activeColor = "#ff0000".ToUIColor();
        private UIColor normalColor = "#f3f3f3".ToUIColor();
        private int numIndicators = 4;
        private int activeIndicators = 1;
        private float barsCornerRadius = 2;
        private float barsSeparation = 10;

        public IndicatorBar(IntPtr intPtr) : base(intPtr)
        {
        }

        public IndicatorBar(NSCoder coder) : base(coder)
        {
        }

        public IndicatorBar(CGRect frame) : base(frame)
        {
        }

        public override void Draw(CGRect rect)
        {
            var width = rect.Size.Width;
            var height = rect.Size.Height;

            var barWidth = (width - (numIndicators - 1) * barsSeparation) / numIndicators;

            for(int i = 0; i < numIndicators; i++)
            {
                if (i < activeIndicators)
                    DrawBar(new CGPoint(i * (barWidth + barsSeparation), height), barWidth, height, activeColor);
                else
                    DrawBar(new CGPoint(i * (barWidth + barsSeparation), height), barWidth, height, normalColor);
            }
            
        }

        public void Set(int numIndicators,int activeIndicators)
        {
            this.numIndicators = numIndicators;
            this.activeIndicators = activeIndicators;
            SetNeedsDisplay();
        }
        

        private void DrawBar(CGPoint initialPoint, nfloat width, nfloat height, UIColor color)
        {
            var path = UIBezierPath.FromRoundedRect(new CGRect(initialPoint.X, initialPoint.Y - height, width, height),  UIRectCorner.AllCorners , new CGSize(barsCornerRadius, barsCornerRadius));

            var rectLayer = new CAShapeLayer
            {
                Path = path.CGPath,
                FillColor = color.CGColor,
                ContentsScale = UIScreen.MainScreen.Scale,
                AllowsEdgeAntialiasing = true
            };

            Layer.AddSublayer(rectLayer);
        }
    }
}
