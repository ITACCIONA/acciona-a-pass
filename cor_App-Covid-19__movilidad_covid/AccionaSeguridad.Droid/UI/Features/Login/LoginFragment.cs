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
using AccionaSeguridad.Presentation.UI.Features.Login;
using Acciona.Domain.Model;
using System.Threading.Tasks;
using Droid.Utils;
using Android;
using Android.Support.V4.Content;
using Android.Net;

namespace AccionaSeguridad.Droid.UI.Features.Login
{
    public class LoginFragment : BaseFragment<LoginPresenter>, LoginUI
    {
        private EditText editUser;
        private EditText editPass;

        internal static LoginFragment NewInstance()
        {
            return new LoginFragment();
        }
        

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override void AssingViews(View view)
        {
            editUser = view.FindViewById<EditText>(Resource.Id.editUser);
            editPass = view.FindViewById<EditText>(Resource.Id.editPass);
            view.FindViewById(Resource.Id.buttonLogin).Click += (o, e) => presenter.LoginClicked(editUser.Text, editPass.Text);
        }
       
        protected override int GetFragmentLayout()
        {
            return Resource.Layout.login_fragment;
        }

        public void SetLastUser(string user)
        {
            editUser.Text = user;
        }

        public bool CheckInternet()
        {
            ConnectivityManager cm = (ConnectivityManager)Context.GetSystemService(Context.ConnectivityService);

            NetworkInfo activeNetwork = cm.ActiveNetworkInfo;
            return  activeNetwork != null && activeNetwork.IsConnectedOrConnecting;
        }
    }
}