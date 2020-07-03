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
using System.Threading.Tasks;
using ServiceLocator;
using Acciona.Domain.Services;
using Acciona.Domain.Model;
using Acciona.Domain.UseCase;

namespace AccionaSeguridad.Droid.Receiver
{
    [BroadcastReceiver(Enabled = true, Exported = false, Label = "Connectivity Plugin Broadcast Receiver")]
    //[IntentFilter(new[] { "android.net.conn.CONNECTIVITY_CHANGE" })]
    [Android.Runtime.Preserve(AllMembers = true)]
    public class NetworkChangeReceiver : BroadcastReceiver
    {

        public override void OnReceive(Context context, Intent intent)
        {
            INetworkService networkService = Locator.Current.GetService<INetworkService>();
            NetworkType status = networkService.GetConnectivityStatus();
            switch (status)
            {
                case NetworkType.Not_Conected:
                    //Toast.MakeText(context, "Not connected", ToastLength.Short).Show();
                    break;
                case NetworkType.Wifi:
                    //Toast.MakeText(context, "WIFI", ToastLength.Short).Show();
                    Locator.Current.GetService<SincroPendingUseCase>().Execute();
                    break;
                case NetworkType.Mobile:
                    //Toast.MakeText(context, "MOBILE", ToastLength.Short).Show();
                    //if (synchronizationService.GetPendingEntries() > 0)
                    //    Task.Run(() => synchronizationService.SynchronicePendingEntries());
                    Locator.Current.GetService<SincroPendingUseCase>().Execute();
                    break;
            }
        }
    }
}