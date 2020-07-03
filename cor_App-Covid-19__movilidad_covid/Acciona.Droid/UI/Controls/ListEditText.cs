using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using ServiceLocator;
using Acciona.Domain.Utils;
using Droid.Utils;
using Acciona.Domain.Model.Base;
using Acciona.Droid;
using Acciona.Droid.Utils;

namespace Acciona.Droid.UI.Controls
{
    [Register("views.ListEditText")]
    public class ListEditText : AppCompatEditText, View.IOnTouchListener
    {
        private List<ListableObject> objects;
        public event EventHandler<ListableObject> ItemChanged;
        private ListableObject selection = null;
        private bool dialogIsShow;

        public ListEditText(Context context) : base(context)
        {
            init();
        }

        public ListEditText(Context context, IAttributeSet attrs) : base(context, attrs)
        {

            init();
        }

        public ListEditText(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            init();
        }

        private void init()
        {
            SetOnTouchListener(this);
            FocusChange += (o, e) =>
            {
                if (e.HasFocus)
                {
                    InputMethodManager imm = (InputMethodManager)Context.GetSystemService(Context.InputMethodService);
                    imm.HideSoftInputFromWindow(WindowToken, 0);
                    FocusableInTouchMode = false;
                    Focusable = false;
                    FocusableInTouchMode = true;
                    Focusable = true;
                    ShowDialog();
                }
            };
        }

        public Boolean OnTouch(View view, MotionEvent motionEvent)
        {
            if (motionEvent.Action == MotionEventActions.Up)
            {
                ShowDialog();
                return true;
            }
            return false;
        }

        public void SetListableObjects(IEnumerable<ListableObject> objects)
        {
            this.objects = objects.ToList();
            if (objects.Count() > 0)
            {
                selection = this.objects[0];
                String strName = this.objects[0].GetListText();
                Text = strName;
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

        public ListableObject GetSelection()
        {
            return selection;
        }

        public void SetSelectionIndex(int selection)
        {
            if (objects != null && selection >= 0 && objects.Count>0 )
            {
                this.selection = objects.ElementAt(selection);
                String strName = objects.ElementAt(selection).GetListText();
                if (Looper.MyLooper() == Looper.MainLooper)
                {
                    Text = strName;
                }
                else 
                {
                    Post(() => Text = strName);
                }
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
            /*AlertDialog.Builder builderSingle = new AlertDialog.Builder(Context);
            String title = null;
            builderSingle.SetTitle(title);

            ArrayAdapter<String> arrayAdapter = new ArrayAdapter<String>(Context, Resource.Layout.item_dialog);
            if (objects == null)
                return;
            for (int i = 0; i < objects.Count(); i++)
                arrayAdapter.Add(objects.ElementAt(i));

            builderSingle.SetNegativeButton("Cancelar", (s, ev) =>
            {
                ((AlertDialog)s).Cancel();
            });

            builderSingle.SetAdapter(arrayAdapter, (o, e) =>
            {
                String strName = arrayAdapter.GetItem(e.Which);
                Text = strName;
                selection = e.Which;
                ItemChanged?.Invoke(this, e.Which);
            });
            builderSingle.Show();*/
            var activity = Locator.Current.GetService<Activity>() as AppCompatActivity;
            var fragment = ListEditTextDialogFragment.NewInstance();
            fragment.OnSelected += (o, item) =>
            {
                Text = item.GetListText();
                var oldSelection = selection;
                selection = item;
                if(!selection.GetListText().Equals(oldSelection.GetListText()))
                    ItemChanged?.Invoke(this, item);
            };
            fragment.Show(activity.SupportFragmentManager, "listedittext");
            fragment.SetObjects(objects);
            fragment.OnDismissEvent += (o, e) => dialogIsShow = false;
        }

        public class ListEditTextDialogFragment : Android.Support.V4.App.DialogFragment
        {
            public event EventHandler<ListableObject> OnSelected;
            public event EventHandler OnDismissEvent;
            private EditText editSearch;
            private View buttonCancel;
            private RecyclerView recyclerView;
            private RecyclerView.LayoutManager layoutManager;
            private ListEditTextAdapter adapter;

            private IEnumerable<ListableObject> objects;

            public static ListEditTextDialogFragment NewInstance()
            {
                ListEditTextDialogFragment fr = new ListEditTextDialogFragment();
                return fr;
            }

            public override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);
            }

            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                return inflater.Inflate(Resource.Layout.listedittext_dialogfragment, container, false);
            }

            public override void OnDismiss(IDialogInterface dialog)
            {
                OnDismissEvent?.Invoke(this,null);
                base.OnDismiss(dialog);
            }

            public override void OnViewCreated(View view, Bundle savedInstanceState)
            {
                base.OnViewCreated(view, savedInstanceState);
                editSearch = view.FindViewById<EditText>(Resource.Id.editSearch);
                buttonCancel = view.FindViewById(Resource.Id.buttonCancel);
                buttonCancel.Click += (o, e) => Dismiss();

                TextWatcher textWatcher = new TextWatcher();
                textWatcher.TextChanging += TextWatcher_TextChanging;
                editSearch.AddTextChangedListener(textWatcher);

                recyclerView = view.FindViewById<RecyclerView>(Resource.Id.rvListEditText);
                layoutManager = new LinearLayoutManager(Activity);
                recyclerView.SetLayoutManager(layoutManager);
            }

            private void TextWatcher_TextChanging(object sender, string filterSearch)
            {
                Task.Run(() =>
                {
                    if (objects == null)
                        return;
                    if (filterSearch.Trim().Length > 0)
                    {
                        var filtered = objects.Where(x => x.GetListText().IgnoreContains(filterSearch));
                        recyclerView.Post(()=> adapter.SetObjects(filtered));                        
                    }
                    else
                    {
                        recyclerView.Post(() => adapter.SetObjects(objects));
                    }
                });
            }

            public async Task SetObjects(IEnumerable<ListableObject> objects)
            {
                await Task.Delay(25); //await dialog created
                editSearch.Text = "";
                this.objects = objects;
                adapter = new ListEditTextAdapter(Context, objects);
                adapter.Click += (o, selected) => {
                    Dismiss();
                    OnSelected?.Invoke(this, selected);                    
                };
                recyclerView.SetAdapter(adapter);                
            }

            public class ListEditTextAdapter : RecyclerView.Adapter
            {
                public event EventHandler<ListableObject> Click;
                private List<ListableObject> elements;
                private Context context;

                public ListEditTextAdapter(Context context, IEnumerable<ListableObject> elements)
                {
                    this.context = context;
                    this.elements = elements.ToList();
                }

                public override int ItemCount => elements==null?0: elements.Count();

                void OnClick(int position)
                {
                    Click?.Invoke(this, elements[position]);
                }

                public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
                {
                    ListEditTextViewHolder vh = holder as ListEditTextViewHolder;
                    vh.Text.Text = elements[position].GetListText();
                }

                public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
                {
                    View itemView = LayoutInflater.From(context).
                        Inflate(Resource.Layout.item_listedittext, parent, false);
                    ListEditTextViewHolder vh = new ListEditTextViewHolder(itemView, OnClick);
                    return vh;
                }

                internal void SetObjects(IEnumerable<ListableObject> newObjects)
                {
                    this.elements = newObjects.ToList();
                    NotifyDataSetChanged();
                }

                public class ListEditTextViewHolder : RecyclerView.ViewHolder
                {
                    public TextView Text { get; private set; }
                    public ListEditTextViewHolder(View view, Action<int> listener) : base(view)
                    {
                        Text = view.FindViewById<TextView>(Resource.Id.text);
                        view.Click += (sender, e) => listener(base.LayoutPosition);                        
                    }
                }
            }
        }
    }
}