using System.Linq;
using System.Threading.Tasks;
using Android.Content.PM;
using Android.OS;
using Android.App;
using System;
using Android.Support.V4.Content;
using Android.Support.V4.App;

namespace Droid.Utils
{
    public class PermissionHelperDialogFragment : Android.Support.V4.App.DialogFragment
    {
        private const int RequestCode = 6666;
        private TaskCompletionSource<bool> _taskCompletion = new TaskCompletionSource<bool>();
        private string[] _allPermissions;
        private string[] _neededPermissions;
        private const string PermissionsKey = "permissions_key";

        public static PermissionHelperDialogFragment NewInstance(string[] permissions)
        {
            var args = new Bundle();
            args.PutStringArray(PermissionsKey, permissions);
            var fragment = new PermissionHelperDialogFragment
            {
                Arguments = args
            };
            return fragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Cancelable = false;
            RequestPermissions(_neededPermissions, RequestCode);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            if (requestCode == RequestCode)
            {
                _taskCompletion.TrySetResult(grantResults.All(p => p == Permission.Granted));
            }
            DismissAllowingStateLoss();
        }

        public override void Show(Android.Support.V4.App.FragmentManager manager, string tag)
        {
            throw new Exception("You should use ShowRequestPermissionsAsync");
        }

        public override int Show(Android.Support.V4.App.FragmentTransaction transaction, string tag)
        {
            throw new Exception("You should use ShowRequestPermissionsAsync");
        }

        public async Task<bool> ShowRequestPermissionsAsync(Activity act, Android.Support.V4.App.FragmentManager manager, string tag)
        {
            if (Build.VERSION.SdkInt <= BuildVersionCodes.Kitkat)
            {
                return true;
            }

            if (Arguments != null)
            {
                _allPermissions = Arguments.GetStringArray(PermissionsKey);
            }

            _neededPermissions = _allPermissions.Where(p => ContextCompat.CheckSelfPermission(act,p) != (int)Permission.Granted).ToArray();
            if (_neededPermissions.Length > 0)
            {
                base.Show(manager, tag);
                return await _taskCompletion.Task;
            }

            return true;
        }

    }
}
