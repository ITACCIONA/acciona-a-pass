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
using Acciona.Presentation.UI.Features.Alarm;

namespace Acciona.Droid.UI.Features.Alarm
{
    public class AlarmFragment : BaseFragment<AlarmPresenter>, AlarmUI
    {
        private View buttonBack;
        private TextView btMailContact, btMail;
        private LinearLayout btEnterpriseSkype;


        internal static AlarmFragment NewInstance()
        {
            return new AlarmFragment();
        }
        

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override int GetFragmentLayout()
        {
            return Resource.Layout.alarm_fragment;
        }

        protected override void AssingViews(View view)
        {
            buttonBack = view.FindViewById(Resource.Id.buttonBack);
            buttonBack.Click += (o, e) => presenter.BackClicked();

            btEnterpriseSkype = view.FindViewById<LinearLayout>(Resource.Id.btEnterpriseSkype);
            btEnterpriseSkype.Click += (o, e) => presenter.OpenSkypeEnterprise();

            btMailContact = view.FindViewById<TextView>(Resource.Id.btMailContact);
            btMailContact.Click += (o, e) => presenter.ContactEmailClicked();

            btMail = view.FindViewById<TextView>(Resource.Id.btMail);
            btMail.Click += (o, e) => presenter.ContactEmailClicked();



            /*buttonCall = view.FindViewById<TextView>(Resource.Id.buttonCall);
            buttonCall.Text = String.Format(GetString(Resource.String.alarm_call_to), GetString(Resource.String.alarm_phone));
            buttonCall.Click += (o, e) => presenter.CallClicked(GetString(Resource.String.alarm_phone));*/
        }       
    }
}