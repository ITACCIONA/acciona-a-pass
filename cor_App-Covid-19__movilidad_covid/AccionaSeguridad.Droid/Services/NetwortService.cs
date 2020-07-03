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
using Android.Net;
using Acciona.Domain.Services;
using Acciona.Domain.Model;
using ServiceLocator;

namespace AccionaSeguridad.Droid.Services
{
    public class NetworkService : INetworkService
    {
    
        public NetworkType GetConnectivityStatus()
        {
            Context context = Locator.Current.GetService<Activity>();
            ConnectivityManager cm = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);

            NetworkInfo activeNetwork = cm.ActiveNetworkInfo;
            if (null != activeNetwork)
            {
                if (activeNetwork.Type == ConnectivityType.Wifi)
                    return NetworkType.Wifi;

                if (activeNetwork.Type == ConnectivityType.Mobile)
                    return NetworkType.Mobile;
            }
            return NetworkType.Not_Conected;
        }        

        public  bool IsConnected()
        {
            Context context = Locator.Current.GetService<Activity>();
            NetworkType conn = GetConnectivityStatus();
            if (conn == NetworkType.Wifi)
            {
                return true;
            }
            else if (conn == NetworkType.Mobile)
            {
                return true;
            }
            return false;
        }
    }
}