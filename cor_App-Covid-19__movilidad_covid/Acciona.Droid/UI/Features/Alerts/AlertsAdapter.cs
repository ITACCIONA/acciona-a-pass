using System;
using System.Collections.Generic;
using System.Linq;
using Acciona.Domain.Model.Employee;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Acciona.Droid.UI.Features.Alerts
{
    public class AlertsAdapter : RecyclerView.Adapter
    {
        public event EventHandler<Alert> ItemClick;
        private List<Alert> elements;
        private Context context;

        public AlertsAdapter(Context context, IEnumerable<Alert> elements)
        {
            this.context = context;
            this.elements = elements.ToList();
        }

        public override int ItemCount => elements.Count();

        void OnClick(int position)
        {
            ItemClick?.Invoke(this, elements.ElementAt(position));
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            BlocksViewHolder vh = holder as BlocksViewHolder;
            Alert a = elements[position];
            vh.Title.Text = a.Title;
            vh.Date.Text = a.FechaNotificacion.ToString(context.GetString(Resource.String.filter_date_format)) +" - "+ a.FechaNotificacion.ToString("HH:mm");
            vh.Description.Text = a.Comment;
            if (a.Read)
                vh.Content.SetBackgroundResource(Resource.Color.colorRead);
            else
                vh.Content.SetBackgroundResource(Resource.Color.colorNotRead);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                        Inflate(Resource.Layout.alert_item, parent, false);
            BlocksViewHolder vh = new BlocksViewHolder(itemView, OnClick);
            return vh;
        }
    }

    public class BlocksViewHolder : RecyclerView.ViewHolder
    {
        public View Content { get; private set; }
        public TextView Title { get; private set; }
        public TextView Date { get; private set; }
        public TextView Description { get; private set; }

        public BlocksViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            Content = itemView.FindViewById(Resource.Id.content);
            Title = itemView.FindViewById<TextView>(Resource.Id.title);
            Date = itemView.FindViewById<TextView>(Resource.Id.date);
            Description = itemView.FindViewById<TextView>(Resource.Id.description);
            itemView.Click += (sender, e) => listener(base.LayoutPosition);
        }
    }
}
