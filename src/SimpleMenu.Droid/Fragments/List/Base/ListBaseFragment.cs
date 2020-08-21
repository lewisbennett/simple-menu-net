using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using SimpleMenu.Core.ViewModels.List.Base;
using SimpleMenu.Droid.Fragments.Base;
using SimpleMenu.Droid.Views;
using System;

namespace SimpleMenu.Droid.Fragments.List.Base
{
    public abstract class ListBaseFragment<TLayoutManager> : BaseFragment
        where TLayoutManager : RecyclerView.LayoutManager
    {
        #region Properties
        /// <summary>
        /// Gets this fragment's recycler view.
        /// </summary>
        public RecyclerView RecyclerView { get; private set; }
        #endregion

        #region Event Handlers
        private void RecyclerView_ScrolledToNextPageTrigger(object sender, EventArgs e)
        {
            if (ViewModel is ListBaseViewModel listBaseViewModel)
                listBaseViewModel.LoadNextPage();
        }
        #endregion

        #region Protected Methods
        protected abstract RecyclerView.LayoutManager CreateLayoutManager();
        #endregion

        #region Lifecycle
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            RecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerview);

            RecyclerView.Setup(CreateLayoutManager());

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();

            if (RecyclerView is ExtendedMvxRecyclerView extendedMvxRecyclerView)
            {
                extendedMvxRecyclerView.ScrolledToNextPageTrigger += RecyclerView_ScrolledToNextPageTrigger;

                if (extendedMvxRecyclerView is ElevationMvxRecyclerView elevationMvxRecyclerView)
                    elevationMvxRecyclerView.RegisterElevationView(AppBarLayout);
            }
        }

        public override void OnPause()
        {
            base.OnPause();

            if (RecyclerView is ExtendedMvxRecyclerView extendedMvxRecyclerView)
            {
                extendedMvxRecyclerView.ScrolledToNextPageTrigger -= RecyclerView_ScrolledToNextPageTrigger;

                if (extendedMvxRecyclerView is ElevationMvxRecyclerView elevationMvxRecyclerView)
                    elevationMvxRecyclerView.UnregisterElevationView(AppBarLayout);
            }
        }
        #endregion
    }
}