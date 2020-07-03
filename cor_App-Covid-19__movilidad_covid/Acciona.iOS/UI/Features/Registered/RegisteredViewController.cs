using System;
using Acciona.Presentation.UI.Features.Registered;
using BaseIOS.UI;
using UIKit;

namespace Acciona.iOS.UI.Features.Registered
{
    public partial class RegisteredViewController : BaseViewController<RegisteredPresenter>,RegisteredUI
    {
        public RegisteredViewController() : base("RegisteredViewController", null)
        {
        }

        protected override void AssingPresenterView()
        {
            presenter.View = this;
        }

        protected override void AssingViews()
        {
            button.TouchUpInside += (o, e) => presenter.ContinueClicked();
        }
    }
}

