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
using Acciona.Presentation.UI.Features.Config;
using Acciona.Domain.Model.Employee;

namespace AccionaSeguridad.Droid.UI.Features.Config
{
    public class ConfigFragment : BaseFragment<ConfigPresenter>, ConfigUI
    {
        private View buttonBack;
        private TextView tvVersion;
        private View buttonLanguage;
        private View buttonCenter;

        internal static ConfigFragment NewInstance()
        {
            var fragment = new ConfigFragment();            
            return fragment;
        }
        

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override int GetFragmentLayout()
        {
            return Resource.Layout.config_fragment;
        }

        protected override void AssingViews(View view)
        {
            buttonBack = view.FindViewById(Resource.Id.buttonBack);
            buttonBack.Click += (o, e) => presenter.BackClicked();
            buttonLanguage = view.FindViewById(Resource.Id.buttonLanguage);
            buttonLanguage.Click += (o, e) => presenter.LanguageClicked();
            buttonCenter = view.FindViewById(Resource.Id.buttonCenter);
            buttonCenter.Click += (o, e) => presenter.CenterClicked();

            tvVersion = view.FindViewById<TextView>(Resource.Id.version);
            tvVersion.Text = String.Format(Context.GetString(Resource.String.version), Context.PackageManager.GetPackageInfo(Context.PackageName, 0).VersionName);
        }        
    }
}