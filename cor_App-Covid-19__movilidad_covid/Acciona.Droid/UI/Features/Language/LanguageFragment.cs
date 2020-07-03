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
using Acciona.Presentation.UI.Features.Language;
using Acciona.Domain.Model.Employee;

namespace Acciona.Droid.UI.Features.Language
{
    public class LanguageFragment : BaseFragment<LanguagePresenter>, LanguageUI
    {
        private View buttonBack;        
        private View buttonModify;

        private View buttonSpanish;
        private View buttonEnglish;
        private ImageView checkSpanish;
        private ImageView checkEnglish;

        internal static LanguageFragment NewInstance()
        {
            var fragment = new LanguageFragment();            
            return fragment;
        }
        

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override int GetFragmentLayout()
        {
            return Resource.Layout.language_fragment;
        }

        protected override void AssingViews(View view)
        {
            buttonBack = view.FindViewById(Resource.Id.buttonBack);
            buttonBack.Click += (o, e) => presenter.BackClicked();

            buttonModify = view.FindViewById(Resource.Id.buttonModify);
            buttonModify.Click += (o, e) => presenter.ModifyClicked();

            buttonSpanish = view.FindViewById(Resource.Id.spanish);
            buttonEnglish = view.FindViewById(Resource.Id.english);
            buttonSpanish.Click += ButtonLang_Click;
            buttonEnglish.Click += ButtonLang_Click;

            checkSpanish = view.FindViewById<ImageView>(Resource.Id.check_spanish);
            checkEnglish = view.FindViewById<ImageView>(Resource.Id.check_english);
        }

        private void ButtonLang_Click(object sender, EventArgs e)
        {
            if (sender == buttonSpanish)
                presenter.LangClicked("es");
            if (sender == buttonEnglish)
                presenter.LangClicked("en");
        }

        public void SetConfiguredLang(string configuredLang)
        {
            buttonSpanish.SetBackgroundResource(Resource.Drawable.button_lang);
            buttonEnglish.SetBackgroundResource(Resource.Drawable.button_lang);
            checkSpanish.Visibility = ViewStates.Gone;
            checkEnglish.Visibility = ViewStates.Gone;
            if (configuredLang == "es")
            {
                buttonSpanish.SetBackgroundResource(Resource.Drawable.button_lang_pressed);
                checkSpanish.Visibility = ViewStates.Visible;
            }
            if (configuredLang == "en")
            {
                buttonEnglish.SetBackgroundResource(Resource.Drawable.button_lang_pressed);
                checkEnglish.Visibility = ViewStates.Visible;
            }
        }
    }
}