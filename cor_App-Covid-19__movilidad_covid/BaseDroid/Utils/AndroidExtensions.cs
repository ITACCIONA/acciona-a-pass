using Android.Graphics;
using Android.Util;
using System;

namespace Droid.Utils
{
    public static class AndroidExtensions
    {
        public static Bitmap ConvertBase64ToBitmap(this string base64String)
        {
            try
            {
                byte[] decodedString = Base64.Decode(base64String, Base64Flags.Default);
                Bitmap decodedByte = BitmapFactory.DecodeByteArray(decodedString, 0, decodedString.Length);
                return decodedByte;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public static Color ConvertHexadecimaltoColor(this string hexadecimalColor)
        {
            return Color.ParseColor(hexadecimalColor);
        }
    }

   
}