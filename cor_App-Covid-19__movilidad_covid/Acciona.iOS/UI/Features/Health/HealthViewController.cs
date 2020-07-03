using System;
using Acciona.Presentation.UI.Features.Health;
using BaseIOS.UI;
using Foundation;
using iOS.UI.Styles;
using UIKit;

namespace Acciona.iOS.UI.Features.Health
{
    public partial class HealthViewController : BaseViewController<HealthPresenter>,HealthUI
    {
        private UIView actualview = null;

        public HealthViewController() : base("HealthViewController", null)
        {
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override void AssingViews()
        {
            buttonBack.TouchUpInside += (o, e) => presenter.BackClicked();

            labelTitle.Text = AppDelegate.LanguageBundle.GetLocalizedString("health_statement");
            labelTitle.Font = Styles.SetHelveticaBoldFont(17);
        }


        public void ShowFirstStep()
        {
            if (actualview != null)
                actualview.RemoveFromSuperview();
            HealthStep1ViewController c = new HealthStep1ViewController();
            c.View.Frame = new CoreGraphics.CGRect(0, 0, contentView.Frame.Width, contentView.Frame.Height);
            contentView.Add(c.View);
            c.NoSymptomsClicked += (o, e) => presenter.ButtonNoSymptomClicked(e[0],e[1]);
            actualview = c.View;
        }

        public void ShowSecondStep()
        {
            if (actualview != null)
                actualview.RemoveFromSuperview();
            HealthStep2ViewController c = new HealthStep2ViewController();
            c.View.Frame = new CoreGraphics.CGRect(0, 0, contentView.Frame.Width, contentView.Frame.Height);
            contentView.Add(c.View);
            c.AskResponded += (o, e) => presenter.ButtonMeetClicked(e);
            actualview = c.View;
        }

        public string GetString(string text)
        {
            return AppDelegate.LanguageBundle.GetLocalizedString(text);
        }
    }
}

