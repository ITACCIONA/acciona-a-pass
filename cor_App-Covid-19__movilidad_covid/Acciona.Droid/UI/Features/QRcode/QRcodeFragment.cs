using System;
using Acciona.Droid.UI.Features.Web;
using Acciona.Presentation.UI.Features.QRcode;
using Android.OS;
using Android.Support.V4.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Droid.UI;

namespace Acciona.Droid.UI.Features.QRcode
{
    public class QRcodeFragment : BaseFragment<QRcodePresenter>, QRcodeUI
    {
        private ImageView imageView;
        private TextView textUserName, tvTimeOutLabel, buttonRenew,textAccess,textToDo;
        private TextView textQRHelp;
        private LinearLayout llStateView;
        private bool caducado;

        internal static QRcodeFragment NewInstance()
        {
            return new QRcodeFragment();
        }

        protected override int GetFragmentLayout()
        {
            return Resource.Layout.qrcode_fragment;
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override void AssingViews(View view)
        {
            llStateView = view.FindViewById<LinearLayout>(Resource.Id.llStateView);
            imageView = view.FindViewById<ImageView>(Resource.Id.image);
            textUserName = view.FindViewById<TextView>(Resource.Id.tvUserName);
            textAccess = view.FindViewById<TextView>(Resource.Id.tvAccess);
            textQRHelp = view.FindViewById<TextView>(Resource.Id.qrHelp);
            buttonRenew = view.FindViewById<TextView>(Resource.Id.buttonRenew);            
            tvTimeOutLabel = view.FindViewById<TextView>(Resource.Id.tvTimeOutLabel);
            textToDo = view.FindViewById<TextView>(Resource.Id.tvToDo);
            textToDo.Click += (o,e)=>presenter.ToDoClicked();

            ISpanned html;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                html = Html.FromHtml(GetString(Resource.String.qrcode_todo), FromHtmlOptions.ModeLegacy);
            else
                html = Html.FromHtml(GetString(Resource.String.qrcode_todo));
            textToDo.SetText(html, TextView.BufferType.Spannable);            
            buttonRenew.Click += (o, e) => presenter.RenewClick();
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

        public void SetCaducado(bool caducado)
        {
            this.caducado = caducado;            
        }

        public void SetPassportInfo(Domain.Model.Employee.Passport passport, string message,bool offline)
        {
            if(offline)
                textUserName.Text = GetString(Resource.String.offline_name);
            else
                textUserName.Text = passport.NombreEmpleado;
            textAccess.Text = "";
            ISpanned html;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                html = Html.FromHtml(message, FromHtmlOptions.ModeLegacy);
            else                
                html = Html.FromHtml(message);                        
            tvTimeOutLabel.SetText(html, TextView.BufferType.Spannable);

            if (passport.HasMessage)
                textToDo.Visibility = ViewStates.Visible;
            else
                textToDo.Visibility = ViewStates.Gone;

            switch (passport.ColorPasaporte)
            {

                case "Gris":
                    textQRHelp.Text = Context.GetString(Resource.String.qrcode_help_normal);
                    imageView.Alpha = 1f;
                    llStateView.SetBackgroundColor(Resources.GetColor(Resource.Color.colorStateCaducado));
                    caducado = true;
                    break;

                case "Verde" :
                    textQRHelp.Text = Context.GetString(Resource.String.qrcode_help_normal);
                    imageView.Alpha = 1f;
                    llStateView.SetBackgroundColor(Resources.GetColor(Resource.Color.colorStateInmune));
                    textAccess.Text = Context.GetString(Resource.String.access_allowed);
                    break;
                
                case "Rojo" :
                    imageView.Alpha = 1f;
                    llStateView.SetBackgroundColor(Resources.GetColor(Resource.Color.colorStateSintomas));
                    textQRHelp.Text = Context.GetString(Resource.String.qrcode_help_normal);
                    textAccess.Text = Context.GetString(Resource.String.access_not_allowed);
                    tvTimeOutLabel.Text = "";
                    if (caducado)
                    {
                        caducado = false;
                        textQRHelp.Text = Context.GetString(Resource.String.qrcode_help_caducado);
                        buttonRenew.Visibility = ViewStates.Visible;
                    }
                    break;
                
            }
            if (caducado)
            {
                textQRHelp.Text = Context.GetString(Resource.String.qrcode_help_caducado);
                buttonRenew.Visibility = ViewStates.Visible;
                llStateView.SetBackgroundColor(Resources.GetColor(Resource.Color.colorStateCaducado));
                imageView.Alpha = 0.5f;
                textAccess.Text = "";
                textToDo.Visibility = ViewStates.Gone;
            }
            else 
            {
                if(!passport.ColorPasaporte.Equals("Rojo"))
                    buttonRenew.Visibility = ViewStates.Gone;
            }
        }

        public void ShowTodo(string url)
        {            
            var dialogFragment = WebDialogFragment.NewInstance(url);
            dialogFragment.Show(Activity.SupportFragmentManager, "webview");
        }

        public void ShowStateChange()
        {
            FragmentTransaction transcation = FragmentManager.BeginTransaction();
            var dialog = new ChangeStatusDialogFragment();
            dialog.Cancelable = false;
            dialog.ShowTodo += (o, e) => presenter.ToDoClicked();
            dialog.Show(transcation, "ChangeStatusDialog");
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