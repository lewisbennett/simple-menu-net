using Android.OS;
using Android.Support.Design.Button;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using SimpleMenu.Core.ViewModels.CreateThing.Base;
using SimpleMenu.Droid.Activities.Base;
using SimpleMenu.Droid.Adapters;
using SimpleMenu.Droid.Views;
using System.ComponentModel;
using V4_Fragment = Android.Support.V4.App.Fragment;

namespace SimpleMenu.Droid.Activities.CreateThing.Base
{
    public abstract class CreateThingBaseActivity<TViewModel> : BaseActivity<TViewModel>
        where TViewModel : CreateThingBaseViewModel
    {
        #region Fields
        private V4_Fragment[] _fragments;
        #endregion

        #region Properties
        /// <summary>
        /// Gets this activity's back button.
        /// </summary>
        public MaterialButton BackButton { get; private set; }

        /// <summary>
        /// Gets this activity's next button.
        /// </summary>
        public MaterialButton NextButton { get; private set; }

        /// <summary>
        /// Gets this activity's scroll view.
        /// </summary>
        public ScrollView ScrollView { get; private set; }

        /// <summary>
        /// Gets this activity's tab layout.
        /// </summary>
        public TabLayout TabLayout { get; private set; }

        /// <summary>
        /// Gets this activity's view pager.
        /// </summary>
        public ViewPager ViewPager { get; private set; }
        #endregion

        #region Event Handlers
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.CurrentStep):

                    var index = -1;

                    for (var i = 0; i < _fragments.Length; i++)
                    {
                        if (_fragments[i] is MvxFragment mvxFragment && mvxFragment.ViewModel == ViewModel.CurrentStep)
                        {
                            index = i;
                            break;
                        }
                    }

                    if (index >= 0)
                        ViewPager.SetCurrentItem(index, true);

                    return;

                default:
                    return;
            }
        }
        #endregion

        #region Protected Methods
        protected override void AddEventHandlers()
        {
            base.AddEventHandlers();

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            if (ScrollView is ElevationScrollView elevationScrollView)
                elevationScrollView.RegisterElevationView(AppBarLayout);
        }

        protected abstract V4_Fragment[] CreateFragments();

        protected override void RemoveEventHandlers()
        {
            base.RemoveEventHandlers();

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;

            if (ScrollView is ElevationScrollView elevationScrollView)
                elevationScrollView.UnregisterElevationView(AppBarLayout);
        }
        #endregion

        #region Lifecycle
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            BackButton = FindViewById<MaterialButton>(Resource.Id.backbutton);
            NextButton = FindViewById<MaterialButton>(Resource.Id.nextbutton);
            ScrollView = FindViewById<ScrollView>(Resource.Id.scrollview);
            TabLayout = FindViewById<TabLayout>(Resource.Id.tablayout);
            ViewPager = FindViewById<ViewPager>(Resource.Id.viewpager);

            _fragments = CreateFragments();

            ViewPager.Adapter = new SimpleFragmentStatePagerAdapter(SupportFragmentManager, _fragments);

            TabLayout.SetupWithViewPager(ViewPager, true);
        }
        #endregion
    }
}