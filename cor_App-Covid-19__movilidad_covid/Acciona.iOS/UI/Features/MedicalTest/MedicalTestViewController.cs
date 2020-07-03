using System;
using Acciona.Presentation.UI.Features.MedicalTest;
using BaseIOS.UI;
using Foundation;
using iOS.UI.Styles;
using UIKit;

namespace Acciona.iOS.UI.Features.MedicalTest
{
    public partial class MedicalTestViewController : BaseViewController<MedicalTestPresenter>,MedicalTestUI
    {
        private UIView actualview=null;
        private bool canBack = false;

        public MedicalTestViewController(bool canBack = false) : base("MedicalTestViewController", null)
        {
            this.canBack = canBack;
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
            presenter.canBack = canBack;
        }

        protected override void AssingViews()
        {            
            buttonBack.TouchUpInside += (o, e) => presenter.BackClicked();
            
            labelTitle.Font = Styles.SetHelveticaBoldFont(17);
        }

        public void ShowConclusionStep(bool?[] responses)
        {
            buttonBack.Hidden = false;
            if (actualview != null)
                actualview.RemoveFromSuperview();
            var c = new MedicalTestStep4ViewController(responses)
            {
                View = {Frame = new CoreGraphics.CGRect(0, 0, contentView.Frame.Width, contentView.Frame.Height)}
            };
            contentView.Add(c.View);
            c.ConfirmedClicked += (o, e) => presenter.TestFinished();
            actualview = c.View;
        }

        public void ShowFirstStep()
        {
            buttonBack.Hidden = !canBack;
            if (actualview != null)
                actualview.RemoveFromSuperview();
            var c = new MedicalTestStep1ViewController
            {
                View = {Frame = new CoreGraphics.CGRect(0, 0, contentView.Frame.Width, contentView.Frame.Height)}
            };
            contentView.Add(c.View);
           c.AskResponded += (o,e)=>presenter.QuestionFirstStepClicked(e);
           actualview = c.View;
        }

        public void ShowSecondStep()
        {
            buttonBack.Hidden = false;
            if (actualview != null)
                actualview.RemoveFromSuperview();
            var c = new MedicalTestStep2ViewController
            {
                View = {Frame = new CoreGraphics.CGRect(0, 0, contentView.Frame.Width, contentView.Frame.Height)}
            };
            contentView.Add(c.View);
            c.AskResponded += (o, e) => presenter.QuestionSecondStepClicked(e);
            actualview = c.View;
        }

        public void ShowThirdStep()
        {
            buttonBack.Hidden = false;
            if (actualview != null)
                actualview.RemoveFromSuperview();
            var c = new MedicalTestStep3ViewController
            {
                View = {Frame = new CoreGraphics.CGRect(0, 0, contentView.Frame.Width, contentView.Frame.Height)}
            };
            contentView.Add(c.View);
            c.AskResponded += (o, e) => presenter.QuestionThirdStepClicked(e);
            actualview = c.View;
        }

        public void ConfigureTitleAndBack(bool canBack)
        {
            if (canBack)
            {
                buttonBack.Hidden = false;
                labelTitle.Text = AppDelegate.LanguageBundle.GetLocalizedString("medical_title_modify");
            }
            else
            {
                buttonBack.Hidden = true;
                labelTitle.Text = AppDelegate.LanguageBundle.GetLocalizedString("medical_title");
            }
        }
    }
}

