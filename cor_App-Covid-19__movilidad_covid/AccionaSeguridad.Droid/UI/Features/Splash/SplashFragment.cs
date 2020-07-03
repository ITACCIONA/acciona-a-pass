using Android.Views;
using Droid.UI;
using AccionaSeguridad.Presentation.UI.Features.Splash;
using Android.Support.V4.OS;
using System.Collections.Generic;
using ServiceLocator;
using Acciona.Domain.Model;
using Android.Widget;
using Android.Content.PM;
using Acciona.Domain;
using Android.Content;
using Android.Net;

namespace AccionaSeguridad.Droid.UI.Features.Splash
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
            return Resource.Layout.splash_view_fragment;
        }

        public int GetVersion()
        {
            return Context.PackageManager.GetPackageInfo(Context.PackageName, 0).VersionCode;
        }

        public void Download()
        {
            Intent share = new Intent(Android.Content.Intent.ActionView);
            share.SetData(Uri.Parse(DomainConstants.seguridadURL));
            Activity.StartActivity(Intent.CreateChooser(share, "A-Pass"));
        }
    }
}