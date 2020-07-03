using System;
using Acciona.Domain.Model.Employee;
using Acciona.iOS.UI.Features.MedicalTest;
using Acciona.Presentation.UI.Features.MedicalInfoEdit;
using BaseIOS.UI;
using Foundation;
using iOS.UI.Styles;
using UIKit;

namespace Acciona.iOS.UI.Features.MedicalInfoEdit
{
    public partial class MedicalInfoEditViewController : BaseViewController<MedicalInfoEditPresenter>,MedicalInfoEditUI
    {
        private Ficha ficha;
        private object actualview;

        public MedicalInfoEditViewController(Ficha ficha) : base("MedicalInfoEditViewController", null)
        {
            this.ficha = ficha;
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
            presenter.Ficha = ficha;
        }

        protected override void AssingViews()
        {
            buttonBack.TouchUpInside += (o, e) => presenter.BackClicked();

            applyTraslations();
            styleView();
        }

        private void applyTraslations()
        {
            labelTitle.Text = AppDelegate.LanguageBundle.GetLocalizedString("medical_title_modify");
            
        }

        private void styleView()
        {
            labelTitle.Font = Styles.SetHelveticaBoldFont(17);            
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (actualview == null)
            {
                var c = new MedicalTestStep1ViewController
                {
                    View = { Frame = new CoreGraphics.CGRect(0, 0, contentView.Frame.Width, contentView.Frame.Height) }
                };
                c.ShowIndicator(false);
                contentView.Add(c.View);
                c.AskResponded += (o, e) => presenter.QuestionFirstStepClicked(e);
                actualview = c.View;
            }
        }
    }
}

