using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Presentation.Navigation.Base;
using Presentation.UI.Base;
using ServiceLocator;
using Droid.Utils;

namespace Droid.UI
{
    public abstract class BasicFragment : Fragment
    {
       
        private bool _isBusy;
        private View _progress;

        private bool _isError;
        private AlertDialog _alertError;
        //private View _error;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }




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
                        _progress.LayoutParameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
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

        public void ShowError(String errorMessage, String actionText, Action action)
        {
            if (_isError)
            {
                return;
            }
            _alertError=DialogUtil.ShowDialog(null, errorMessage, actionText,action);
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

        public void ShowDialog(string text,string buttonText, Action action)
        {
            DialogUtil.ShowDialog(null, text, buttonText,action);
        }

        public void ShowDialog(string text, string NegativeButton, string PositiveButton, Action positiveAction)
        {
            DialogUtil.ShowDialog(null, text,NegativeButton,PositiveButton, positiveAction);
        }
    }
}