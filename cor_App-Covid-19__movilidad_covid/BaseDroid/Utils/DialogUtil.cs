using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ServiceLocator;
using Android.Support.V7.App;

namespace Droid.Utils
{
    public static class DialogUtil
    {

        public static AlertDialog ShowDialog(String title_text, String message, String okButton)
        {
            Android.App.Activity act = Locator.Current.GetService<Android.App.Activity>();
            AlertDialog.Builder builder = new AlertDialog.Builder(act);

            if(title_text!=null)
                builder.SetTitle(title_text);

            builder.SetMessage(message);
            builder.SetCancelable(false);
            builder.SetNegativeButton(okButton, (s, ev) =>
            {
                ((AlertDialog)s).Cancel();
            });
            AlertDialog alert = builder.Create();
            act.RunOnUiThread(() =>
            {
                alert.Show();
            });
            return alert;
        }
     

        public static AlertDialog ShowDialog(String title_text, String message, String okButton, Action action)
        {
            Android.App.Activity act = Locator.Current.GetService<Android.App.Activity>();
            AlertDialog.Builder builder = new AlertDialog.Builder(act);

            if (title_text != null)
                builder.SetTitle(title_text);

            builder.SetMessage(message);
            builder.SetCancelable(false);
            builder.SetNegativeButton(okButton, (s, ev) =>
            {
                if (action != null)
                    action.Invoke();
                ((AlertDialog)s).Cancel();
            });

            AlertDialog alert = builder.Create();
            act.RunOnUiThread(() =>
            {
                alert.Show();
            });
            return alert;

        }

        public static void ShowDialog(String title_text, String message,String NegativeButton,String PositiveButton, Action actionPositive)
        {
            Android.App.Activity act = Locator.Current.GetService<Android.App.Activity>();
            AlertDialog.Builder builder = new AlertDialog.Builder(act);

            if (title_text != null)
                builder.SetTitle(title_text);

            builder.SetMessage(message);
            builder.SetCancelable(false);
            builder.SetPositiveButton(PositiveButton, (s, ev) =>
            {
                if (actionPositive != null)
                    actionPositive.Invoke();
                ((AlertDialog)s).Cancel();
            });
            builder.SetNegativeButton(NegativeButton, (s, ev) =>
            {
                
                ((AlertDialog)s).Cancel();
            });

            act.RunOnUiThread(() =>
            {
                AlertDialog alert = builder.Show();
            });

        }

        public static void ShowDialog(String title_text, String message, String NegativeButton, Action actionNegative, String PositiveButton, Action actionPositive)
        {
            Android.App.Activity act = Locator.Current.GetService<Android.App.Activity>();
            AlertDialog.Builder builder = new AlertDialog.Builder(act);

            if (title_text != null)
                builder.SetTitle(title_text);

            builder.SetMessage(message);
            builder.SetCancelable(false);
            builder.SetPositiveButton(PositiveButton, (s, ev) =>
            {
                if (actionPositive != null)
                    actionPositive.Invoke();
                ((AlertDialog)s).Cancel();
            });
            builder.SetNegativeButton(NegativeButton, (s, ev) =>
            {
                if (actionNegative != null)
                    actionNegative.Invoke();
                ((AlertDialog)s).Cancel();
            });

            act.RunOnUiThread(() =>
            {
                AlertDialog alert = builder.Show();
            });

        }

    }
}