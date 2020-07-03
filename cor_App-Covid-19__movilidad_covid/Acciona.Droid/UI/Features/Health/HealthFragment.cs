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
using Acciona.Presentation.UI.Features.Health;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Text;

namespace Acciona.Droid.UI.Features.Health
{
    public class HealthFragment : BaseFragment<HealthPresenter>, HealthUI
    {
        private View buttonBack;
        private TextView tvSymptom1, tvSymptom2, tvSymptom3, tvSymptom4, btNextStep, btYes, btNo,tvSymptom2Help1,tvSymptom2Help2;
        private LinearLayout llFirstStepView, llSecondStepView;
        private ImageView ivSymptom1, ivSymptom2, ivSymptom3, ivSymptom4;
        private CardView cvSymptom1, cvSymptom2;
        private bool fiebre;
        private bool sintomas;
        private TextView textContactSubtitle;

        internal static HealthFragment NewInstance()
        {
            return new HealthFragment();
        }
        

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override int GetFragmentLayout()
        {
            return Resource.Layout.health_fragment;
        }

        protected override void AssingViews(View view)
        {
            buttonBack = view.FindViewById(Resource.Id.buttonBack);

            llFirstStepView = view.FindViewById<LinearLayout>(Resource.Id.llHealthStepOne);
            llSecondStepView = view.FindViewById<LinearLayout>(Resource.Id.llHealthStepTwo);

            tvSymptom1 = llFirstStepView.FindViewById<LinearLayout>(Resource.Id.cvSymptom1).FindViewById<TextView>(Resource.Id.tvSymptomDescription);
            tvSymptom2 = llFirstStepView.FindViewById<LinearLayout>(Resource.Id.cvSymptom2).FindViewById<TextView>(Resource.Id.tvSymptomDescription);
            tvSymptom3 = llFirstStepView.FindViewById<LinearLayout>(Resource.Id.cvSymptom3).FindViewById<TextView>(Resource.Id.tvSymptomDescription);
            tvSymptom4 = llFirstStepView.FindViewById<LinearLayout>(Resource.Id.cvSymptom4).FindViewById<TextView>(Resource.Id.tvSymptomDescription);

            ivSymptom1 = llFirstStepView.FindViewById<LinearLayout>(Resource.Id.cvSymptom1).FindViewById<ImageView>(Resource.Id.ivSymptomIcon);
            ivSymptom2 = llFirstStepView.FindViewById<LinearLayout>(Resource.Id.cvSymptom2).FindViewById<ImageView>(Resource.Id.ivSymptomIcon);
            ivSymptom3 = llFirstStepView.FindViewById<LinearLayout>(Resource.Id.cvSymptom3).FindViewById<ImageView>(Resource.Id.ivSymptomIcon);
            ivSymptom4 = llFirstStepView.FindViewById<LinearLayout>(Resource.Id.cvSymptom4).FindViewById<ImageView>(Resource.Id.ivSymptomIcon);

            
            cvSymptom1 = view.FindViewById<CardView>(Resource.Id.cvSymptomOne);
            cvSymptom2 = view.FindViewById<CardView>(Resource.Id.cvSymptomTwo);
            
            cvSymptom1.Click += IvSymptom_Click;
            cvSymptom2.Click += IvSymptom_Click;

            cvSymptom1.SetBackgroundResource(Resource.Drawable.health_symptoms_normal);
            cvSymptom2.SetBackgroundResource(Resource.Drawable.health_symptoms_normal);

            btNo = llSecondStepView.FindViewById<TextView>(Resource.Id.btNo);
            btYes = llSecondStepView.FindViewById<TextView>(Resource.Id.btYes);

            btNextStep = llFirstStepView.FindViewById<TextView>(Resource.Id.btNextStep);

            buttonBack.Click += (o, e) => presenter.BackClicked();
            
            btNo.Click += (o, e) => presenter.ButtonMeetClicked(false);
            btYes.Click += (o, e) => presenter.ButtonMeetClicked(true);
            btNextStep.Click += (o, e) => presenter.ButtonNoSymptomClicked(fiebre,sintomas);


            tvSymptom1.Text = GetString(Resource.String.health_symptom_1);
            tvSymptom2.Text = GetString(Resource.String.health_symptom_2);
            tvSymptom3.Text = GetString(Resource.String.health_symptom_3);
            tvSymptom4.Text = GetString(Resource.String.health_symptom_4);

            ivSymptom1.SetImageResource(Resource.Drawable.thermometer);
            ivSymptom2.SetImageResource(Resource.Drawable.dolor);
            ivSymptom3.SetImageResource(Resource.Drawable.respiracion);
            ivSymptom4.SetImageResource(Resource.Drawable.tos);

            tvSymptom2Help1 = llSecondStepView.FindViewById<TextView>(Resource.Id.help1);
            tvSymptom2Help2 = llSecondStepView.FindViewById<TextView>(Resource.Id.help2);

            textContactSubtitle = llSecondStepView.FindViewById<TextView>(Resource.Id.subtitle);
            ISpanned html;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                html = Html.FromHtml(GetString(Resource.String.health_step_two_description_subtitle), FromHtmlOptions.ModeLegacy);
            else
                html = Html.FromHtml(GetString(Resource.String.health_step_two_description_subtitle));
            textContactSubtitle.SetText(html, TextView.BufferType.Spannable);
            
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                html = Html.FromHtml(GetString(Resource.String.health_step_two_help1), FromHtmlOptions.ModeLegacy);
            else
                html = Html.FromHtml(GetString(Resource.String.health_step_two_help1));
            tvSymptom2Help1.SetText(html, TextView.BufferType.Spannable);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                html = Html.FromHtml(GetString(Resource.String.health_step_two_help2), FromHtmlOptions.ModeLegacy);
            else
                html = Html.FromHtml(GetString(Resource.String.health_step_two_help2));
            tvSymptom2Help2.SetText(html, TextView.BufferType.Spannable);
        }

        private void IvSymptom_Click(object sender, EventArgs e)
        {
            if (sender == cvSymptom1)
            {
                if (fiebre)
                {
                    fiebre = false;
                    cvSymptom1.SetBackgroundResource(Resource.Drawable.health_symptoms_normal);
                    //cvSymptom1.SetBackgroundColor(Color.White);
                }
                else
                {
                    fiebre = true;
                    cvSymptom1.SetBackgroundResource(Resource.Drawable.health_symtomps_selected);
                }
            }
            else if(sender == cvSymptom2)
            {
                if (sintomas)
                {
                    sintomas = false;
                    cvSymptom2.SetBackgroundResource(Resource.Drawable.health_symptoms_normal);
                }
                else
                {
                    sintomas = true;
                    cvSymptom2.SetBackgroundResource(Resource.Drawable.health_symtomps_selected);
                }
            }
            
            setButtonRed(fiebre || sintomas);
        }

        public void ShowFirstStep()
        {
            llFirstStepView.Visibility = ViewStates.Visible;
            llSecondStepView.Visibility = ViewStates.Gone;
        }

        public void ShowSecondStep()
        {
            llFirstStepView.Visibility = ViewStates.Gone;
            llSecondStepView.Visibility = ViewStates.Visible;
        }
        
        private void setButtonRed(bool active)
        {
            if (active)
            {
                btNextStep.SetBackgroundResource(Resource.Drawable.button_red);
                btNextStep.SetTextColor(new Android.Graphics.Color(ContextCompat.GetColor(Context, Resource.Color.colorPrimary)));
                btNextStep.Text = Context.GetString(Resource.String.health_button_first_yes_symptoms);
            }
            else
            {
                btNextStep.SetBackgroundResource(Resource.Drawable.button_white);
                btNextStep.SetTextColor(new Android.Graphics.Color(ContextCompat.GetColor(Context, Resource.Color.colorBlackTwo)));
                btNextStep.Text = Context.GetString(Resource.String.health_button_first_no_symptoms);

            }
        }

        public string GetString(string text)
        {
            int id = Activity.Resources.GetIdentifier(text, "string", Activity.PackageName);
            if (id > 0)
                text = Resources.GetString(id);
            return text;
        }
    }
}