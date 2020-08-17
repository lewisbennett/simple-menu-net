using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using SimpleMenu.Core.ViewModels.Base;
using SimpleMenu.Droid.Attributes;
using System.Reflection;

namespace SimpleMenu.Droid.Fragments.Base
{
    public abstract class BaseFragment : MvxFragment
    {
        #region Properties
        /// <summary>
        /// Gets the activity's app bar layout, if any.
        /// </summary>
        public AppBarLayout AppBarLayout { get; private set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the fragment's layout attribute, if any.
        /// </summary>
        public FragmentLayoutAttribute GetLayoutAttribute() => GetType().GetCustomAttribute<FragmentLayoutAttribute>(true);
        #endregion

        #region Lifecycle
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            HasOptionsMenu = GetLayoutAttribute().MenuResourceID != 0;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            AppBarLayout = Activity.FindViewById<AppBarLayout>(Resource.Id.appbar);

            return this.BindingInflate(GetLayoutAttribute().LayoutResourceID, null, false);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);

            menu.Clear();

            if (HasOptionsMenu)
                inflater.Inflate(GetLayoutAttribute().MenuResourceID, menu);
        }

        public override void OnResume()
        {
            base.OnResume();

            if (ViewModel is BaseViewModel baseViewModel)
                baseViewModel.AddEventHandlers();
        }

        public override void OnPause()
        {
            base.OnPause();

            if (ViewModel is BaseViewModel baseViewModel)
                baseViewModel.AddEventHandlers();
        }
        #endregion
    }
}