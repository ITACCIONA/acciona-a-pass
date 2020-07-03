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
using Acciona.Presentation.UI.Features.DataContact;
using Acciona.Domain.Model.Employee;

namespace Acciona.Droid.UI.Features.ContactData
{
    public class ContactDataFragment : BaseFragment<ContactDataPresenter>, ContactDataUI
    {
        private View buttonBack;
        private EditText etUserMail, etUserPhone;
        private Ficha ficha;
        private View buttonSave;

        internal static ContactDataFragment NewInstance(Ficha ficha)
        {
            var fragment = new ContactDataFragment();
            fragment.ficha = ficha;
            return fragment;
        }
        

        protected override void AssingPresenterView()
        {
            presenter.View = this;
            presenter.ficha = ficha;
        }

        protected override int GetFragmentLayout()
        {
            return Resource.Layout.contact_data_fragment;
        }

        protected override void AssingViews(View view)
        {
            etUserMail = view.FindViewById<EditText>(Resource.Id.etUserMail);
            etUserPhone = view.FindViewById<EditText>(Resource.Id.etUserPhone);

            
            buttonBack = view.FindViewById(Resource.Id.buttonBack);
            buttonBack.Click += (o, e) => presenter.BackClicked();

            buttonSave = view.FindViewById(Resource.Id.buttonSave);
            buttonSave.Click += (o, e) => presenter.SaveClicked(etUserPhone.Text);

        }

        public void setProfileData(Ficha ficha)
        {
            etUserMail.Text = ficha.MailEmpleado;
            etUserPhone.Text = ficha.TelefonoEmpleado;
        }
    }
}