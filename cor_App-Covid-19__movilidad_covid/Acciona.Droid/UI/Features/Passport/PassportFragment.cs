using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Acciona.Domain.Model;
using Acciona.Droid.Utils;
using Acciona.Presentation.UI.Features.Passport;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Droid.UI;
using Droid.Utils;
using Java.IO;
using Newtonsoft.Json;

namespace Acciona.Droid.UI.Features.Passport
{
    public class PassportFragment : BaseFragment<PassportPresenter>, PassportUI
    {
       
        internal static PassportFragment NewInstance()
        {
            return new PassportFragment();
        }

        protected override int GetFragmentLayout()
        {
            return Resource.Layout.passport_fragment;
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override void AssingViews(View view)
        {
            
        }

    }
}