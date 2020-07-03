using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Acciona.Droid.Utils
{
    public class SpaceItemDecoration : RecyclerView.ItemDecoration
    {
        private int space;
        private int previous;

        public SpaceItemDecoration(int space,int previous)
        {
            this.space = space;
            this.previous = previous;
        }
        public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
        {
            int position = parent.GetChildLayoutPosition(view);
            
            // Add top margin only for the first item to avoid double space between items
            if ( position%2 == 0)
            {
                outRect.Top = 0;
                outRect.Left = space-previous;
                outRect.Right = space/2-previous;
                outRect.Bottom = space-previous;
            }
            else
            {
                outRect.Top = 0;
                outRect.Left = space/2-previous;
                outRect.Right = space-previous;
                outRect.Bottom = space-previous;
            }
            if(position<2)
                outRect.Top = space-previous;
        }
    }
}