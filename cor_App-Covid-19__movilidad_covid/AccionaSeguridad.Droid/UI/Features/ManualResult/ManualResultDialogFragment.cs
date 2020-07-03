using Android.Views;
using Droid.UI;
using AccionaSeguridad.Presentation.UI.Features.ManualResult;
using Android.OS;
using Android.Widget;
using Acciona.Domain.Model.Security;
using System;
using Android.Text;

namespace AccionaSeguridad.Droid.UI.Features.ManualResult
{
    public class ManualResultDialogFragment : BaseDialogFragment<ManualResultPresenter>, ManualResultUI
    {

        private TextView textName,textFiebre,textSintomas,textContacto,textHelp1,textHelp2;
        private UserPaper user;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStyle((int)Android.App.DialogFragmentStyle.Normal, Resource.Style.FullScreenDialogStyle);
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
            presenter.SetUser(user);
        }

        protected override int GetFragmentLayout()
        {
            return Resource.Layout.manual_result_dialog_fragment;
        }

        protected override void AssingViews(View view)
        {
            view.FindViewById(Resource.Id.back).Click += (o, e) => presenter.BackClicked();
            view.FindViewById(Resource.Id.buttonNo).Click += (o, e) => presenter.NoClicked();
            view.FindViewById(Resource.Id.buttonYes).Click += (o, e) => presenter.YesClicked();
            textName = view.FindViewById<TextView>(Resource.Id.name);
            textFiebre = view.FindViewById<TextView>(Resource.Id.fiebre);
            textSintomas = view.FindViewById<TextView>(Resource.Id.sintomas);
            textContacto = view.FindViewById<TextView>(Resource.Id.contacto);
            textHelp1 = view.FindViewById<TextView>(Resource.Id.help_contacto);
            textHelp2 = view.FindViewById<TextView>(Resource.Id.help_contacto2);

            FillHTML(textFiebre, Resource.String.result_temperature);
            FillHTML(textSintomas, Resource.String.result_sympthoms);
            FillHTML(textContacto, Resource.String.result_contact);
            FillHTML(textHelp1, Resource.String.help_contact_1);
            FillHTML(textHelp2, Resource.String.help_contact_2);
        }

        private void FillHTML(TextView tv,int resource)
        {
            ISpanned html;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                html = Html.FromHtml(GetString(resource), FromHtmlOptions.ModeLegacy);
            else
                html = Html.FromHtml(GetString(resource));
            tv.SetText(html, TextView.BufferType.Spannable);
        }

        public void Close()
        {
            Dismiss();
        }

        internal void SetUser(UserPaper user)
        {
            this.user = user;
        }

        public void SetName(string name)
        {
            textName.Text = name;
        }
    }
}