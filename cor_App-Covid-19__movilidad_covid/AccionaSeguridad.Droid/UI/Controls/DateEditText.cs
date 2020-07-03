using System;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace AccionaSeguridad.Droid.UI.Controls
{
    public interface IDateEditTextListener
    {
        void DateSet();
    }

    [Register("views.DateEditText")]
    public class DateEditText : EditText
    {
        private DateTime? date;
        public DateTime? Date {
            get
            {
                return date;
            }
            set
            {
                date = value;
                if (value == null)
                    Text = "";
                else
                    Text = date?.ToString(Context.Resources.GetString(Resource.String.filter_date_format));
            }
        }
        
        public DateTime? MaxDate;
        public DateTime? MinDate;

        private FragmentManager fragmentManager;
        private IDateEditTextListener dateEditTextListener;

        public DateEditText(Context context) : base(context)
        {
            Init();
        }

        public DateEditText(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        public DateEditText(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init();
        }

        public DateEditText(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init();
        }

        protected DateEditText(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
         
        }

        public void SetDateEditTextListener(IDateEditTextListener listener)
        {
            dateEditTextListener = listener;
        }

        private void Init()
        {
            Touch += async (o, e) =>
            {
                e.Handled = false;
                if (e.Event.Action == MotionEventActions.Up)
                {
                    if(Enabled)
                        ShowDialog();
                    e.Handled = true;
                }
            };

            FocusChange += (o, e) => {
                if (e.HasFocus)
                {
                    FocusableInTouchMode = false;
                    Focusable = false;
                    FocusableInTouchMode = true;
                    Focusable = true;
                    if (Enabled)
                        ShowDialog();
                }
            };
        }

        public void SetFragmentManager(FragmentManager fragmentManager)
        {
            this.fragmentManager = fragmentManager;
        }

        private void ShowDialog()
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                Date = time;
                Text = time.ToString(Context.Resources.GetString(Resource.String.filter_date_format));
                dateEditTextListener?.DateSet();
            });
            frag.CurrentDate = Date;
            frag.MaxDate = MaxDate;
            frag.MinDate = MinDate;
            if (fragmentManager != null)
                frag.Show(fragmentManager, DatePickerFragment.TAG);
            else
                throw new Exception("You must set Fragment manager manually");
        }
    }

    public class DatePickerFragment : DialogFragment,
                                  Android.App.DatePickerDialog.IOnDateSetListener
    {
        public static readonly string TAG = "X:" + typeof(DatePickerFragment).Name.ToUpper();

        // Initialize this value to prevent NullReferenceExceptions.
        Action<DateTime> _dateSelectedHandler = delegate { };
        public DateTime? CurrentDate;
        public DateTime? MaxDate;
        public DateTime? MinDate;

        public static DatePickerFragment NewInstance(Action<DateTime> onDateSelected)
        {
            DatePickerFragment frag = new DatePickerFragment();
            frag._dateSelectedHandler = onDateSelected;
            return frag;
        }

        public override Android.App.Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            if(!CurrentDate.HasValue)
                CurrentDate = DateTime.Now;
            Android.App.DatePickerDialog dialog = new Android.App.DatePickerDialog(Activity,
                                                           this,
                                                           CurrentDate.Value.Year,
                                                           CurrentDate.Value.Month - 1,                 
                                                           CurrentDate.Value.Day);
            DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            if (MaxDate.HasValue)
            {
                dialog.DatePicker.MaxDate = new Java.Util.Date(MaxDate.Value.Year - 1900, MaxDate.Value.Month - 1, MaxDate.Value.Day).Time;
            }
            if (MinDate.HasValue)
            {
                dialog.DatePicker.MinDate = new Java.Util.Date(MinDate.Value.Year - 1900, MinDate.Value.Month - 1, MinDate.Value.Day).Time;
            }
            return dialog;
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            // Note: monthOfYear is a value between 0 and 11, not 1 and 12!
            DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
            _dateSelectedHandler(selectedDate);
        }
    }
}