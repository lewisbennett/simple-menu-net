using Android.Content;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using System;

namespace SimpleMenu.Droid.Views
{
    public class HeightWrappingViewPager : ViewPager
    {
        #region Properties
        public override int CurrentItem
        {
            get => base.CurrentItem;

            set
            {
                base.CurrentItem = value;

                RequestLayout();
            }
        }
        #endregion

        #region Event Handlers
        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            return false;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            return false;
        }
        #endregion

        #region Lifecycle
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            try
            {
                var wrapHeight = MeasureSpec.GetMode(heightMeasureSpec) == MeasureSpecMode.AtMost;

                if (wrapHeight)
                {
                    var child = GetChildAt(CurrentItem);

                    if (child != null)
                    {
                        child.Measure(widthMeasureSpec, MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));

                        var childHeight = child.MeasuredHeight;

                        heightMeasureSpec = MeasureSpec.MakeMeasureSpec(childHeight, MeasureSpecMode.Exactly);
                    }
                }
            }
            catch (Exception)
            {
                // Do nothing.
            }

            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }
        #endregion

        #region Constructors
        public HeightWrappingViewPager(Context context)
            : base(context)
        {
        }

        public HeightWrappingViewPager(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        public HeightWrappingViewPager(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
        #endregion
    }
}