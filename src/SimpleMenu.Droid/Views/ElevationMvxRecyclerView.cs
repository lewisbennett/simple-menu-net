using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using MvvmCross.Droid.Support.V7.RecyclerView;
using System;
using System.Collections.Generic;

namespace SimpleMenu.Droid.Views
{
    public class ElevationMvxRecyclerView : ExtendedMvxRecyclerView
    {
        #region Fields
        private readonly List<View> _elevationViews = new List<View>();
        #endregion

        #region Event Handlers
        public override void OnScrolled(int dx, int dy)
        {
            base.OnScrolled(dx, dy);

            if (_elevationViews.Count < 1)
                return;

            var selected = VerticalScrollPosition != 0;

            foreach (var view in _elevationViews)
            {
                if (view != null)
                    view.Selected = selected;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Registers a view to be elevated when this ElevationMvxRecyclerView is scrolled.
        /// </summary>
        /// <param name="view">The view to be registered.</param>
        public void RegisterElevationView(View view)
        {
            if (_elevationViews.Contains(view))
                return;

            _elevationViews.Add(view);
            view.Selected = VerticalScrollPosition != 0;
        }

        /// <summary>
        /// Unregisters a view from being elevated when this ElevationMvxRecyclerView is scrolled.
        /// </summary>
        /// <param name="view">The view to be unregistered.</param>
        public void UnregisterElevationView(View view)
        {
            if (!_elevationViews.Contains(view))
                return;

            _elevationViews.Remove(view);
            view.Selected = false;
        }
        #endregion

        #region Constructors
        public ElevationMvxRecyclerView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        public ElevationMvxRecyclerView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public ElevationMvxRecyclerView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
        }

        public ElevationMvxRecyclerView(Context context, IAttributeSet attrs, int defStyle, IMvxRecyclerAdapter adapter)
            : base(context, attrs, defStyle, adapter)
        {
        }
        #endregion
    }
}