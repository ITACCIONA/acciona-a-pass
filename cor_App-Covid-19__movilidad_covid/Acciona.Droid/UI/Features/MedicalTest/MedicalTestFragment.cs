using Android.Views;
using Android.Widget;
using Droid.UI;
using Acciona.Presentation.UI.Features.MedicalTest;

namespace Acciona.Droid.UI.Features.MedicalTest
{
    public class MedicalTestFragment : BaseFragment<MedicalTestPresenter>, MedicalTestUI
    {
        private View btBack;
        private bool CanBack;

        private TextView btFirstStepYes,
            btFirstStepNo,
            btSecondStepNo,
            btSecondStepYes,
            btThirdStepNo,
            btThirdStepYes,
            btConfirmForm,
            btSkipQuestionOne,
            btSkipQuestionTwo,
            btSkipQuestionThree,
            tvRiskConclusion,
            tvCovidDiagnosedConclusion,
            tvDangerWorkConclusion,
            tvtitle;

        private LinearLayout llMedicalTestFirstStep,
            llMedicalTestSecondStep,
            llMedicalTestThirdStep,
            llMedicalTestConclusionStep,
            llModifyRiskConclusion,
            llModifyPreviousDiagnosedCovid,
            llModifyDangerWork;        

        internal static MedicalTestFragment NewInstance(bool? canback = false)
        {
            var fragment = new MedicalTestFragment();
            fragment.CanBack = canback.Value;
            
            return fragment;
        }


        protected override void AssingPresenterView()
        {
            presenter.View = this;
            presenter.canBack = CanBack;
        }

        protected override int GetFragmentLayout()
        {
            return Resource.Layout.medical_test_fragment;
        }

        protected override void AssingViews(View view)
        {
            llMedicalTestFirstStep = view.FindViewById<LinearLayout>(Resource.Id.llMedicalTestOne);
            llMedicalTestSecondStep = view.FindViewById<LinearLayout>(Resource.Id.llMedicalTestTwo);
            llMedicalTestThirdStep = view.FindViewById<LinearLayout>(Resource.Id.llMedicalTestThree);
            llMedicalTestConclusionStep = view.FindViewById<LinearLayout>(Resource.Id.llMedicalTestConfirmation);

            tvtitle = view.FindViewById<TextView>(Resource.Id.title);
            btBack = view.FindViewById(Resource.Id.buttonBack);
            btBack.Click += (o, e) => presenter.BackClicked();


            //FirstStepViews

            btFirstStepYes = llMedicalTestFirstStep.FindViewById<TextView>(Resource.Id.btYes);
            btFirstStepYes.Click += (o, e) => presenter.QuestionFirstStepClicked(true);

            btFirstStepNo = llMedicalTestFirstStep.FindViewById<TextView>(Resource.Id.btNo);
            btFirstStepNo.Click += (o, e) => presenter.QuestionFirstStepClicked(false);

            btSkipQuestionOne = llMedicalTestFirstStep.FindViewById<TextView>(Resource.Id.btSkipQuestion);
            btSkipQuestionOne.Click += (o, e) => presenter.QuestionFirstStepClicked((bool?) null);

            

            //SecondStepViews
            btSecondStepYes = llMedicalTestSecondStep.FindViewById<TextView>(Resource.Id.btYes);
            btSecondStepYes.Click += (o, e) => presenter.QuestionSecondStepClicked(true);

            btSecondStepNo = llMedicalTestSecondStep.FindViewById<TextView>(Resource.Id.btNo);
            btSecondStepNo.Click += (o, e) => presenter.QuestionSecondStepClicked(false);

            btSkipQuestionTwo = llMedicalTestSecondStep.FindViewById<TextView>(Resource.Id.btSkipQuestion);
            btSkipQuestionTwo.Click += (o, e) => presenter.QuestionSecondStepClicked((bool?) null);
            
            

            //ThirsStepViews
            btThirdStepYes = llMedicalTestThirdStep.FindViewById<TextView>(Resource.Id.btYes);
            btThirdStepYes.Click += (o, e) => presenter.QuestionThirdStepClicked(true);

            btThirdStepNo = llMedicalTestThirdStep.FindViewById<TextView>(Resource.Id.btNo);
            btThirdStepNo.Click += (o, e) => presenter.QuestionThirdStepClicked(false);

            btSkipQuestionThree = llMedicalTestThirdStep.FindViewById<TextView>(Resource.Id.btSkipQuestion);
            btSkipQuestionThree.Click += (o, e) => presenter.QuestionThirdStepClicked((bool?) null);
            
           

            //ConclusionStepViews

            btConfirmForm = llMedicalTestConclusionStep.FindViewById<TextView>(Resource.Id.btConfirm);
            btConfirmForm.Click += (o, e) => presenter.TestFinished();
            tvRiskConclusion = llMedicalTestConclusionStep.FindViewById<TextView>(Resource.Id.tvRiskConclusion);
            tvCovidDiagnosedConclusion =
                llMedicalTestConclusionStep.FindViewById<TextView>(Resource.Id.tvCovidDiagnosedConclusion);
            tvDangerWorkConclusion =
                llMedicalTestConclusionStep.FindViewById<TextView>(Resource.Id.tvDangerWorkConclusion);
            
            
        }

        public void ShowFirstStep()
        {
            ConfigureTitleAndBack(CanBack);
            llMedicalTestFirstStep.Visibility = ViewStates.Visible;
            llMedicalTestSecondStep.Visibility = ViewStates.Gone;
            llMedicalTestThirdStep.Visibility = ViewStates.Gone;
            llMedicalTestConclusionStep.Visibility = ViewStates.Gone;
        }

        public void ShowSecondStep()
        {
            ConfigureTitleAndBack(CanBack);
            btBack.Visibility = ViewStates.Visible;
            llMedicalTestFirstStep.Visibility = ViewStates.Gone;
            llMedicalTestSecondStep.Visibility = ViewStates.Visible;
            llMedicalTestThirdStep.Visibility = ViewStates.Gone;
            llMedicalTestConclusionStep.Visibility = ViewStates.Gone;
        }

        public void ShowThirdStep()
        {
            ConfigureTitleAndBack(CanBack);
            btBack.Visibility = ViewStates.Visible;
            llMedicalTestFirstStep.Visibility = ViewStates.Gone;
            llMedicalTestSecondStep.Visibility = ViewStates.Gone;
            llMedicalTestThirdStep.Visibility = ViewStates.Visible;
            llMedicalTestConclusionStep.Visibility = ViewStates.Gone;
        }

        public void ShowConclusionStep(bool?[] responses)
        {
            tvtitle.Text = Context.GetString(Resource.String.medical_test_title_step_final);

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

            
            llMedicalTestFirstStep.Visibility = ViewStates.Gone;
            llMedicalTestSecondStep.Visibility = ViewStates.Gone;
            llMedicalTestThirdStep.Visibility = ViewStates.Gone;
            llMedicalTestConclusionStep.Visibility = ViewStates.Visible;
        }

        public void ConfigureTitleAndBack(bool canBack)
        {
            if (canBack)
            {
                btBack.Visibility = ViewStates.Visible;
                tvtitle.Text = Context.GetString(Resource.String.medical_test_title_steps_modify);
            }
            else
            {
                btBack.Visibility = ViewStates.Invisible;
                tvtitle.Text = Context.GetString(Resource.String.medical_test_title_steps);
            }
        }
    }
}