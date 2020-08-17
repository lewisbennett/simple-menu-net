using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace SimpleMenu.Droid.Views
{
    public class ElevationScrollView : ScrollView
    {
        #region Fields
        private readonly List<View> _elevationViews = new List<View>();
        #endregion

        #region Event Handlers
        private void ElevationScrollView_ScrollChange(object sender, ScrollChangeEventArgs e)
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
        /// Registers a view to be elevated when this ElevationScrollView is scrolled.
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
        /// Unregisters a view from being elevated when this ElevationScrollView is scrolled.
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
        public ElevationScrollView(Context context)
            : base(context)
        {
            Initialize();
        }

        public ElevationScrollView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Initialize();
        }

        public ElevationScrollView(IntPtr reference, JniHandleOwnership transfer)
            : base(reference, transfer)
        {
        }

        public ElevationScrollView(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
            Initialize();
        }

        public ElevationScrollView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Initialize();
        }
        #endregion

        #region Private Methods
        private void Initialize()
        {
            ScrollChange += ElevationScrollView_ScrollChange;
        }
        #endregion
    }
}