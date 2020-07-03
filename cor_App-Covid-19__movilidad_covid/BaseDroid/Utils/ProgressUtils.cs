using System;
using Android.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;

namespace Droid.Utils
{
    public static class ProgressUtils
    {
        private static string DEFAULT_LAYOUT = "view_progress_message";

        /// <summary>
        /// If progress view has previously been created, layoutName=null can be used to just update the text
        /// </summary>
        public static void ShowLoading(string message, string layoutName, string template, Activity activity, View view, ref View _progress)
        {
            ViewGroup root;
            if (view == null)
                root = activity.FindViewById<RelativeLayout>(Resource.Id.root);
            else
                root = view.FindViewById<RelativeLayout>(Resource.Id.rootFragment);

            if (root != null)
            {
                if (_progress == null)
                {
                    var layoutId = GetLayoutId(layoutName, activity);

                    _progress = View.Inflate(activity, layoutId, null);
                    _progress.LayoutParameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
                    _progress.SetOnTouchListener(new BlockTouchListener());
                    ViewCompat.SetElevation(_progress, 10);

                    SetMessage(message, template, activity, _progress);

                    root.AddView(_progress);
                }
                else
                {
                    SetMessage(message, template, activity, _progress);
                }
            }
        }

        private static int GetLayoutId(string layoutName, Activity activity)
        {
            if (layoutName == null)
                layoutName = DEFAULT_LAYOUT;

            int id = activity.Resources.GetIdentifier(layoutName, "layout", activity.PackageName);
            if (id <= 0)
                id = activity.Resources.GetIdentifier(DEFAULT_LAYOUT, "layout", activity.PackageName);

            return id;
        }

        private static void SetMessage(string message, string template, Activity activity, View progress)
        {
            int id = activity.Resources.GetIdentifier(message, "string", activity.PackageName);
            if (id > 0)
                message = activity.Resources.GetString(id);

            if (template != null)
            {
                id = activity.Resources.GetIdentifier(template, "string", activity.PackageName);
                if (id > 0)
                    template = activity.Resources.GetString(id);

                message = String.Format(template, message);
            }

            TextView messageTV = progress.FindViewById<TextView>(Resource.Id.message);
            messageTV.Text = message;
        }
    }
}
