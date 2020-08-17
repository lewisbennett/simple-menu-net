using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using SimpleMenu.Core.ViewModels;
using SimpleMenu.Core.ViewModels.Base;
using SimpleMenu.Droid.Activities.Base;
using SimpleMenu.Droid.Attributes;
using SimpleMenu.Droid.Fragments;
using SimpleMenu.Droid.Fragments.List;
using System;
using System.Collections.Generic;
using System.Linq;
using V4_Fragment = Android.Support.V4.App.Fragment;

namespace SimpleMenu.Droid.Activities
{
    [MvxActivityPresentation]
    [Activity(Label = "", WindowSoftInputMode = SoftInput.AdjustPan)]
    [ActivityLayout(EnableBackButton = true, LayoutResourceID = Resource.Layout.activity_main)]
    public class MainActivity : BaseActivity<MainViewModel>, NavigationView.IOnNavigationItemSelectedListener
    {
        #region Fields
        private V4_Fragment _currentFragment;
        private readonly Dictionary<int, V4_Fragment> _fragments = new Dictionary<int, V4_Fragment>();
        #endregion

        #region Properties
        /// <summary>
        /// Gets this activity's action bar drawer toggle.
        /// </summary>
        public ActionBarDrawerToggle ActionBarDrawerToggle { get; private set; }

        /// <summary>
        /// Gets this activity's drawer layout.
        /// </summary>
        public DrawerLayout DrawerLayout { get; private set; }

        /// <summary>
        /// Gets this activity's navigation view.
        /// </summary>
        public NavigationView NavigationView { get; private set; }
        #endregion

        #region Event Handlers
        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            ActionBarDrawerToggle.OnConfigurationChanged(newConfig);
        }

        public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            DrawerLayout.CloseDrawer(GravityCompat.Start);

            SetFragment(menuItem.ItemId, _fragments[menuItem.ItemId]);

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (ActionBarDrawerToggle.OnOptionsItemSelected(item))
                return true;

            return base.OnOptionsItemSelected(item);
        }
        #endregion

        #region Lifecycle
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawerlayout);
            NavigationView = FindViewById<NavigationView>(Resource.Id.navigationview);

            ActionBarDrawerToggle = new ActionBarDrawerToggle(this, DrawerLayout, Toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_closed)
            {
                DrawerSlideAnimationEnabled = false
            };

            DrawerLayout.AddDrawerListener(ActionBarDrawerToggle);

            NavigationView.SetNavigationItemSelectedListener(this);

            var menuIds = new[]
            {
                Resource.Id.main_navigation_meals,
                Resource.Id.main_navigation_menus,
                Resource.Id.main_navigation_ingredients,
                Resource.Id.main_navigation_temperature_calculator,
                Resource.Id.main_navigation_volume_calculator,
                Resource.Id.main_navigation_weight_calculator
            };

            if (bundle != null)
            {
                foreach (var menuId in menuIds)
                {
                    var existingFragmentTag = bundle.GetString(menuId.ToString());

                    if (!string.IsNullOrWhiteSpace(existingFragmentTag))
                        _fragments[menuId] = SupportFragmentManager.FindFragmentByTag(existingFragmentTag);
                }
            }

            foreach (var menuId in menuIds)
            {
                if (_fragments.TryGetValue(menuId, out _))
                    continue;

                _fragments[menuId] = menuId switch
                {
                    Resource.Id.main_navigation_meals => new MealListFragment { ViewModel = ViewModel.MealListViewModel },
                    Resource.Id.main_navigation_menus => new MenuListFragment { ViewModel = ViewModel.MenuListViewModel },
                    Resource.Id.main_navigation_ingredients => new IngredientsListFragment { ViewModel = ViewModel.IngredientsListViewModel },
                    Resource.Id.main_navigation_temperature_calculator => new TemperatureCalculatorFragment { ViewModel = ViewModel.TemperatureCalculatorViewModel },
                    Resource.Id.main_navigation_volume_calculator => new VolumeCalculatorFragment { ViewModel = ViewModel.VolumeCalculatorViewModel },
                    Resource.Id.main_navigation_weight_calculator => new WeightCalculatorFragment { ViewModel = ViewModel.WeightCalculatorViewModel },
                    _ => null
                };
            }

            if (bundle != null)
            {
                var menuItemId = bundle.GetInt(CurrentFragmentBundleKey);

                if (_fragments.TryGetValue(menuItemId, out V4_Fragment fragment))
                    SetFragment(menuItemId, fragment);
            }
            else
            {
                var firstFragmentPair = _fragments.First();

                SetFragment(firstFragmentPair.Key, firstFragmentPair.Value);
            }
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            ActionBarDrawerToggle.SyncState();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            foreach (var fragmentPair in _fragments)
            {
                if (!string.IsNullOrWhiteSpace(fragmentPair.Value.Tag))
                    outState.PutString(fragmentPair.Key.ToString(), fragmentPair.Value.Tag);
            }

            outState.PutInt(CurrentFragmentBundleKey, NavigationView.CheckedItem.ItemId);
        }
        #endregion

        #region Private Methods
        private void SetFragment(int menuItemId, V4_Fragment fragment)
        {
            if (_currentFragment == fragment || fragment.IsVisible)
                return;

            var transaction = SupportFragmentManager.BeginTransaction();

            if (string.IsNullOrWhiteSpace(fragment.Tag))
                transaction.Add(Resource.Id.framelayout, fragment, Guid.NewGuid().ToString());
            else
            {
                var existingFragment = SupportFragmentManager.FindFragmentByTag(fragment.Tag);

                fragment = _fragments[menuItemId] = existingFragment;
            }

            foreach (var fragmentPair in _fragments)
            {
                if (fragmentPair.Value != fragment)
                    transaction.Hide(fragmentPair.Value);
            }

            transaction.Show(fragment).Commit();

            NavigationView.SetCheckedItem(menuItemId);

            if (fragment is MvxFragment mvxFragment && mvxFragment.ViewModel is BaseViewModel baseViewModel)
                ViewModel.Title = baseViewModel.Title;

            _currentFragment = fragment;
        }
        #endregion

        #region Constant Values
        public const string CurrentFragmentBundleKey = "current_fragment";
        #endregion
    }
}