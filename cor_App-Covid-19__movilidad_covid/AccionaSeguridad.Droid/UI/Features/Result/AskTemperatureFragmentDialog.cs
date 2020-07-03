using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace AccionaSeguridad.Droid.UI.Features.Result
{
    public class AskTemperatureDialogFragment:DialogFragment
    {

        private View buttonHigh;
        private View buttonLow;
        private View buttonSkip;

        public event EventHandler<bool?> AskResponded;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStyle((int)Android.App.DialogFragmentStyle.Normal, Resource.Style.FullScreenDialogStyle);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.temperature_dialog_fragment, container, false);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            buttonHigh = view.FindViewById(Resource.Id.buttonHidht);
            buttonHigh.Click += ButtonTemperature_Click;
            buttonLow = view.FindViewById(Resource.Id.buttonLow);
            buttonLow.Click += ButtonTemperature_Click;
            buttonSkip = view.FindViewById(Resource.Id.buttonSkip);
            buttonSkip.Click += ButtonTemperature_Click;

        }

        private void ButtonTemperature_Click(object sender, EventArgs e)
        {
            bool? value= (bool?)null; 
            if (sender == buttonHigh)
                value = true;
            else if (sender == buttonLow)
                value = false;
            //else if (sender == buttonSkip)
            //    value = (bool?)null;
            AskResponded?.Invoke(this, value);
            Dismiss();
        }
    }
}
