using System;
using System.Collections.Generic;
using Acciona.Presentation.UI.Features.Web;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Droid.UI;
using ServiceLocator;

namespace Acciona.Droid.UI.Features.Web
{
    public class WebDialogFragment : BaseDialogFragment<WebPresenter>, WebUI
    {

        private string url;

        public event EventHandler ErrorEvent; //Only auth
        public event EventHandler<IDictionary<string, string>> OkEvent; //only auth

        private ImageView btBack;
        private WebView webview;

        public void SetData(string url)
        {
            this.url = url;
        }

        public static WebDialogFragment NewInstance(string url)
        {
            var fragment = new WebDialogFragment();
            fragment.SetData(url);
            return fragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStyle((int)DialogFragmentStyle.Normal, Resource.Style.FullScreenDialogStyle);

        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
            presenter.SetUrl(url);
        }

        protected override void AssingViews(View view)
        {

            btBack = view.FindViewById<ImageView>(Resource.Id.back);
            webview = view.FindViewById<WebView>(Resource.Id.webview);

            btBack.Click += (o, e) =>
            {
                if (webview.CanGoBack())
                    webview.GoBack();
                else
                    presenter.CloseClicked();
            };

            View.FocusableInTouchMode = true;
            View.RequestFocus();
            View.KeyPress += (o, e) => {
                if (e.KeyCode == Keycode.Back)
                {
                    if (webview.CanGoBack())
                        webview.GoBack();
                    else
                        presenter.CloseClicked();
                    e.Handled = true;
                }
            };

            webview.Settings.JavaScriptEnabled = true;
            webview.Settings.SetAppCacheEnabled(false);
            webview.Settings.CacheMode = CacheModes.NoCache;
            //webview.ClearCache(true);
            //webview.ClearHistory();
            ClearCookies();
            webview.SetWebViewClient(new MyWebViewClient(this));
        }



        public void ClearCookies()
        {

            if ((int)Build.VERSION.SdkInt >= (int)Android.OS.BuildVersionCodes.LollipopMr1)
            {
                CookieManager.Instance.RemoveAllCookies(null);
                CookieManager.Instance.Flush();
            }
            else
            {

                CookieSyncManager cookieSyncMngr = CookieSyncManager.CreateInstance(Context);
                cookieSyncMngr.StartSync();
                CookieManager cookieManager = CookieManager.Instance;
                cookieManager.RemoveAllCookie();
                cookieManager.RemoveSessionCookie();
                cookieSyncMngr.StopSync();
                cookieSyncMngr.Sync();
            }
        }

        protected override int GetFragmentLayout()
        {
            return Resource.Layout.webview_fragment_dialog;
        }

        public void OpenUrl(string url)
        {
            webview.LoadUrl(url);
        }

        public void Close()
        {
            Dismiss();
        }

        public void OnError()
        {
            ErrorEvent?.Invoke(this, null);
        }

        public void OnOk(IDictionary<string, string> values)
        {
            OkEvent?.Invoke(this, values);
        }

        public class MyWebViewClient : WebViewClient
        {
            private WebDialogFragment webDialogFragment;

            public MyWebViewClient(WebDialogFragment webDialogFragment)
            {
                this.webDialogFragment = webDialogFragment;
            }

            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                webDialogFragment.presenter.NavigateTo(url);
                view.LoadUrl(url);
                return false;
            }
        }
    }
}
