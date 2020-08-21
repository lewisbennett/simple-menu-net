using Android.OS;
using Android.Support.V7.Widget;
using SimpleMenu.Core.ViewModels.List.Base;
using SimpleMenu.Droid.Activities.Base;
using SimpleMenu.Droid.Views;
using System;

namespace SimpleMenu.Droid.Activities.List.Base
{
    public abstract class ListBaseActivity<TViewModel> : BaseActivity<TViewModel>
        where TViewModel : ListBaseViewModel
    {
        #region Properties
        /// <summary>
        /// Gets this activity's recycler view.
        /// </summary>
        public RecyclerView RecyclerView { get; private set; }
        #endregion

        #region Event Handlers
        private void ExtendedMvxRecyclerView_ScrolledToNextPageTrigger(object sender, EventArgs e)
        {
            ViewModel?.LoadNextPage();
        }
        #endregion

        #region Protected Methods
        protected override void AddEventHandlers()
        {
            base.AddEventHandlers();

            if (RecyclerView is ExtendedMvxRecyclerView extendedMvxRecyclerView)
            {
                extendedMvxRecyclerView.ScrolledToNextPageTrigger += ExtendedMvxRecyclerView_ScrolledToNextPageTrigger;

                if (extendedMvxRecyclerView is ElevationMvxRecyclerView elevationMvxRecyclerView)
                    elevationMvxRecyclerView.RegisterElevationView(AppBarLayout);
            }
        }

        protected override void RemoveEventHandlers()
        {
            base.RemoveEventHandlers();

            if (RecyclerView is ExtendedMvxRecyclerView extendedMvxRecyclerView)
            {
                extendedMvxRecyclerView.ScrolledToNextPageTrigger -= ExtendedMvxRecyclerView_ScrolledToNextPageTrigger;

                if (extendedMvxRecyclerView is ElevationMvxRecyclerView elevationMvxRecyclerView)
                    elevationMvxRecyclerView.UnregisterElevationView(AppBarLayout);
            }
        }

        protected abstract RecyclerView.LayoutManager CreateLayoutManager();
        #endregion

        #region Lifecycle
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerview);

            RecyclerView.Setup(CreateLayoutManager());
        }
        #endregion
    }
}