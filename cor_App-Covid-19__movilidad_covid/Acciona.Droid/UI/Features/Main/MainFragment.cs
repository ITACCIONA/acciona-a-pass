using System;
using System.Collections.Generic;
using System.Globalization;
using Acciona.Domain;
using Acciona.Droid.UI.Features.Passport;
using Acciona.Droid.UI.Features.Profile;
using Acciona.Droid.UI.Features.QRcode;
using Acciona.Presentation.UI.Features.Main;
using Acciona.Presentation.UI.Features.Splash;
using Android.Content;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Droid.UI;
using Java.Security;
using static Acciona.Presentation.UI.Features.Main.MainPresenter;

namespace Acciona.Droid.UI.Features.Main
{
    public class MainFragment : BaseFragment<MainPresenter>, MainUI
    {
        private View buttonPhone;
        private View buttonBell;
        private View buttonPassport;
        private View buttonAlarm;
        private View buttonProfile;
        private TextView textPassport;
        private TextView textQRcode;
        private TextView textProfile;

        private Fragment actualFragment;
        private MainState state;

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
            buttonPhone = view.FindViewById(Resource.Id.buttonPhone);
            buttonPhone.Click += (o, e) => presenter.PhoneClicked();
            buttonBell = view.FindViewById(Resource.Id.buttonBell);
            buttonBell.Click += (o, e) => presenter.BellClicked();
            buttonPassport = view.FindViewById(Resource.Id.framePassport);
            buttonPassport.Click += MainButton_Click;
            buttonAlarm = view.FindViewById(Resource.Id.frameQrcode);
            buttonAlarm.Click += MainButton_Click;
            buttonProfile = view.FindViewById(Resource.Id.frameProfile);
            buttonProfile.Click += MainButton_Click;
            textPassport = view.FindViewById<TextView>(Resource.Id.passport);
            textQRcode = view.FindViewById<TextView>(Resource.Id.qrcode);
            textProfile = view.FindViewById<TextView>(Resource.Id.profile);
        }

        private void MainButton_Click(object sender, EventArgs e)
        {
            if (sender.Equals(buttonPassport))
            {
                presenter.PassportClicked();
            }
            else if (sender.Equals(buttonAlarm))
            {
                presenter.AlarmClicked();
            }
            else if (sender.Equals(buttonProfile))
            {
                presenter.ProfileClicked();
            }
        }

        public void ShowState(MainState state)
        {
            this.state = state;
            if (actualFragment != null)
            {
                var removeTransaction = ChildFragmentManager.BeginTransaction();
                removeTransaction.Remove(actualFragment);
                removeTransaction.Commit();
            }
            switch (state)
            {
                case MainState.PASSPORT:
                    actualFragment = QRcodeFragment.NewInstance();
                    break;
                case MainState.PROFILE:
                    actualFragment = ProfileFragment.NewInstance();
                    break;                
            }
            var transaction = ChildFragmentManager.BeginTransaction();            
            transaction.Add(Resource.Id.content, actualFragment);
            transaction.Commit();
        }

        public void ShowNotificationsNotRead(bool notRead)
        {
            buttonBell.SetBackgroundResource(notRead ? Resource.Drawable.notifications : Resource.Drawable.bell);
        }

        public SplashPresenter.Platform GetPlatform()
        {
            return SplashPresenter.Platform.Android;
        }

        public int GetVersion()
        {
            return Context.PackageManager.GetPackageInfo(Context.PackageName, 0).VersionCode;
        }

        public void DownloadApp()
        {
            Intent share = new Intent(Android.Content.Intent.ActionView);
            share.SetData(Android.Net.Uri.Parse(DomainConstants.androidURL));
            Activity.StartActivity(Intent.CreateChooser(share, "A-Pass"));
        }

        /*public override void OnResume()
        {
            base.OnResume();
            string redirect = GetRedirectUriForBroker();
            Toast.MakeText(Context, redirect, ToastLength.Long).Show();
        }

        private string GetRedirectUriForBroker()
        {
            var RedirectUriScheme = "msauth";
            string packageName = Android.App.Application.Context.PackageName;
            string signatureDigest = this.GetCurrentSignatureForPackage(packageName);
            if (!string.IsNullOrEmpty(signatureDigest))
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}://{1}/{2}", RedirectUriScheme,
                   packageName.ToLowerInvariant(), signatureDigest);
            }

            return string.Empty;
        }

        private string GetCurrentSignatureForPackage(string packageName)
        {
            PackageInfo info = Android.App.Application.Context.PackageManager.GetPackageInfo(packageName,
               PackageInfoFlags.Signatures);
            if (info != null && info.Signatures != null && info.Signatures.Count > 0)
            {
                // First available signature. Applications can be signed with multiple signatures.
                // The order of Signatures is not guaranteed.
                Android.Content.PM.Signature signature = info.Signatures[0];
                MessageDigest md = MessageDigest.GetInstance("SHA");
                md.Update(signature.ToByteArray());
                return Convert.ToBase64String(md.Digest(), Base64FormattingOptions.None);
                // Server side needs to register all other tags. ADAL will
                // send one of them.
            }
            return null;
        }*/
    }
}