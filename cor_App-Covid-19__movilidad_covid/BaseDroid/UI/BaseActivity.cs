using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Presentation.Navigation.Base;
using Presentation.UI.Base;
using ServiceLocator;
using Droid.Utils;

namespace Droid.UI
{
    public abstract class BaseActivity<TPresenter> : AppCompatActivity, IBaseUI  where TPresenter:IBasePresenter
    {

        protected TPresenter presenter;

        private bool _isBusy;
        private View _progress;
        private View _progressWithMessage;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            base.OnCreate(savedInstanceState);            
            presenter = Locator.Current.GetService<TPresenter>();        
            int layoutRes = GetActivityLayout();
            if (layoutRes == 0)
            {
                throw new Exception(
                        "getLayoutRes() returned 0, which is not allowed. "
                                + "If you don't want to use getLayoutRes() but implement your own view for this "
                                + "fragment manually, then you have to override onCreateView();");
            }
            SetContentView(layoutRes);
            AssingViews();
            AssingPresenterView();                           
            presenter?.OnCreate();            
        }

        protected abstract void AssingViews();

        protected override void OnResume()
        {
            base.OnResume();
            presenter?.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
            presenter?.OnPause();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            presenter?.OnDestroy();
        }

        protected abstract int GetActivityLayout();

        protected abstract void AssingPresenterView();

        public void HideKeyboard()
        {
            KeyboardUtils.HideSoftInput(this);
        }

        public void HideLoading()
        {
            if (!_isBusy)
            {
                return;
            }
            var root = FindViewById<RelativeLayout>(Resource.Id.root);
            root?.Post(() => root.RemoveView(_progress));

            root.RemoveView(_progressWithMessage);
            _progressWithMessage = null;

            _isBusy = false;
        }

        public void ShowLoading()
        {
            if (_isBusy)
            {
                return;
            }
            var root = FindViewById<RelativeLayout>(Resource.Id.root);
            if (root != null)
            {
                if (_progress == null)
                {
                    _progress = View.Inflate(this, Resource.Layout.view_progress, null);
                    _progress.LayoutParameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
                    _progress.SetOnTouchListener(new BlockTouchListener());
                }
                root.Post(() => root.AddView(_progress));
            }
            _isBusy = true;
        }
        

        public void ShowDialog(string text, String buttonText, Action action)
        {
            int id = Resources.GetIdentifier(text, "string", PackageName);
            if (id > 0)
                text = Resources.GetString(id);
            id = Resources.GetIdentifier(buttonText, "string", PackageName);
            if (id > 0)
                buttonText = Resources.GetString(id);
            if (Looper.MyLooper() == Looper.MainLooper)
            {                
                ShowMainUIDialog(text,buttonText, action);
            }
            else
            {
                RunOnUiThread(() => ShowMainUIDialog(text,buttonText, action));
            }
        }

        private static void ShowMainUIDialog(string text, String buttonText,Action action)
        {
            if (action != null)
                DialogUtil.ShowDialog(null, text, buttonText,action);
            else
                DialogUtil.ShowDialog(null, text,buttonText);
        }

        public void ShowDialog(string text, string NegativeButton, string PositiveButton, Action positiveAction)
        {
            int id = Resources.GetIdentifier(text, "string", PackageName);
            if (id > 0)
                text = Resources.GetString(id);
            id = Resources.GetIdentifier(NegativeButton, "string", PackageName);
            if (id > 0)
                NegativeButton = Resources.GetString(id);
            id = Resources.GetIdentifier(PositiveButton, "string", PackageName);
            if (id > 0)
                PositiveButton = Resources.GetString(id);
            DialogUtil.ShowDialog(null, text, NegativeButton, PositiveButton, positiveAction);
        }

        public void ShowDialog(string text, string NegativeButton, Action negativeAction,string PositiveButton, Action positiveAction)
        {
            int id = Resources.GetIdentifier(text, "string",PackageName);
            if (id > 0)
                text = Resources.GetString(id);
            id = Resources.GetIdentifier(NegativeButton, "string", PackageName);
            if (id > 0)
                NegativeButton = Resources.GetString(id);
            id = Resources.GetIdentifier(PositiveButton, "string", PackageName);
            if (id > 0)
                PositiveButton = Resources.GetString(id);
            DialogUtil.ShowDialog(null, text, NegativeButton,negativeAction, PositiveButton, positiveAction);
        }

        /*public override void OnBackPressed()
        {
            if(presenter.BackClicked())
                base.OnBackPressed();
        }*/
    }
}