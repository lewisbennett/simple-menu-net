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
        #region Lifecycle
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var mode = MeasureSpec.GetMode(heightMeasureSpec);

            if (mode == MeasureSpecMode.Unspecified || mode == MeasureSpecMode.AtMost)
            {
                base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

                var height = 0;

                for (var i = 0; i < ChildCount; i++)
                {
                    var childView = GetChildAt(i);

                    childView.Measure(widthMeasureSpec, MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));

                    var childViewMeasuredHeight = childView.MeasuredHeight;

                    if (childViewMeasuredHeight > height)
                        height = childViewMeasuredHeight;
                }

                heightMeasureSpec = MeasureSpec.MakeMeasureSpec(height, MeasureSpecMode.Exactly);
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