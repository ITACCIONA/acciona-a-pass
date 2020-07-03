using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;

namespace AccionaSeguridad.Droid.Utils
{
    public class TextWatcher : Java.Lang.Object, ITextWatcher
    {
        public event EventHandler<string> TextChanging;
        public void AfterTextChanged(IEditable s)
        {
            TextChanging?.Invoke(this, s.ToString());
        }

        public void BeforeTextChanged(Java.Lang.ICharSequence s, int start, int count, int after)
        {
            //
        }

        public void OnTextChanged(Java.Lang.ICharSequence s, int start, int before, int count)
        {
            //
        }
    }
}