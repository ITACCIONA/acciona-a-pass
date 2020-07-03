using Android.Views;
using Droid.UI;
using Acciona.Presentation.UI.Features.Splash;
using Acciona.Droid.Services;
using Android.Support.V4.OS;
using System.Collections.Generic;
using ServiceLocator;
using Acciona.Domain.Model;
using Android.Widget;
using Android.Content.PM;
using Android.Content;
using Acciona.Domain;
using Android.Net;
using System;
using Java.Util;
using Android.Content.Res;
using Android.OS;

namespace Acciona.Droid.UI.Features.Splash
{
    public class SplashFragment : BaseFragment<SplashPresenter>, SplashUI
    {

        public static SplashFragment NewInstance()
        {
            return new SplashFragment();
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override void AssingViews(View view)
        {

        }

        protected override int GetFragmentLayout()
        {
            return Resource.Layout.splash_fragment;
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

        
    }
}