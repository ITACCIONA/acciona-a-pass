
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Droid.Utils
{		
	public class ScreenTouchListener : Java.Lang.Object, View.IOnTouchListener
	{
		private Action action;
        public ScreenTouchListener(Action action)
        {
			this.action = action;
        }

		public bool OnTouch(View v, MotionEvent e)
		{
			action.Invoke();
			return true;
		}
	}
}

