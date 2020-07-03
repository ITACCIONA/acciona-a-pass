using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Acciona.Domain.Model;
using AccionaSeguridad.Droid.Utils;
using AccionaSeguridad.Presentation.UI.Features.Main;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Droid.UI;
using Droid.Utils;
using Java.IO;
using Newtonsoft.Json;
using Java.Security;
using ZXing.Mobile;
using Acciona.Domain.Model.Security;

namespace AccionaSeguridad.Droid.UI.Features.Main
{
    public class MainFragment : BaseFragment<MainPresenter>, MainUI
    {
        private ZXingScannerFragment scannerFragment;
        private TextView textCenter;

        internal static MainFragment NewInstance()
        {
            return new MainFragment();
        }

        protected override int GetFragmentLayout()
        {
            return Resource.Layout.main_fragment;
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override void AssingViews(View view)
        {
            view.FindViewById(Resource.Id.touch).Click += (o,e)=>presenter.ScreenTouched();
            view.FindViewById(Resource.Id.config).Click += (o, e) => presenter.ConfigClicked();
            view.FindViewById(Resource.Id.manual).Click += (o, e) => presenter.ManualClicked();
            //view.SetOnTouchListener(new ScreenTouchListener(() => presenter.ScreenTouched()));
            textCenter = view.FindViewById<TextView>(Resource.Id.center);
            textCenter.Click += (o, e) => presenter.CenterClicked();
        }

        public async void ShowQRScanner()
        {
            var options = new ZXing.Mobile.MobileBarcodeScanningOptions();
            options.PossibleFormats = new List<ZXing.BarcodeFormat>() {
                ZXing.BarcodeFormat.QR_CODE
            };
            ShowLoading();                        
            var result = await Scan(options);
            HideLoading();
            presenter.SetSacnnerResult(result?.Text);
        }

        public Task<ZXing.Result> Scan(MobileBarcodeScanningOptions options)
        {            

            var task = Task.Factory.StartNew(() =>
            {

                var waitScanResetEvent = new System.Threading.ManualResetEvent(false);

                var scanIntent = new Intent(Context, typeof(PortraitZxingActivity));

                scanIntent.AddFlags(ActivityFlags.NewTask);

                ZxingActivity.UseCustomOverlayView = true;
                ZxingActivity.CustomOverlayView = View.Inflate(Context, Resource.Layout.overlay_scan_view, null);
                ZxingActivity.ScanningOptions = options;
                ZxingActivity.ScanContinuously = false;
                ZxingActivity.TopText = "";
                ZxingActivity.BottomText = "";

                ZXing.Result scanResult = null;

                ZxingActivity.CanceledHandler = () => waitScanResetEvent.Set();

                ZxingActivity.ScanCompletedHandler = (ZXing.Result result) =>
                {
                    scanResult = result;
                    waitScanResetEvent.Set();
                };

                Context.StartActivity(scanIntent);

                waitScanResetEvent.WaitOne();

                return scanResult;
            });

            return task;
        }

        public void SetSelectedLocation(Location location)
        {
            textCenter.Text = location.Name;
        }
    }
}