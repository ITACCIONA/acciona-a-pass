using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Acciona.iOS.UI.Features.WorkingCenter;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using iOS.UI.Styles;
using ServiceLocator;
using UIKit;

namespace Acciona.iOS.UI.Controls
{
    [Register("ListStringTextfield"), DesignTimeVisible(true)]
    public class ListStringTextfield : UITextField
    {
        private List<string> objects;
        public event EventHandler<string> ItemChanged;
        private string selection = null;
        private bool dialogIsShow;
     
        public ListStringTextfield() : base()
        {
            InitView();
        }

        public ListStringTextfield(IntPtr intPtr) : base(intPtr)
        {
            InitView();
        }

        public ListStringTextfield(Foundation.NSObjectFlag t) : base(t)
        {
            InitView();
        }

        public ListStringTextfield(Foundation.NSCoder coder) : base(coder)
        {
            InitView();
        }

        public ListStringTextfield(CoreGraphics.CGRect frame) : base(frame)
        {
            InitView();
        }

        private void InitView()
        {

            Style(UIColor.Gray);
            EditingDidBegin += (obj, arg) =>
            {
                Style(UIColor.Gray);
                ShowDialog();
                ResignFirstResponder();
            };

            EditingDidEnd += (obj, arg) =>
            {
                Style(UIColor.DarkGray);                
            };        
        }        

        public override bool Enabled
        {
            get => base.Enabled; set
            {
                base.Enabled = value;
                if (Enabled)
                {
                    //TODO check if editing
                }
                else
                {
                    Style(UIColor.LightGray);
                }

            }
        }

        private void Style(UIColor color)
        {
            Layer.CornerRadius = 3;
            Layer.BorderWidth = 1;
            Layer.BorderColor = color.CGColor;

        }

        public void SetListableObjects(IEnumerable<string> objects)
        {
            this.objects = objects.ToList();
            if (objects.Count() > 0)
            {
                selection = this.objects[0];
                Text = selection;
            }
            else
            {
                selection = null;
                Text = "";
            }
        }

        public void ForceInvokeEvent()
        {
            //Pensado para rellenar formularios con dependencias
            ItemChanged?.Invoke(this, selection);
        }

        public string GetSelection()
        {
            return selection;
        }

        public void SetSelectionIndex(int selection)
        {
            if (objects != null && selection >= 0 && objects.Count > 0)
            {
                this.selection = objects.ElementAt(selection);
                String strName = objects.ElementAt(selection);
                /*if (Looper.MyLooper() == Looper.MainLooper)
                {*/
                    Text = strName;
                /*}
                else
                {
                    Post(() => Text = strName);
                }*/
            }
            else
            {

            }
        }


        private void ShowDialog()
        {
            if (dialogIsShow)
                return;
            dialogIsShow = true;
                        
            var picker =new ListStringPickerViewController(objects);
            picker.OnSelected += (o, item) =>
            {
                Text = item;
                var oldSelection = selection;
                selection = item;
                dialogIsShow = false;
                if (!selection.Equals(oldSelection))
                    ItemChanged?.Invoke(this, item);
            };            
            picker.OnDismissEvent += (o, e) => dialogIsShow = false;
            var rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController as UINavigationController;
            picker.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            rootViewController.PresentViewController(picker, false, null);
         }


    }
}
