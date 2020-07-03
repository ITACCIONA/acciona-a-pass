using Android.App;
using Android.Support.V7.App;
using Presentation.Navigation.Base;
using ServiceLocator;

namespace Droid.Navigation
{
    public class BaseNavigator : IBaseNavigator
    {
        protected AppCompatActivity activity;

        public BaseNavigator()
        {
            activity = Locator.Current.GetService<Activity>() as AppCompatActivity;
        }
    }
}