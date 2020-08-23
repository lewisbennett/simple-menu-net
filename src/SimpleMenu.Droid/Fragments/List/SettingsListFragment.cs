using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using SimpleMenu.Droid.Attributes;
using SimpleMenu.Droid.Fragments.Base;
using SimpleMenu.Droid.Helper;
using SimpleMenu.Droid.Views;

namespace SimpleMenu.Droid.Fragments.List
{
    [FragmentLayout(LayoutResourceID = Resource.Layout.frag_list_settings)]
    public class SettingsListFragment : BaseFragment
    {
        #region Properties
        /// <summary>
        /// Gets the account settings recycler view.
        /// </summary>
        public RecyclerView AccountSettingsRecyclerView { get; private set; }

        /// <summary>
        /// Gets the app settings recycler view.
        /// </summary>
        public RecyclerView AppSettingsRecyclerView { get; private set; }

        /// <summary>
        /// Gets the configuration settings recycler view.
        /// </summary>
        public RecyclerView ConfigurationSettingsRecyclerView { get; private set; }

        /// <summary>
        /// Gets the nested scroll view.
        /// </summary>
        public NestedScrollView NestedScrollView { get; private set; }
        #endregion

        #region Lifecycle
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            AccountSettingsRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.accountsettingsrecyclerview);
            AppSettingsRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.appsettingsrecyclerview);
            ConfigurationSettingsRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.configurationsettingsrecyclerview);
            NestedScrollView = view.FindViewById<NestedScrollView>(Resource.Id.nestedscrollview);

            AccountSettingsRecyclerView.Setup(new GridLayoutManager(Context, DimensionHelper.ListViewHorizontalCount));
            AppSettingsRecyclerView.Setup(new GridLayoutManager(Context, DimensionHelper.ListViewHorizontalCount));
            ConfigurationSettingsRecyclerView.Setup(new GridLayoutManager(Context, DimensionHelper.ListViewHorizontalCount));

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();

            if (NestedScrollView is ElevationNestedScrollView elevationNestedScrollView)
                elevationNestedScrollView.RegisterElevationView(AppBarLayout);
        }

        public override void OnPause()
        {
            base.OnPause();

            if (NestedScrollView is ElevationNestedScrollView elevationNestedScrollView)
                elevationNestedScrollView.UnregisterElevationView(AppBarLayout);
        }
        #endregion
    }
}