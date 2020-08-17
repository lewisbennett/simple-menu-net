using Android.OS;
using Android.Support.Design.Button;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Widget;
using SimpleMenu.Core.ViewModels.Base;
using SimpleMenu.Droid.Adapters;
using SimpleMenu.Droid.Views;
using System;
using V4_Fragment = Android.Support.V4.App.Fragment;

namespace SimpleMenu.Droid.Activities.Base
{
    public abstract class CreateThingBaseActivity<TViewModel> : BaseActivity<TViewModel>
        where TViewModel : CreateThingBaseViewModel
    {
        #region Fields
        private V4_Fragment[] _fragments;
        #endregion

        #region Properties
        /// <summary>
        /// Gets this activity's finish button.
        /// </summary>
        public MaterialButton FinishButton { get; private set; }

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
        private void FinishButton_Click(object sender, EventArgs e)
        {
            ViewModel.CreateThingAndClose();
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            ViewPager.SetCurrentItem(ViewPager.CurrentItem + 1, true);
        }

        private void ViewPager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            ViewModel.ShowNextButton = e.Position != _fragments.Length - 1;
        }
        #endregion

        #region Protected Methods
        protected override void AddEventHandlers()
        {
            base.AddEventHandlers();

            FinishButton.Click += FinishButton_Click;
            NextButton.Click += NextButton_Click;
            ViewPager.PageSelected += ViewPager_PageSelected;

            if (ScrollView is ElevationScrollView elevationScrollView)
                elevationScrollView.RegisterElevationView(AppBarLayout);
        }

        protected abstract V4_Fragment[] CreateFragments();

        protected override void RemoveEventHandlers()
        {
            base.RemoveEventHandlers();

            FinishButton.Click -= FinishButton_Click;
            NextButton.Click -= NextButton_Click;
            ViewPager.PageSelected -= ViewPager_PageSelected;

            if (ScrollView is ElevationScrollView elevationScrollView)
                elevationScrollView.UnregisterElevationView(AppBarLayout);
        }
        #endregion

        #region Lifecycle
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            FinishButton = FindViewById<MaterialButton>(Resource.Id.finishbutton);
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