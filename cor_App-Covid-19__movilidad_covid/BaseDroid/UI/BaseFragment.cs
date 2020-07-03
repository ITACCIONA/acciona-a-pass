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
using Presentation.Navigation.Base;
using Presentation.UI.Base;
using ServiceLocator;
using Droid.Utils;
using static Android.Views.View;
using Android.Support.V4.View;

namespace Droid.UI
{
    public abstract class BaseFragment<TPresenter> : Fragment,IBaseUI  where TPresenter:IBasePresenter
    {

        protected TPresenter presenter;

        protected bool _isBusy;
        protected View _progress;
        protected View _progressWithMessage;

        private bool _isError;
        private AlertDialog _alertError;
        //private View _error;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            UserVisibleHint = true;
            if(presenter==null)
                presenter = Locator.Current.GetService<TPresenter>();
        }

        public override bool UserVisibleHint
        {
            get
            {
                return base.UserVisibleHint;
            }

            set
            {
                base.UserVisibleHint = value;
                if (value && IsResumed)
                    presenter.OnResume();         
            }
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
            //var backKeyListener = new BackKeyListener(presenter.BackClicked);
            //View.SetOnKeyListener(backKeyListener);
        }

        public override void OnPause()
        {            
            //View.SetOnKeyListener(null);
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

        public void HideKeyboard()
        {
            KeyboardUtils.HideSoftInput(Activity);
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

        public virtual void ShowLoading()
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
                    _progress.LayoutParameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
                    _progress.SetOnTouchListener(new BlockTouchListener());
                    ViewCompat.SetElevation(_progress, 10);
                }
                root.Post(() => root.AddView(_progress));
            }            
            _isBusy = true;
        }
        
        public virtual void HideError()
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

       

        public void ShowDialog(string text, string buttonText, Action action)
        {
            int id = Activity.Resources.GetIdentifier(text, "string", Activity.PackageName);
            if (id > 0)
                text = Resources.GetString(id);
            id = Activity.Resources.GetIdentifier(buttonText, "string", Activity.PackageName);
            if (id > 0)
                buttonText = Resources.GetString(id);
            if (Looper.MyLooper() == Looper.MainLooper)
            {                
                ShowMainUIDialog(text, buttonText, action);
            }
            else
            {
                Activity.RunOnUiThread(() => ShowMainUIDialog(text,buttonText, action));
            }
        }

        private void ShowMainUIDialog(string text, string buttonText, Action action)
        {

            if (action != null)
                DialogUtil.ShowDialog(null, text, buttonText, action);
            else
                DialogUtil.ShowDialog(null, text, buttonText);
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
            DialogUtil.ShowDialog(null, text,NegativeButton,PositiveButton, positiveAction);
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

        public void ShowError(String errorMessage, String actionText, Action action)
        {
            if (_isError)
            {
                return;
            }
            _alertError = DialogUtil.ShowDialog(null, errorMessage,actionText, action);
            /*var root = View.FindViewById<RelativeLayout>(Resource.Id.rootFragment);
            if (root != null)
            {
                _error = View.Inflate(Activity, Resource.Layout.view_error, null);
                _error.LayoutParameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
                _error.FindViewById(Resource.Id.back).SetOnTouchListener(new BlockTouchListener());
                _error.FindViewById<TextView>(Resource.Id.textError).Text = errorMessage;
                var actionButton = _error.FindViewById<Button>(Resource.Id.actionButton);
                actionButton.Text = actionText;
                actionButton.Click += (o, e) => action?.Invoke();
                root.Post(() => root.AddView(_error));
            }*/
            _isError = true;
        }

        /*public class BackKeyListener : Java.Lang.Object, IOnKeyListener
        {
            private Func<bool> func;

            public BackKeyListener(Func<bool> func)
            {
                this.func = func;
            }

            public bool OnKey(View v, [GeneratedEnum] Keycode keyCode, KeyEvent e)
            {
                if (e.Action==KeyEventActions.Up && keyCode == Keycode.Back)
                {
                    if(!func())
                        return true;
                }
                return false;
            }
        }*/
        
    }
}