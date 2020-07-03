using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Presentation.UI.Base;
using ServiceLocator;
using Droid.Utils;

namespace Droid.UI
{
    public abstract class BaseDialogFragment<TPresenter> : DialogFragment, IBaseErrorUI where TPresenter : IBasePresenter
    {

        protected TPresenter presenter;

        private bool _isBusy;
        private View _progress;
        private View _progressWithMessage;

        private bool _isError;
        private AlertDialog _alertError;
        //private View _error;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            presenter = Locator.Current.GetService<TPresenter>();
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            int layoutRes = GetFragmentLayout();
            if (layoutRes == 0)
            {
                throw new Exception(
                        "getLayoutRes() returned 0, which is not allowed. "
                                + "If you don't want to use getLayoutRes() but implement your own view for this "
                                + "fragment manually, then you have to override onCreateView();");
            }
            View view = inflater.Inflate(layoutRes, container, false);
            return view;
        }

        protected abstract void AssingViews(View view);


        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            AssingViews(view);
            AssingPresenterView();
            presenter?.OnCreate();
        }

        public override void OnResume()
        {
            base.OnResume();
            presenter.OnResume();
        }

        public override void OnPause()
        {
            base.OnPause();
            presenter.OnPause();
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            presenter.OnDestroy();
        }

        protected abstract int GetFragmentLayout();

        protected abstract void AssingPresenterView();

        public virtual void HideKeyboard()
        {
            //KeyboardUtils.HideSoftInput(Activity);            
            KeyboardUtils.HideKeyboard(Context, View);
        }

        public void HideLoading()
        {
            if (!_isBusy)
            {
                return;
            }
            var root = View.FindViewById<RelativeLayout>(Resource.Id.rootFragment);
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
            var root = View.FindViewById<RelativeLayout>(Resource.Id.rootFragment);
            if (root != null)
            {
                if (_progress == null)
                {
                    _progress = View.Inflate(Activity, Resource.Layout.view_progress, null);
                    _progress.LayoutParameters = new RelativeLayout.LayoutParams(root.Width, root.Height);
                    _progress.SetOnTouchListener(new BlockTouchListener());
                }
                root.Post(() => root.AddView(_progress));
            }
            _isBusy = true;
        }        

        public void HideError()
        {
            if (!_isError)
            {
                return;
            }
            //var root = View.FindViewById<RelativeLayout>(Resource.Id.rootFragment);
            //root?.Post(() => root.RemoveView(_error));
            _alertError.Dismiss();
            _isError = false;
        }

        public void ShowError(String errorMessage,String actionText,String buttonText,Action action)
        {
            if (_isError)
            {
                return;
            }
            _alertError = DialogUtil.ShowDialog(null, errorMessage,buttonText, action);
            /*var root = View.FindViewById<RelativeLayout>(Resource.Id.rootFragment);
            if (root != null)
            {
                 _error = View.Inflate(Activity, Resource.Layout.view_error, null);
                 _error.LayoutParameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
                 _error.FindViewById(Resource.Id.back).SetOnTouchListener(new BlockTouchListener());
                _error.FindViewById<TextView>(Resource.Id.textError).Text = errorMessage;
                var actionButton = _error.FindViewById<Button>(Resource.Id.actionButton);
                actionButton.Text = actionText;
                actionButton.Click += (o,e)=>action.Invoke();                               
                root.Post(() => root.AddView(_error));
            }*/
            _isError = true;
        }

        public void ShowDialog(string text, String buttonText, Action action)
        {
            if (Looper.MyLooper() == Looper.MainLooper)
            {
                int id = Activity.Resources.GetIdentifier(text, "string", Activity.PackageName);
                if (id > 0)
                    text = Resources.GetString(id);
                id = Activity.Resources.GetIdentifier(buttonText, "string", Activity.PackageName);
                if (id > 0)
                    buttonText = Resources.GetString(id);
                ShowMainUIDialog(text, buttonText, action);
            }
            else
            {
                Activity.RunOnUiThread(() => ShowMainUIDialog(text,buttonText, action));
            }
        }

        private static void ShowMainUIDialog(string text, String buttonText, Action action)
        {
            if (action != null)
                DialogUtil.ShowDialog(null, text, buttonText,action);
            else
                DialogUtil.ShowDialog(null, text,buttonText);
        }

        public void ShowDialog(string text, string NegativeButton, string PositiveButton, Action positiveAction)
        {
            int id = Activity.Resources.GetIdentifier(text, "string", Activity.PackageName);
            if (id > 0)
                text = Resources.GetString(id);
            id = Activity.Resources.GetIdentifier(NegativeButton, "string", Activity.PackageName);
            if (id > 0)
                NegativeButton = Resources.GetString(id);
            id = Activity.Resources.GetIdentifier(PositiveButton, "string", Activity.PackageName);
            if (id > 0)
                PositiveButton = Resources.GetString(id);
            DialogUtil.ShowDialog(null, text, NegativeButton, PositiveButton, positiveAction);
        }

        public void ShowDialog(string text, string NegativeButton, Action negativeAction, string PositiveButton, Action positiveAction)
        {
            int id = Activity.Resources.GetIdentifier(text, "string", Activity.PackageName);
            if (id > 0)
                text = Resources.GetString(id);
            id = Activity.Resources.GetIdentifier(NegativeButton, "string", Activity.PackageName);
            if (id > 0)
                NegativeButton = Resources.GetString(id);
            id = Activity.Resources.GetIdentifier(PositiveButton, "string", Activity.PackageName);
            if (id > 0)
                PositiveButton = Resources.GetString(id);
            DialogUtil.ShowDialog(null, text, NegativeButton, negativeAction, PositiveButton, positiveAction);
        }

    }
}