using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

namespace Droid.Utils
{
    public static class KeyboardUtils
    {
        public static void HideSoftInput(Activity activity)
        {
            InputMethodManager inputMethodManager = (InputMethodManager)activity.GetSystemService(Activity.InputMethodService);
            View currentFocus = activity.CurrentFocus;
            if (currentFocus == null)
            {
                currentFocus = new View(activity);
            }
            inputMethodManager.HideSoftInputFromWindow(currentFocus.WindowToken, 0);
        }

        public static void HideKeyboard(Context context, View view)
        {
            InputMethodManager imm = (InputMethodManager)context.GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(view.WindowToken, 0);
        }

        public static void ShowSoftKeyboard(Activity activity)
        {
            InputMethodManager inputMethodManager = (InputMethodManager)activity.GetSystemService(Activity.InputMethodService);
            inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
        }

        public static void ShowKeyboard(Context context, View view)
        {
            InputMethodManager inputMethodManager = (InputMethodManager)context.GetSystemService(Activity.InputMethodService);
            inputMethodManager.ShowSoftInput(view, ShowFlags.Implicit);
        }
    }
}