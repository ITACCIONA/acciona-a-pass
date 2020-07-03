using System;
using System.Collections.Generic;
using Acciona.Presentation.UI.Features.Web;
using BaseIOS.UI;
using Foundation;
using UIKit;
using WebKit;

namespace Acciona.iOS.UI.Features.Web
{
    public partial class WebViewController : BaseViewController<WebPresenter>,WebUI
    {
        private string url;

        public event EventHandler ErrorEvent; //Only auth
        public event EventHandler<IDictionary<string, string>> OkEvent; //only auth


        public WebViewController(string url) : base("WebViewController", null)
        {
            this.url = url;
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
            presenter.SetUrl(url);
        }

        protected override void AssingViews()
        {
            buttonBack.TouchUpInside += (o, e) =>
            {
                if (WebviewContainer.CanGoBack)
                    WebviewContainer.GoBack();
                else
                    presenter.CloseClicked();
            };
            var websiteDataTypes = new NSSet<NSString>(new[]
            {
                //Choose which ones you want to remove
                WKWebsiteDataType.Cookies,
                //WKWebsiteDataType.DiskCache,
                //WKWebsiteDataType.IndexedDBDatabases,
                //WKWebsiteDataType.LocalStorage,
                //WKWebsiteDataType.MemoryCache,
                //WKWebsiteDataType.OfflineWebApplicationCache,
                WKWebsiteDataType.SessionStorage,
                //WKWebsiteDataType.WebSQLDatabases
            });

            WKWebsiteDataStore.DefaultDataStore.FetchDataRecordsOfTypes(websiteDataTypes, (NSArray records) =>
            {
                for (nuint i = 0; i < records.Count; i++)
                {
                    var record = records.GetItem<WKWebsiteDataRecord>(i);

                    WKWebsiteDataStore.DefaultDataStore.RemoveDataOfTypes(record.DataTypes,
                        new[] { record }, () => { Console.Write($"deleted: {record.DisplayName}"); });
                }
            });
            WebviewContainer.NavigationDelegate = new MyWKNavigationDelegate(this);

        }

        public class MyWKNavigationDelegate : WKNavigationDelegate
        {
            private WebViewController controller;

            public MyWKNavigationDelegate(WebViewController controller)
            {
                this.controller = controller;
            }

            [Export("webView:decidePolicyForNavigationAction:decisionHandler:")]
            public override void DecidePolicy(WKWebView webView, WKNavigationAction
                        navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
            {
                controller.presenter.NavigateTo(navigationAction.Request.Url.AbsoluteString);
                decisionHandler(WKNavigationActionPolicy.Allow);
            }
        }

        public void Close()
        {
            this.DismissViewController(true, null);
        }

        public void OpenUrl(string url)
        {
            WebviewContainer.LoadRequest(new NSUrlRequest(new NSUrl(url)));
        }

        public void OnError()
        {
            ErrorEvent?.Invoke(this, null);
        }

        public void OnOk(IDictionary<string, string> values)
        {
            OkEvent?.Invoke(this, values);
        }
    }
}

