using Android.Views;
using Android.Widget;
using Droid.UI;
using Acciona.Presentation.UI.Features.MedicalInfo;
using Acciona.Domain.Model.Employee;
using Android.Support.V4.Content;

namespace Acciona.Droid.UI.Features.MedicalInfo
{
    public class MedicalInfoFragment : BaseFragment<MedicalInfoPresenter>, MedicalInfoUI
    {
        private View buttonBack;
        private LinearLayout llMedicalTestFirstStep, llMedicalInfo;

        private TextView btFirstStepYes,
            btFirstStepNo,
            btFirstStepSkipQuestion,
            tvRiskConclusion,
            tvCovidDiagnosedConclusion,
            tvDangerWorkConclusion,
            btConfirm;

        private Ficha ficha;

        internal static MedicalInfoFragment NewInstance(Ficha ficha)
        {
            var fragment = new MedicalInfoFragment();
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
            return Resource.Layout.medical_info_fragment;
        }

        protected override void AssingViews(View view)
        {
            llMedicalTestFirstStep = view.FindViewById<LinearLayout>(Resource.Id.llMedicalTestOne);
            llMedicalInfo = view.FindViewById<LinearLayout>(Resource.Id.llMedicalTestConfirmation);


            llMedicalInfo.FindViewById<TextView>(Resource.Id.tvDescription).Visibility = ViewStates.Gone;

            llMedicalInfo.FindViewById<TextView>(Resource.Id.tvDescriptionTwo).Text =
                Resources.GetString(Resource.String.medical_info_description_two_text);
            
            llMedicalInfo.FindViewById<ImageView>(Resource.Id.ivRiskNext).Visibility =
                ViewStates.Visible;

            llMedicalInfo.FindViewById<ImageView>(Resource.Id.ivPreviousDiagnosedCovidNext).Visibility =
                ViewStates.Invisible;

            llMedicalInfo.FindViewById<ImageView>(Resource.Id.ivDangerWorkNext).Visibility = ViewStates.Invisible;

            llMedicalInfo.FindViewById<LinearLayout>(Resource.Id.llStepBars).Visibility = ViewStates.Gone;

            llMedicalTestFirstStep.FindViewById<LinearLayout>(Resource.Id.llStepBars).Visibility = ViewStates.Gone;


            //Si pulsamos sobre colectivo de riesgo nos muestra la pantalla correspondiente para poder modificarlo
            llMedicalInfo.FindViewById<LinearLayout>(Resource.Id.llModifyRiskConclusion).Click +=
                (o, e) => presenter.ModifyRiskClicked();


            tvRiskConclusion = llMedicalInfo.FindViewById<TextView>(Resource.Id.tvRiskConclusion);
            tvCovidDiagnosedConclusion = llMedicalInfo.FindViewById<TextView>(Resource.Id.tvCovidDiagnosedConclusion);
            tvDangerWorkConclusion = llMedicalInfo.FindViewById<TextView>(Resource.Id.tvDangerWorkConclusion);


            btFirstStepYes = view.FindViewById<LinearLayout>(Resource.Id.llMedicalTestOne)
                .FindViewById<TextView>(Resource.Id.btYes);
            btFirstStepYes.Click += (o, e) => presenter.QuestionFirstStepClicked(true);

            btFirstStepNo = view.FindViewById<LinearLayout>(Resource.Id.llMedicalTestOne)
                .FindViewById<TextView>(Resource.Id.btNo);
            btFirstStepNo.Click += (o, e) => presenter.QuestionFirstStepClicked(false);

            btFirstStepSkipQuestion = view.FindViewById<LinearLayout>(Resource.Id.llMedicalTestOne)
                .FindViewById<TextView>(Resource.Id.btSkipQuestion);
            btFirstStepSkipQuestion.Click += (o, e) => presenter.QuestionFirstStepClicked(null);


            buttonBack = view.FindViewById(Resource.Id.buttonBack);
            buttonBack.Click += (o, e) => presenter.BackClicked();

            btConfirm = llMedicalInfo.FindViewById<TextView>(Resource.Id.btConfirm);
            btConfirm.Visibility = ViewStates.Gone;
            /*btConfirm.Text = Context.GetString( Resource.String.medical_info_button_text);
            btConfirm.SetBackgroundResource(Resource.Drawable.button_white);
            btConfirm.SetTextColor(new Android.Graphics.Color(ContextCompat.GetColor(Context, Resource.Color.colorBlackTwo)));
            */
            
            btConfirm.Click += (sender, args) => presenter.ModifyDataClicked();
        }


        public void showRiskModification()
        {
            llMedicalTestFirstStep.Visibility = ViewStates.Visible;
            llMedicalInfo.Visibility = ViewStates.Gone;
        }

        public void ShowMedicalInfoStep()
        {
            llMedicalTestFirstStep.Visibility = ViewStates.Gone;
            llMedicalInfo.Visibility = ViewStates.Visible;
        }

        public void setResponsesValues(bool? responses)
        {
            if (!responses.HasValue)
                tvRiskConclusion.Text = Resources.GetString(Resource.String.conclusion_nsnc);
            else if (responses.Value)
                tvRiskConclusion.Text = Resources.GetString(Resource.String.conclusion_yes);
            else
                tvRiskConclusion.Text = Resources.GetString(Resource.String.conclusion_no);
        }

        public void setResponsesValues(bool?[] responses)
        {
            if (!responses[0].HasValue)
                tvRiskConclusion.Text = Resources.GetString(Resource.String.conclusion_nsnc);
            else if (responses[0].Value)
                tvRiskConclusion.Text = Resources.GetString(Resource.String.conclusion_yes);
            else
                tvRiskConclusion.Text = Resources.GetString(Resource.String.conclusion_no);

            if (!responses[1].HasValue)
                tvCovidDiagnosedConclusion.Text = Resources.GetString(Resource.String.conclusion_nsnc);
            else if (responses[1].Value)
                tvCovidDiagnosedConclusion.Text = Resources.GetString(Resource.String.conclusion_yes);
            else
                tvCovidDiagnosedConclusion.Text = Resources.GetString(Resource.String.conclusion_no);

            if (!responses[2].HasValue)
                tvDangerWorkConclusion.Text = Resources.GetString(Resource.String.conclusion_nsnc);
            else if (responses[2].Value)
                tvDangerWorkConclusion.Text = Resources.GetString(Resource.String.conclusion_yes);
            else
                tvDangerWorkConclusion.Text = Resources.GetString(Resource.String.conclusion_no);
        }
    }
}

