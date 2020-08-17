using Android.Content;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Util;
using Android.Views;
using System;
using System.Collections.Generic;

namespace SimpleMenu.Droid.Views
{
    public class ElevationNestedScrollView : NestedScrollView
    {
        #region Fields
        private readonly List<View> _elevationViews = new List<View>();
        #endregion

        #region Event Handlers
        private void ElevationNestedScrollView_ScrollChange(object sender, ScrollChangeEventArgs e)
        {
            if (_elevationViews.Count < 1)
                return;

            var selected = e.ScrollY != 0;

            foreach (var view in _elevationViews)
            {
                if (view != null)
                    view.Selected = selected;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Registers a view to be elevated when this ElevationNestedScrollView is scrolled.
        /// </summary>
        /// <param name="view">The view to be registered.</param>
        public void RegisterElevationView(View view)
        {
            if (_elevationViews.Contains(view))
                return;

            _elevationViews.Add(view);
            view.Selected = ScrollY != 0;
        }

        /// <summary>
        /// Unregisters a view from being elevated when this ElevationNestedScrollView is scrolled.
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
        public ElevationNestedScrollView(Context context)
            : base(context)
        {
            Initialize();
        }

        public ElevationNestedScrollView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Initialize();
        }

        public ElevationNestedScrollView(IntPtr reference, JniHandleOwnership transfer)
            : base(reference, transfer)
        {
        }

        public ElevationNestedScrollView(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
            Initialize();
        }
        #endregion

        #region Private Methods
        private void Initialize()
        {
            ScrollChange += ElevationNestedScrollView_ScrollChange;
        }
        #endregion
    }
}