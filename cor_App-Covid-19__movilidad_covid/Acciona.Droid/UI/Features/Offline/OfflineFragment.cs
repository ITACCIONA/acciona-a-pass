using Android.Views;
using Droid.UI;
using Acciona.Presentation.UI.Features.Offline;
using Acciona.Droid.Services;
using Android.Support.V4.OS;
using System.Collections.Generic;
using ServiceLocator;
using Acciona.Domain.Model;
using Android.Widget;
using Android.Content.PM;
using Android.Content;
using Acciona.Domain;
using Android.Net;
using Android.Text;
using Android.OS;

namespace Acciona.Droid.UI.Features.Offline
{
    public class OfflineFragment : BaseFragment<OfflinePresenter>, OfflineUI
    {

        private ImageView imageView;
        private TextView textUserName, tvTimeOutLabel, buttonContinue, textAccess;
        private TextView textQRHelp;
        private LinearLayout llStateView;
        
        public static OfflineFragment NewInstance()
        {
            return new OfflineFragment();
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override int GetFragmentLayout()
        {
            return Resource.Layout.offline_fragment;
        }

        protected override void AssingViews(View view)
        {
            llStateView = view.FindViewById<LinearLayout>(Resource.Id.llStateView);
            imageView = view.FindViewById<ImageView>(Resource.Id.image);
            textUserName = view.FindViewById<TextView>(Resource.Id.tvUserName);
            textAccess = view.FindViewById<TextView>(Resource.Id.tvAccess);
            textQRHelp = view.FindViewById<TextView>(Resource.Id.qrHelp);
            buttonContinue = view.FindViewById<TextView>(Resource.Id.buttonContinue);
            tvTimeOutLabel = view.FindViewById<TextView>(Resource.Id.tvTimeOutLabel);                        
            buttonContinue.Click += (o, e) => presenter.ContinueClick();
        }

        public void SetQRInfo(string info)
        {
            var barcodeWriter = new ZXing.Mobile.BarcodeWriter
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 1000,
                    Height = 1000,
                    Margin = 0
                }
            };
            barcodeWriter.Renderer = new ZXing.Mobile.BitmapRenderer();
            var bitmap = barcodeWriter.Write(info);
            imageView.SetImageBitmap(bitmap);
        }
        

        public void SetPassportInfo(Domain.Model.Employee.Passport passport, string message)
        {
            textAccess.Text = "";
            ISpanned html;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                html = Html.FromHtml(message, FromHtmlOptions.ModeLegacy);
            else
                html = Html.FromHtml(message);
            tvTimeOutLabel.SetText(html, TextView.BufferType.Spannable);
            
            switch (passport.ColorPasaporte)
            {
                case "Gris":
                    textQRHelp.Text = Context.GetString(Resource.String.offline_qr_timeout);
                    imageView.Alpha = 1f;
                    llStateView.SetBackgroundColor(Resources.GetColor(Resource.Color.colorStateCaducado));                    
                    break;

                case "Verde":
                    textQRHelp.Text = Context.GetString(Resource.String.qrcode_help_normal);
                    imageView.Alpha = 1f;
                    llStateView.SetBackgroundColor(Resources.GetColor(Resource.Color.colorStateInmune));
                    textAccess.Text = Context.GetString(Resource.String.access_allowed);
                    break;

                case "Rojo":
                    imageView.Alpha = 1f;
                    llStateView.SetBackgroundColor(Resources.GetColor(Resource.Color.colorStateSintomas));
                    textQRHelp.Text = Context.GetString(Resource.String.qrcode_help_normal);
                    textAccess.Text = Context.GetString(Resource.String.access_not_allowed);
                    tvTimeOutLabel.Text = "";
                    break;
            }            
        }

        public string GetString(string text)
        {
            int id = Activity.Resources.GetIdentifier(text, "string", Activity.PackageName);
            if (id > 0)
                text = Resources.GetString(id);
            return text;
        }
    }
}