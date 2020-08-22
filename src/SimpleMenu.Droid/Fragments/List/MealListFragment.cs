using Android.OS;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using SimpleMenu.Core.Models;
using SimpleMenu.Core.ViewModels.List;
using SimpleMenu.Droid.Attributes;
using SimpleMenu.Droid.Fragments.List.Base;
using SimpleMenu.Droid.Helper;

namespace SimpleMenu.Droid.Fragments.List
{
    [FragmentLayout(LayoutResourceID = Resource.Layout.frag_refreshable_list, MenuResourceID = Resource.Menu.menu_meal_list)]
    public class MealListFragment : ListBaseFragment<StaggeredGridLayoutManager>
    {
        #region Fields
        private CustomItemTouchHelperCallback<MealModel> _customItemTouchHelperCallback;
        private ItemTouchHelper _itemTouchHelper;
        #endregion

        #region Event Handlers
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (ViewModel is MealListViewModel mealListViewModel)
            {
                switch (item.ItemId)
                {
                    case Resource.Id.menu_create_meal:
                        mealListViewModel.NavigateToCreateMealViewModel();
                        return true;

                    default:
                        break;
                }
            }

            return base.OnOptionsItemSelected(item);
        }
        #endregion

        #region Protected Methods
        protected override RecyclerView.LayoutManager CreateLayoutManager()
            => new StaggeredGridLayoutManager(DimensionHelper.StaggeredViewHorizontalCount, StaggeredGridLayoutManager.Vertical);
        #endregion

        #region Lifecycle
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _customItemTouchHelperCallback = new CustomItemTouchHelperCallback<MealModel>();
            _itemTouchHelper = new ItemTouchHelper(_customItemTouchHelperCallback);

            _itemTouchHelper.AttachToRecyclerView(RecyclerView);
        }
        #endregion
    }
}