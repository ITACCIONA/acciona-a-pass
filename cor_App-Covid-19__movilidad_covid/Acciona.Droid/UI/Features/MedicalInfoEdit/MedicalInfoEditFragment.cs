using Android.Views;
using Android.Widget;
using Droid.UI;
using Acciona.Presentation.UI.Features.MedicalInfoEdit;
using Acciona.Domain.Model.Employee;
using Android.Support.V4.Content;

namespace Acciona.Droid.UI.Features.MedicalInfoEdit
{
    public class MedicalInfoEditFragment : BaseFragment<MedicalInfoEditPresenter>, MedicalInfoEditUI
    {
        private View buttonBack;
        private LinearLayout llMedicalTestFirstStep;
        private TextView btFirstStepYes;
        private TextView btFirstStepNo;
        private TextView btSkipQuestionOne;
        private Ficha ficha;

        internal static MedicalInfoEditFragment NewInstance(Ficha ficha)
        {
            var fragment = new MedicalInfoEditFragment();
            fragment.ficha = ficha;
            return fragment;
        }


        protected override void AssingPresenterView()
        {
            presenter.View = this;
            presenter.Ficha = ficha;
        }

        protected override int GetFragmentLayout()
        {
            return Resource.Layout.medical_info_edit_fragment;
        }

        protected override void AssingViews(View view)
        {           
            buttonBack = view.FindViewById(Resource.Id.buttonBack);
            buttonBack.Click += (o, e) => presenter.BackClicked();

            //FirstStepViews
            view.FindViewById(Resource.Id.llStepBars).Visibility = ViewStates.Gone;

            llMedicalTestFirstStep = view.FindViewById<LinearLayout>(Resource.Id.llMedicalTestOne);

            btFirstStepYes = llMedicalTestFirstStep.FindViewById<TextView>(Resource.Id.btYes);
            btFirstStepYes.Click += (o, e) => presenter.QuestionFirstStepClicked(true);

            btFirstStepNo = llMedicalTestFirstStep.FindViewById<TextView>(Resource.Id.btNo);
            btFirstStepNo.Click += (o, e) => presenter.QuestionFirstStepClicked(false);

            btSkipQuestionOne = llMedicalTestFirstStep.FindViewById<TextView>(Resource.Id.btSkipQuestion);
            btSkipQuestionOne.Click += (o, e) => presenter.QuestionFirstStepClicked((bool?)null);


        }

    }
}

