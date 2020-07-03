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
using Android.Widget;

namespace Droid.UI
{ 
    public abstract class BasicActivity:AppCompatActivity
    {

        protected Fragment fragment;
        
        protected abstract Fragment CreateFragment();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.view_activity);            
            fragment = CreateFragment();
            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.container_master, fragment).Commit();
        }

    }
}