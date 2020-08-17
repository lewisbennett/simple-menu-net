using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using SimpleMenu.Core.ViewModels.Base;
using SimpleMenu.Droid.Attributes;
using System.Reflection;

namespace SimpleMenu.Droid.Activities.Base
{
    public abstract class BaseActivity<TViewModel> : MvxAppCompatActivity<TViewModel>
        where TViewModel : BaseViewModel
    {
        #region Properties
        /// <summary>
        /// Gets this activity's app bar layout, if any.
        /// </summary>
        public AppBarLayout AppBarLayout { get; private set; }

        /// <summary>
        /// Gets or sets this activity's toolbar, if any.
        /// </summary>
        public Toolbar Toolbar { get; private set; }
        #endregion

        #region Event Handlers
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    OnBackPressed();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the activity's layout attribute, if any.
        /// </summary>
        public ActivityLayoutAttribute GetLayoutAttribute() => GetType().GetCustomAttribute<ActivityLayoutAttribute>(true);
        #endregion

        #region Protected Methods
        protected virtual void AddEventHandlers()
        {
            ViewModel?.AddEventHandlers();
        }

        protected virtual void RemoveEventHandlers()
        {
            ViewModel?.RemoveEventHandlers();
        }
        #endregion

        #region Lifecycle
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView();
        }

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);

            SetContentView();
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            AddEventHandlers();
        }

        protected void SetContentView()
        {
            var layoutAttribute = GetLayoutAttribute();

            if (layoutAttribute == null)
                return;

            SetContentView(layoutAttribute.LayoutResourceID);

            if (layoutAttribute.EnableBackButton)
                SupportActionBar?.SetDisplayHomeAsUpEnabled(true);
        }

        public override void SetContentView(View view)
        {
            base.SetContentView(view);

            GetViews();
        }

        public override void SetContentView(int layoutResId)
        {
            base.SetContentView(layoutResId);

            GetViews();
        }

        public override void SetContentView(View view, ViewGroup.LayoutParams @params)
        {
            base.SetContentView(view, @params);

            GetViews();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var layoutAttribute = GetLayoutAttribute();

            if (SupportActionBar != null && layoutAttribute != null && layoutAttribute.MenuResourceID != 0)
                MenuInflater.Inflate(layoutAttribute.MenuResourceID, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            RemoveEventHandlers();
        }
        #endregion

        #region Private Methods
        private void GetViews()
        {
            AppBarLayout = FindViewById<AppBarLayout>(Resource.Id.appbar);
            Toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            if (Toolbar != null)
                SetSupportActionBar(Toolbar);
        }
        #endregion
    }
}