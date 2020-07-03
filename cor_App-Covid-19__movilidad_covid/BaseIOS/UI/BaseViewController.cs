using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Presentation.UI.Base;
using ServiceLocator;
using UIKit;

namespace BaseIOS.UI
{
    public abstract class BaseViewController<TPresenter> : UIViewController, IUITextFieldDelegate, IBaseUI where TPresenter : IBasePresenter
    {
        protected TPresenter presenter;

        private bool _isBusy;

        private List<UITextField> textFields;
        private LoadingView loadingView;
        private NSObject _willResignActiveNotificationObserver;
        private NSObject _didBecomeActiveNotificationObserver;
        private NSObject _onShowKeyboardObserver;
        private NSObject _onHideKeyboardObserver;

        public BaseViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
            if (HandlesKeyboardNotifications)
            {
                _onShowKeyboardObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
                _onHideKeyboardObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);
            }
        }
        

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Locator.CurrentMutable.Register<UIViewController>(() => this);
            presenter = Locator.Current.GetService<TPresenter>();
            AssingViews();
            AssingPresenterView();
            presenter?.OnCreate();
        }

        protected abstract void AssingViews();

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            presenter?.OnResume();
            _willResignActiveNotificationObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.WillResignActiveNotification, ResignActiveNotifcationCallback);
            _didBecomeActiveNotificationObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.DidBecomeActiveNotification, BecomeActiveNotifcationCallback);
        }

        private void BecomeActiveNotifcationCallback(NSNotification obj)
        {
            presenter?.OnResume();
        }

        private void ResignActiveNotifcationCallback(NSNotification obj)
        {
            presenter?.OnPause();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);            
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            presenter?.OnPause();
            if (_willResignActiveNotificationObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_willResignActiveNotificationObserver);
            }
            if (_didBecomeActiveNotificationObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_didBecomeActiveNotificationObserver);
            }
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            NSNotificationCenter.DefaultCenter.RemoveObserver(_onHideKeyboardObserver);
            NSNotificationCenter.DefaultCenter.RemoveObserver(_onShowKeyboardObserver);
            presenter?.OnDestroy();
        }

        protected abstract void AssingPresenterView();

        public void HideKeyboard()
        {
            View.EndEditing(true);
        }



        public void ShowDialog(string text, string buttonText, Action action)
        {
            InvokeOnMainThread(() =>
            {
                var bundle=Locator.Current.GetService<NSBundle>();
                UIAlertController _error = UIAlertController.Create(null, bundle.GetLocalizedString(text), UIAlertControllerStyle.Alert);
                UIAlertAction alertAction = UIAlertAction.Create(bundle.GetLocalizedString(buttonText), UIAlertActionStyle.Default, (o) =>
                {
                    _error.DismissViewController(false, null);
                    action?.Invoke();
                });

                _error.AddAction(alertAction);
                PresentViewController(_error, false, null);
            });
        }

        public void ShowDialog(string text, string NegativeButton, string PositiveButton, Action positiveAction)
        {
            var bundle = Locator.Current.GetService<NSBundle>();
            UIAlertController _error = UIAlertController.Create(null, bundle.GetLocalizedString(text), UIAlertControllerStyle.Alert);
            UIAlertAction alertAction = UIAlertAction.Create(bundle.GetLocalizedString(PositiveButton), UIAlertActionStyle.Default, (o) =>
            {
                _error.DismissViewController(false, null);
                positiveAction?.Invoke();
            });

            UIAlertAction alertNegativeAction = UIAlertAction.Create(bundle.GetLocalizedString(NegativeButton), UIAlertActionStyle.Default, (o) =>
            {
                _error.DismissViewController(false, null);
            });

            _error.AddAction(alertAction);
            _error.AddAction(alertNegativeAction);
            PresentViewController(_error, false, null);
        }

        public void ShowDialog(string text, string NegativeButton, Action negativeAction,string PositiveButton, Action positiveAction)
        {
            var bundle = Locator.Current.GetService<NSBundle>();

            UIAlertController _error = UIAlertController.Create(null, bundle.GetLocalizedString(text), UIAlertControllerStyle.Alert);

            UIAlertAction alertAction = UIAlertAction.Create(bundle.GetLocalizedString(PositiveButton), UIAlertActionStyle.Default, (o) =>
            {
                _error.DismissViewController(false, null);
                positiveAction?.Invoke();
            });

            UIAlertAction alertNegativeAction = UIAlertAction.Create(bundle.GetLocalizedString(NegativeButton), UIAlertActionStyle.Default, (o) =>
            {
                _error.DismissViewController(false, null);
                negativeAction?.Invoke();
            });

            _error.AddAction(alertAction);
            _error.AddAction(alertNegativeAction);
            PresentViewController(_error, false, null);
        }

        public void ShowDialogWithTextField(string text, string textFieldPlaceholder, bool isSecureField, string negativeButton, string positiveButton, Action<string> positiveAction)
        {
            var bundle = Locator.Current.GetService<NSBundle>();
            UIAlertController alert = UIAlertController.Create(null, bundle.GetLocalizedString(text), UIAlertControllerStyle.Alert);
            UIAlertAction okAction = UIAlertAction.Create(bundle.GetLocalizedString(positiveButton), UIAlertActionStyle.Default, (o) =>
            {
                alert.DismissViewController(false, null);
                positiveAction?.Invoke(alert.TextFields[0]?.Text);
            });
            UIAlertAction cancelAction = UIAlertAction.Create(bundle.GetLocalizedString(negativeButton), UIAlertActionStyle.Cancel, (o) =>
            {
                alert.DismissViewController(false, null);
            });

            alert.AddAction(okAction);
            alert.AddAction(cancelAction);
            alert.AddTextField((textField) =>
            {
                textField.Placeholder = textFieldPlaceholder;
                textField.SecureTextEntry = isSecureField;
            });

            PresentViewController(alert, false, null);
        }

        public void ShowLoading()
        {
            if (_isBusy)
            {
                return;
            }
            //var currenWindow = UIApplication.SharedApplication.KeyWindow;

            if (loadingView == null)
            {
                loadingView = LoadingView.Create();
                loadingView.ConfigureView(View.Frame);
                View.Add(loadingView);
                //loadingView.ConfigureView(currenWindow.Frame);
                //currenWindow.Add(loadingView);
            }
            else if (!View.Subviews.Contains<UIView>(loadingView))
            {
                View.Add(loadingView);
            }
            else
            {
                View.BringSubviewToFront(loadingView);
            }
            /*else if (!currenWindow.Subviews.Contains<UIView>(loadingView))
            {
                currenWindow.Add(loadingView);
            }
            else
            {
                currenWindow.BringSubviewToFront(loadingView);
            }*/

            loadingView.StartAnimation();
            _isBusy = true;
        }

        public void HideLoading()
        {
            if (!_isBusy)
            {
                return;
            }
            loadingView.RemoveFromSuperview();
            _isBusy = false;
        }


        #region Keyboard display behaviour


        public virtual bool HandlesKeyboardNotifications
        {
            get { return false; }
        }

        public static UIView FindFirstResponder(UIView view)
        {
            if (view.IsFirstResponder)
            {
                return view;
            }
            foreach (UIView subView in view.Subviews)
            {
                var firstResponder = FindFirstResponder(subView);
                if (firstResponder != null)
                    return firstResponder;
            }
            return null;
        }

        private void OnKeyboardNotification(NSNotification notification)
        {
            if (IsViewLoaded)
            {

                //Check if the keyboard is becoming visible
                bool visible = notification.Name == UIKeyboard.WillShowNotification;

                UIView activeView = FindFirstResponder(View);

                //Start an animation, using values from the keyboard
                UIView.BeginAnimations("AnimateForKeyboard");
                UIView.SetAnimationBeginsFromCurrentState(true);
                UIView.SetAnimationDuration(UIKeyboard.AnimationDurationFromNotification(notification));
                UIView.SetAnimationCurve((UIViewAnimationCurve)UIKeyboard.AnimationCurveFromNotification(notification));

                //Pass the notification, calculating keyboard height, etc.
                bool landscape = InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft || InterfaceOrientation == UIInterfaceOrientation.LandscapeRight;
                if (visible)
                {
                    nfloat h = 0;
                    if (activeView != null)
                    {
                        h = activeView.Frame.Y;
                        UIView super = activeView.Superview;
                        while (super != null)
                        {
                            if (typeof(UIScrollView) == super.GetType())
                            {
                                h = h + super.Frame.Y - ((UIScrollView)super).ContentOffset.Y;
                            }
                            else
                            {
                                h = h + super.Frame.Y;
                            }
                            super = super.Superview;
                        }
                    }
                    var keyboardFrame = UIKeyboard.FrameEndFromNotification(notification);
                    OnKeyboardChanged(visible, keyboardFrame.Height, h);
                    //OnKeyboardChanged (visible, landscape ? keyboardFrame.Width : keyboardFrame.Height,activeView.Frame.Y);
                }
                else
                {
                    nfloat h = 0;
                    if (activeView != null)
                    {
                        h = activeView.Frame.Y;
                        UIView super = activeView.Superview;
                        while (super != null)
                        {
                            if (typeof(UIScrollView) == super.GetType())
                            {
                                h = h + super.Frame.Y - ((UIScrollView)super).ContentOffset.Y;
                            }
                            else
                            {
                                h = h + super.Frame.Y;
                            }
                            super = super.Superview;
                        }
                    }
                    var keyboardFrame = UIKeyboard.FrameBeginFromNotification(notification);
                    OnKeyboardChanged(visible, keyboardFrame.Height, h);
                    //OnKeyboardChanged (visible, landscape ? keyboardFrame.Width : keyboardFrame.Height,activeView.Frame.Y);
                }

                //Commit the animation
                UIView.CommitAnimations();
            }
        }

        /// <summary>
        /// Override this method to apply custom logic when the keyboard is shown/hidden
        /// </summary>
        /// <param name='visible'>
        /// If the keyboard is visible
        /// </param>
        /// <param name='height'>
        /// Calculated height of the keyboard (width not generally needed here)
        /// </param>
        protected virtual void OnKeyboardChanged(bool visible, nfloat keyboardHeight, nfloat position)
        {
            //We "center" the popup when the keyboard appears/disappears
            var frame = View.Frame;
            if (visible)
            {
                if (position + 50 > View.Frame.Height - keyboardHeight)
                    frame.Y = (View.Frame.Height - keyboardHeight) - (position + 50);
                else
                    frame.Y = 0;
            }
            else
                frame.Y = 0;
            View.Frame = frame;
        }

        public void ShowFullScreenLoading()
        {

        }

        public void HideFullScreenLoading()
        {

        }

        public void ShowDialog(string text, string NegativeButton, string PositiveButton, Action negativeAction, Action positiveAction)
        {
            UIAlertController _error = UIAlertController.Create(null, text, UIAlertControllerStyle.Alert);


            UIAlertAction alertAction = UIAlertAction.Create(PositiveButton, UIAlertActionStyle.Default, (o) =>
            {
                _error.DismissViewController(false, null);
                positiveAction?.Invoke();
            });

            _error.AddAction(alertAction);

            UIAlertAction negAlertAction = UIAlertAction.Create(NegativeButton, UIAlertActionStyle.Cancel, (o) =>
            {
                _error.DismissViewController(false, null);
                negativeAction?.Invoke();
            });

            _error.AddAction(negAlertAction);
            PresentViewController(_error, false, null);
        }

        public void ShowDialogDefaultStyleButtons(string text, string NegativeButton, string PositiveButton, Action negativeAction, Action positiveAction)
        {
            UIAlertController _error = UIAlertController.Create(null, text, UIAlertControllerStyle.Alert);


            UIAlertAction alertAction = UIAlertAction.Create(PositiveButton, UIAlertActionStyle.Default, (o) =>
            {
                _error.DismissViewController(false, null);
                positiveAction?.Invoke();
            });

            _error.AddAction(alertAction);

            UIAlertAction negAlertAction = UIAlertAction.Create(NegativeButton, UIAlertActionStyle.Default, (o) =>
            {
                _error.DismissViewController(false, null);
                negativeAction?.Invoke();
            });

            _error.AddAction(negAlertAction);
            PresentViewController(_error, false, null);
        }



        #endregion

        /*#region Orientation methods

        public override bool ShouldAutorotate()
        {
            return false;
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return UIInterfaceOrientationMask.Portrait;
        }

        public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation()
        {
            return UIInterfaceOrientation.Portrait;
        }

        public void RestrictRotation(bool restriction)
        {
            AppDelegate app = (AppDelegate)UIApplication.SharedApplication.Delegate;
            app.RestrictRotation = restriction;
        }

        #endregion*/
    }
}