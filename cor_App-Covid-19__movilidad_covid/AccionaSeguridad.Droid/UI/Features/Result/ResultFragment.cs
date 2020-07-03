using Android.Views;
using Droid.UI;
using AccionaSeguridad.Presentation.UI.Features.Result;
using System;
using Android.Support.V4.OS;
using System.Collections.Generic;
using ServiceLocator;
using Acciona.Domain.Model;
using Android.Widget;
using Android.Content.PM;
using Android.Support.V4.App;
using Droid.Utils;

namespace AccionaSeguridad.Droid.UI.Features.Result
{
    public class ResultFragment : BaseFragment<ResultPresenter>, ResultUI
    {
        private TextView label1;
        private TextView label2;
        private ImageView image;
        private ImageView offline;
        private View root;

        public static ResultFragment NewInstance()
        {
            return new ResultFragment();
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override void AssingViews(View view)
        {
            label1 = view.FindViewById<TextView>(Resource.Id.label1);
            label2 = view.FindViewById<TextView>(Resource.Id.label2);
            image = view.FindViewById<ImageView>(Resource.Id.image);
            root = view.FindViewById(Resource.Id.rootFragment);
            offline = view.FindViewById<ImageView>(Resource.Id.offline);
            offline.Visibility = ViewStates.Gone;
            //view.Click += (o, e) => presenter.ScreenTouched();
            view.SetOnTouchListener(new ScreenTouchListener(()=>presenter.ScreenTouched()));
        }

        protected override int GetFragmentLayout()
        {
            return Resource.Layout.result_fragment;
        }

        public void SetInfo(QRInfo info,bool online)
        {
            if(online)
                offline.Visibility = ViewStates.Gone;
            else
                offline.Visibility = ViewStates.Visible;
            switch (info.PasaporteColor)
            {
                case "Verde":
                    label1.Text = GetString(Resource.String.title_pasar);
                    label2.Text = GetString(Resource.String.subtitle_pasar);
                    image.SetImageResource(Resource.Drawable.ok);
                    root.SetBackgroundResource(Resource.Color.colorStateGreen);
                    break;
                case "Rojo":
                    label1.Text = GetString(Resource.String.title_nopasar);
                    label2.Text = GetString(Resource.String.subtitle_nopasar);
                    image.SetImageResource(Resource.Drawable.block);
                    root.SetBackgroundResource(Resource.Color.colorStateRed);
                    break;
                case "Gris":
                    label1.Text = GetString(Resource.String.title_nopasar);
                    label2.Text = GetString(Resource.String.subtitle_caducado);
                    image.SetImageResource(Resource.Drawable.block);
                    root.SetBackgroundResource(Resource.Color.colorStateCaducado);
                    break;
            }
        }

        public void AskTemperature()
        {
            FragmentTransaction transcation = FragmentManager.BeginTransaction();
            var dialog = new AskTemperatureDialogFragment();
            dialog.Cancelable = false;
            dialog.AskResponded += (o, response) => presenter.TemperatureAskResponded(response);
            dialog.Show(transcation, "TemperatureDialog");
        }
    }
}