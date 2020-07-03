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
using Acciona.Presentation.UI.Features.AlertDetail;
using Acciona.Domain.Model.Employee;

namespace Acciona.Droid.UI.Features.AlertDetail
{
    public class AlertDetailFragment : BaseFragment<AlertDetailPresenter>, AlertDetailUI
    {
        private View buttonBack;
        private TextView title;
        private TextView date;
        private TextView description;

        internal static AlertDetailFragment NewInstance()
        {
            return new AlertDetailFragment();
        }
        

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override int GetFragmentLayout()
        {
            return Resource.Layout.alert_detail_fragment;
        }

        protected override void AssingViews(View view)
        {
            buttonBack = view.FindViewById(Resource.Id.buttonBack);
            buttonBack.Click += (o, e) => presenter.BackClicked();

            title = view.FindViewById<TextView>(Resource.Id.title);
            date = view.FindViewById<TextView>(Resource.Id.date);
            description = view.FindViewById<TextView>(Resource.Id.description);
        }

        public void SetAlert(Alert alert)
        {
            title.Text = alert.Title;
            date.Text = alert.FechaNotificacion.ToString(Context.GetString(Resource.String.filter_date_format)) + " - " + alert.FechaNotificacion.ToString("HH:mm");
            description.Text = alert.Comment;
        }
    }
}