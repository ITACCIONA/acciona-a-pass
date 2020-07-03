using System;
using Acciona.Presentation.UI.Features.Passport;
using BaseIOS.UI;
using UIKit;

namespace Acciona.iOS.UI.Features.Passport
{
    public partial class PassportViewController : BaseViewController<PassportPresenter>, PassportUI
    {
        public PassportViewController() : base("PassportViewController", null)
        {
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override void AssingViews()
        {

        }
    }
}

