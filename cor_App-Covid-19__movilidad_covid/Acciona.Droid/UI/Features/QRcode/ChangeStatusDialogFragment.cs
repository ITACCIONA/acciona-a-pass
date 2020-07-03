using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace Acciona.Droid.UI.Features.QRcode
{
    public class ChangeStatusDialogFragment : DialogFragment
    {
        private View buttonClose;
        private View buttonShow;        

        public event EventHandler ShowTodo;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStyle((int)Android.App.DialogFragmentStyle.Normal, Resource.Style.FullScreenDialogStyle);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            Dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            var view = inflater.Inflate(Resource.Layout.change_status_dialog_fragment, container, false);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            buttonClose = view.FindViewById(Resource.Id.buttonClose);
            buttonShow = view.FindViewById(Resource.Id.buttonShow);
            buttonClose.Click += ButtonClose_Click;
            buttonShow.Click += ButtonShow_Click;
        }

        private void ButtonShow_Click(object sender, EventArgs e)
        {
            Dismiss();
            ShowTodo?.Invoke(this, null);
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Dismiss();
        }
    }
}
