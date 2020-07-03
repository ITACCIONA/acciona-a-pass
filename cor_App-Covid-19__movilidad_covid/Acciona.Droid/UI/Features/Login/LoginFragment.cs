using System;
using Android.Content;
using Android.Views;
using Android.Widget;
using Droid.UI;
using Acciona.Presentation.UI.Features.Login;
using System.Threading.Tasks;
using Android.Net;
using Acciona.Droid.Utils;
using Acciona.Droid.UI.Features.Web;

namespace Acciona.Droid.UI.Features.Login
{
    public class LoginFragment : BaseFragment<LoginPresenter>, LoginUI
    {
        /*private EditText editUser;
        private EditText editPass;*/
        private ImageView ivStateLogin;
        private TextView btRetryLogin, tvStateLogin;


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
            /*editUser = view.FindViewById<EditText>(Resource.Id.editUser);
            editPass = view.FindViewById<EditText>(Resource.Id.editPass);*/
            //view.FindViewById(Resource.Id.buttonLogin).Click += (o, e) => GetResult(); presenter.LoginClicked(editUser.Text, editPass.Text);

            ivStateLogin = view.FindViewById<ImageView>(Resource.Id.ivLoginState);
            btRetryLogin = view.FindViewById<TextView>(Resource.Id.btRetryConnect);
            tvStateLogin = view.FindViewById<TextView>(Resource.Id.tvStateLogin);

            btRetryLogin.Click += (o, e) => GetResult(); 
            
            GetResult();
        }



        protected override int GetFragmentLayout()
        {
            return Resource.Layout.login_fragment;
        }

        public void SetLastUser(string user)
        {
           // editUser.Text = user;
        }

        public bool CheckInternet()
        {
            ConnectivityManager cm = (ConnectivityManager)Context.GetSystemService(Context.ConnectivityService);

            NetworkInfo activeNetwork = cm.ActiveNetworkInfo;
            return  activeNetwork != null && activeNetwork.IsConnectedOrConnecting;
        }

        public void showLoadingLogin()
        {
            tvStateLogin.Text = GetString(Resource.String.login_connecting_message);
            ivStateLogin.SetImageResource(Resource.Drawable.img_login_loading);
            btRetryLogin.Visibility = ViewStates.Invisible;
        }

        public void ShowErrorLogin()
        {
            tvStateLogin.Text = GetString(Resource.String.login_error_message);
            ivStateLogin.SetImageResource(Resource.Drawable.img_login_error);
            
            btRetryLogin.Visibility = ViewStates.Visible;
        }

        private async Task GetResult()
        {
            try
            {
                var dialogFragment = WebDialogFragment.NewInstance(null);
                dialogFragment.OkEvent += dialog_OkEvent;
                dialogFragment.ErrorEvent += dialog_ErrorEvent;
                dialogFragment.Show(Activity.SupportFragmentManager, "webview");
            }
            catch (Exception e)
            {
                ShowDialog(e.Message, "msg_ok", null);
            }
        }

        private void dialog_OkEvent(object sender, System.Collections.Generic.IDictionary<string, string> dictionary)
        {
            showLoadingLogin();
            if (dictionary.ContainsKey("token_type") && dictionary.ContainsKey("access_token"))
                presenter.Authorized(dictionary["token_type"] + " " + dictionary["access_token"]);
            else
                ShowErrorLogin();
        }

        private void dialog_ErrorEvent(object sender, EventArgs e)
        {
            ShowErrorLogin();
        }

        public void ShowRetry()
        {
            ShowErrorLogin();
        }
    }
}