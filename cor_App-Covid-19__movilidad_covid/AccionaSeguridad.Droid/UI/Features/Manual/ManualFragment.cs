using Android.Views;
using Droid.UI;
using AccionaSeguridad.Presentation.UI.Features.Manual;
using Android.Widget;
using AccionaSeguridad.Droid.UI.Features.ManualResult;
using Android.Support.V4.App;
using Acciona.Domain.Model.Security;

namespace AccionaSeguridad.Droid.UI.Features.Manual
{
    public class ManualFragment : BaseFragment<ManualPresenter>, ManualUI
    {

        private EditText editDocument;
        private EditText editPhone;
        private View buttonNext;

        public static ManualFragment NewInstance()
        {
            return new ManualFragment();
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override int GetFragmentLayout()
        {
            return Resource.Layout.manual_fragment;
        }

        protected override void AssingViews(View view)
        {
            view.FindViewById(Resource.Id.back).Click += (o, e) => presenter.BackClicked(); 
            editDocument = view.FindViewById<EditText>(Resource.Id.etDocument);
            editPhone = view.FindViewById<EditText>(Resource.Id.etPhone);
            buttonNext = view.FindViewById(Resource.Id.next);
            buttonNext.Click += (o, e) => presenter.NextClicked(editDocument.Text, editPhone.Text);

            
        }

        public void ShowResult(UserPaper user)
        {
            FragmentTransaction transcation = FragmentManager.BeginTransaction();
            var dialog = new ManualResultDialogFragment();
            dialog.SetUser(user);
            dialog.Cancelable = false;            
            dialog.Show(transcation, "ManualResult");
        }
    }
}