using System;
using CoreText;
using Foundation;
using UIKit;

namespace iOS.UI.Styles
{
    public static class Styles
    {
        public static UIFont SetBoldFont(float size)
        {
            return UIFont.BoldSystemFontOfSize(size);
        }

        public static UIFont SetRegularFont(float size)
        {
            return UIFont.SystemFontOfSize(size);
        }

        public static UIFont SetThinFont(float size)
        {
            return UIFont.SystemFontOfSize(size, UIFontWeight.Thin);
        }

        public static UIFont SetHelveticaFont(float size)
        {
            return UIFont.FromName("HelveticaNeue", size);
        }

        public static UIFont SetHelveticaBoldFont(float size)
        {
            return UIFont.FromName("HelveticaNeue-Bold", size);
        }

        public static NSAttributedString ConvertHTMLStyles(string htmlText, string fontFamily, nfloat fontSize)
        {
            var attr = new NSAttributedStringDocumentAttributes();
            var nsError = new NSError();
            attr.DocumentType = NSDocumentType.HTML;

            var htmlStyle = string.Format("<style>body{{font-family:'{0}'; font-size:{1}px;}}</style>",
                        fontFamily,
                        fontSize);
            var modifiedFont = String.Format("{0}{1}", htmlStyle, htmlText);
            return new NSAttributedString(new NSString(modifiedFont).Encode(NSStringEncoding.Unicode, true), attr, ref nsError);
        }

        public static NSAttributedString ConvertHTMLStyles(string htmlText, string fontFamily, nfloat fontSize, UITextAlignment textAlign=UITextAlignment.Left)
        {
            var attr = new NSAttributedStringDocumentAttributes();
            var nsError = new NSError();
            attr.DocumentType = NSDocumentType.HTML;            

            var htmlStyle = string.Format("<style>body{{font-family:'{0}'; font-size:{1}px;}}</style>",
                        fontFamily,
                        fontSize);
            var modifiedFont = String.Format("{0}{1}", htmlStyle, htmlText);
            var attstring= new NSMutableAttributedString(new NSAttributedString(new NSString(modifiedFont).Encode(NSStringEncoding.Unicode, true), attr, ref nsError));
            var style = new NSMutableParagraphStyle();
            style.Alignment = textAlign;
            attstring.AddAttribute(CTStringAttributeKey.ParagraphStyle ,style, new NSRange(0, attstring.Length));
            return attstring;
        }

        public static UIView StyleCircle(this UIView view,
                                        UIColor backgroundColor,
                                        UIColor borderColor,
                                        float borderWidth)
        {
            double min = Math.Min(view.Frame.Width, view.Frame.Width);
            view.Layer.CornerRadius = (float)(min / 2.0);
            view.Layer.BorderColor = borderColor.CGColor;
            view.Layer.BorderWidth = borderWidth;
            view.BackgroundColor = backgroundColor;
            view.ClipsToBounds = true;
            return view;
        }

        public static UIView setBorderShadow(this UIView view)
        {
            view.Layer.ShadowColor = UIColor.LightGray.CGColor;
            view.Layer.ShadowOpacity = 2.0f;
            view.Layer.ShadowRadius = 3.0f;
            view.Layer.ShadowOffset = new System.Drawing.SizeF(0f, 0f);
            view.Layer.MasksToBounds = false;

            view.Layer.BorderColor = UIColor.LightGray.CGColor;
            view.Layer.BorderWidth = 0.5f;

            return view;
        }

        public static UIView setDownBorderShadow(this UIView view)
        {
            view.Layer.ShadowColor = UIColor.LightGray.CGColor;
            view.Layer.ShadowOpacity = 2.0f;
            view.Layer.ShadowRadius = 3.0f;
            view.Layer.ShadowOffset = new System.Drawing.SizeF(0f, 4f);
            view.Layer.MasksToBounds = false;

            return view;
        }
        public static UIView setLittleDownBorderShadow(this UIView view)
        {
            view.Layer.ShadowColor = UIColor.LightGray.CGColor;
            view.Layer.ShadowOpacity = 0.5f;
            view.Layer.ShadowRadius = 1.5f;
            view.Layer.ShadowOffset = new System.Drawing.SizeF(0f, 2f);
            view.Layer.MasksToBounds = false;

            return view;
        }
    }
}

