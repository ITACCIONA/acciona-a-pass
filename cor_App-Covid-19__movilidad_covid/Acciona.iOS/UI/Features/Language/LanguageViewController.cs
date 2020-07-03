using System;
using Acciona.Domain.Model.Employee;
using Acciona.iOS.Utils;
using Acciona.Presentation.UI.Features.Language;
using BaseIOS.UI;
using Foundation;
using iOS.UI.Styles;
using UIKit;

namespace Acciona.iOS.UI.Features.Language
{
    public partial class LanguageViewController : BaseViewController<LanguagePresenter>, LanguageUI
    {
        

        public LanguageViewController() : base("LanguageViewController", null)
        {            
        }

        protected override void AssingViews()
        {
            BackButton.TouchUpInside += (o, e) => presenter.BackClicked();
            ModifyButton.TouchUpInside += (o, e) => presenter.ModifyClicked();

            SpanishButton.TouchUpInside += LangButton_TouchUpInside;
            EnglishButton.TouchUpInside += LangButton_TouchUpInside;

            styleView();
            applyTraslations();
        }

        private void LangButton_TouchUpInside(object sender, EventArgs e)
        {
            if (sender == SpanishButton)
                presenter.LangClicked("es");
            if (sender == EnglishButton)
                presenter.LangClicked("en");
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        private void applyTraslations()
        {
            TitleViewLabel.Text = AppDelegate.LanguageBundle.GetLocalizedString("language_title");
            ModifyButton.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("language_modify"), UIControlState.Normal);
            SpanishButton.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("language_spanish"), UIControlState.Normal);
            EnglishButton.SetTitle(AppDelegate.LanguageBundle.GetLocalizedString("language_english"), UIControlState.Normal);
        }

        private void styleView()
        {

            ModifyButton.Layer.CornerRadius = 4;
            SpanishButton.Layer.CornerRadius = 4;
            SpanishButton.Layer.BorderWidth = 1;
            SpanishButton.Layer.BorderColor = "#f3f3f3".ToUIColor().CGColor;
            EnglishButton.Layer.CornerRadius = 4;
            EnglishButton.Layer.BorderWidth = 1;
            EnglishButton.Layer.BorderColor = "#f3f3f3".ToUIColor().CGColor;
            SpanishButton.setLittleDownBorderShadow();
            EnglishButton.setLittleDownBorderShadow();
        }

        public void SetConfiguredLang(string configuredLang)
        {
            SpanishButton.Layer.BorderColor = "#f3f3f3".ToUIColor().CGColor;
            SpanishButton.BackgroundColor = "#FFFFFF".ToUIColor();
            EnglishButton.Layer.BorderColor = "#f3f3f3".ToUIColor().CGColor;
            EnglishButton.BackgroundColor = "#FFFFFF".ToUIColor();
            SpanishButton.SetImage(null, UIControlState.Normal);
            EnglishButton.SetImage(null, UIControlState.Normal);
            if (configuredLang.Equals("es"))
            {
                SpanishButton.Layer.BorderColor = "#999999".ToUIColor().CGColor;
                SpanishButton.BackgroundColor = "#F3F3F3".ToUIColor();
                SpanishButton.SetImage(UIImage.FromBundle("next"), UIControlState.Normal);
            }
            else if (configuredLang.Equals("en"))
            {
                EnglishButton.Layer.BorderColor = "#999999".ToUIColor().CGColor;
                EnglishButton.BackgroundColor = "#F3F3F3".ToUIColor();
                EnglishButton.SetImage(UIImage.FromBundle("check"), UIControlState.Normal);
            }
        }
    }
}

