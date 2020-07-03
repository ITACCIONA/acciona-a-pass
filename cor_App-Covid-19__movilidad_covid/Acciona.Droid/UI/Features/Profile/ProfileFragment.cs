using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Acciona.Domain.Model;
using Acciona.Domain.Model.Employee;
using Acciona.Droid.Utils;
using Acciona.Presentation.UI.Features.Profile;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Droid.UI;
using Droid.Utils;
using Java.IO;
using Newtonsoft.Json;

namespace Acciona.Droid.UI.Features.Profile
{
    public class ProfileFragment : BaseFragment<ProfilePresenter>, ProfileUI
    {

        private Ficha ficha;
        private TextView tvTitle, tvLabelName, tvVersion,tvCenter;

        internal static ProfileFragment NewInstance()
        {
            return new ProfileFragment();

        }

        protected override int GetFragmentLayout()
        {
            return Resource.Layout.profile_fragment;
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override void AssingViews(View view)
        {
            view.FindViewById(Resource.Id.llContactData).Click += (o, e) => presenter.OpenContactData();
            view.FindViewById(Resource.Id.llMedicalData).Click += (o, e) => presenter.OpenMedicalInfo();
            view.FindViewById(Resource.Id.llCenter).Click += (o, e) => presenter.OpenCenter();
            view.FindViewById(Resource.Id.btLogout).Click += (o, e) => presenter.LogoutClicked();
            view.FindViewById(Resource.Id.language).Click += (o, e) => presenter.LanguageClicked();

            tvTitle = view.FindViewById<TextView>(Resource.Id.tvTitle);
            tvLabelName = view.FindViewById<TextView>(Resource.Id.tvLabelName);
            tvVersion = view.FindViewById<TextView>(Resource.Id.tvVersion);
            tvCenter = view.FindViewById<TextView>(Resource.Id.center);
            tvVersion.Text = String.Format(Context.GetString(Resource.String.version), Context.PackageManager.GetPackageInfo(Context.PackageName, 0).VersionName);
        }

        public void SetFicha(Ficha ficha)
        {
            this.ficha = ficha;

            ISpanned html;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                html = Html.FromHtml(String.Format(GetString(Resource.String.profile_hello),ficha.NombreEmpleado), FromHtmlOptions.ModeLegacy);
            else
                html = Html.FromHtml(String.Format(GetString(Resource.String.profile_hello), ficha.NombreEmpleado));

            tvTitle.SetText(html, TextView.BufferType.Spannable);
            tvLabelName.Text = String.Format(GetString(Resource.String.profile_not_user), ficha.NombreEmpleado);
            if (ficha.IdLocalizacion.HasValue)
                tvCenter.Text = ficha.Localizacion;
            else
                tvCenter.Text = GetString(Resource.String.center_other);
        }

    }
}