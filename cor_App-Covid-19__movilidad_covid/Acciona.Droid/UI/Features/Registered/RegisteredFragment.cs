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
using Acciona.Presentation.UI.Features.Registered;

namespace Acciona.Droid.UI.Features.Registered
{
    public class RegisteredFragment : BaseFragment<RegisteredPresenter>, RegisteredUI
    {
        private View buttonBack;

        internal static RegisteredFragment NewInstance()
        {
            return new RegisteredFragment();
        }
        

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override int GetFragmentLayout()
        {
            return Resource.Layout.registered_fragment;
        }

        protected override void AssingViews(View view)
        {
            view.FindViewById(Resource.Id.button).Click += (o, e) => presenter.ContinueClicked();
        }       
    }
}