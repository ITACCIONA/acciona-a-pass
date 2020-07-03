using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Droid.Utils
{
    public static class BitmapUtils
    {
        public static Bitmap LoadBitmapFromView(View v)
        {
            if (v.Height == 0 || v.Width == 0)
                return null;
            Bitmap b = Bitmap.CreateBitmap(v.Width, v.Height, Bitmap.Config.Argb8888);
            Canvas c = new Canvas(b);
            v.Layout(v.Left, v.Top, v.Right, v.Bottom);
            v.Draw(c);
            return b;
        }
    }
}