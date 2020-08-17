using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using MvvmCross.Droid.Support.V7.RecyclerView;
using System;

namespace SimpleMenu.Droid.Views
{
    public class ExtendedMvxRecyclerView : MvxRecyclerView
    {
        #region Fields
        private float _nextPageTrigger = DefaultNextPageTrigger;
        #endregion

        #region Events
        public event EventHandler ScrolledToNextPageTrigger;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the current horizontal scroll position.
        /// </summary>
        public int HorizontalScrollPosition { get; private set; }

        /// <summary>
        /// Gets or sets the next page trigger (0 to 1).
        /// </summary>
        public float NextPageTrigger
        {
            get => _nextPageTrigger;

            set
            {
                if (value < 0 || value > 1)
                    return;

                _nextPageTrigger = value;
            }
        }

        /// <summary>
        /// Gets or sets the current vertical scroll position.
        /// </summary>
        public int VerticalScrollPosition { get; private set; }
        #endregion

        #region Event Handlers
        public override void OnScrolled(int dx, int dy)
        {
            base.OnScrolled(dx, dy);

            HorizontalScrollPosition += dx;
            VerticalScrollPosition += dy;

            var visiblePosition = GetLayoutManager() switch
            {
                GridLayoutManager gridLayoutManager => gridLayoutManager.FindFirstVisibleItemPosition(),
                LinearLayoutManager linearLayoutManager => linearLayoutManager.FindFirstVisibleItemPosition(),
                _ => 0
            };

            if (ChildCount + visiblePosition >= GetAdapter().ItemCount * NextPageTrigger)
                ScrolledToNextPageTrigger?.Invoke(this, new EventArgs());
        }
        #endregion

        #region Constructors
        public ExtendedMvxRecyclerView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        public ExtendedMvxRecyclerView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public ExtendedMvxRecyclerView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
        }

        public ExtendedMvxRecyclerView(Context context, IAttributeSet attrs, int defStyle, IMvxRecyclerAdapter adapter)
            : base(context, attrs, defStyle, adapter)
        {
        }
        #endregion

        #region Constant Values
        public const float DefaultNextPageTrigger = 0.8f;
        #endregion
    }
}