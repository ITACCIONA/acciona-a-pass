using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Droid.UI;
using Acciona.Domain.Model;
using System.Threading.Tasks;
using Droid.Utils;
using Android;
using Android.Support.V4.Content;
using Android.Net;
using Acciona.Presentation.UI.Features.Alerts;
using Acciona.Domain.Model.Employee;
using Android.Support.V7.Widget;
using Android.Text;

namespace Acciona.Droid.UI.Features.Alerts
{
    public class AlertsFragment : BaseFragment<AlertsPresenter>, AlertsUI
    {
        private View buttonBack;
        private TextView textNotRead;
        private TextView textRead;
        private RecyclerView recyclerNotRead;
        private RecyclerView recyclerRead;
        private RecyclerView.LayoutManager layoutManagerNotRead;
        private RecyclerView.LayoutManager layoutManagerRead;
        private AlertsAdapter adapterNotRead;
        private AlertsAdapter adapterRead;

        internal static AlertsFragment NewInstance()
        {
            return new AlertsFragment();
        }
        
        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override int GetFragmentLayout()
        {
            return Resource.Layout.alerts_fragment;
        }

        protected override void AssingViews(View view)
        {
            buttonBack = view.FindViewById(Resource.Id.buttonBack);
            buttonBack.Click += (o, e) => presenter.BackClicked();
            textNotRead = view.FindViewById<TextView>(Resource.Id.textNotRead);
            textRead = view.FindViewById<TextView>(Resource.Id.textRead);

            recyclerNotRead = view.FindViewById<RecyclerView>(Resource.Id.recyclerNotRead);
            recyclerRead = view.FindViewById<RecyclerView>(Resource.Id.recyclerRead);            
            layoutManagerNotRead = new LinearLayoutManager(this.Context);
            layoutManagerRead = new LinearLayoutManager(this.Context);
            recyclerNotRead.SetLayoutManager(layoutManagerNotRead);
            recyclerRead.SetLayoutManager(layoutManagerRead);
            
        }

        public void SetAlerts(IEnumerable<Alert> alerts)
        {
            var notRead = alerts.Where(x => !x.Read);
            ISpanned html;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                html = Html.FromHtml(String.Format(GetString(Resource.String.alerts_not_read), "<b><font color='red'>" + notRead.Count()+"</font></b>"), FromHtmlOptions.ModeLegacy);
            else                
                html = Html.FromHtml(String.Format(GetString(Resource.String.alerts_not_read), "<b><font color='red'>" + notRead.Count() + "</font></b>"));            
            textNotRead.SetText(html, TextView.BufferType.Spannable);
            adapterNotRead = new AlertsAdapter(Context, notRead);
            adapterNotRead.ItemClick += (o, alert) => presenter.OpenAlert(alert);
            recyclerNotRead.SetAdapter(adapterNotRead);
            var read = alerts.Where(x => x.Read);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                html = Html.FromHtml(String.Format(GetString(Resource.String.alerts_read), "<b><font color='red'>" + read.Count() + "</font></b>"), FromHtmlOptions.ModeLegacy);
            else
                html = Html.FromHtml(String.Format(GetString(Resource.String.alerts_read), "<b><font color='red'>" + read.Count() + "</font></b>"));
            textRead.SetText(html, TextView.BufferType.Spannable);
            adapterRead = new AlertsAdapter(Context, read);
            adapterRead.ItemClick += (o, alert) => presenter.OpenAlert(alert);
            recyclerRead.SetAdapter(adapterRead);
        }
    }
}