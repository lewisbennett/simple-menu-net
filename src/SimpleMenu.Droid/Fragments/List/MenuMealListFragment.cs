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
    [FragmentLayout(LayoutResourceID = Resource.Layout.frag_list, MenuResourceID = Resource.Menu.menu_menu_meal_list)]
    public class MenuMealListFragment : ListBaseFragment<LinearLayoutManager>
    {
        #region Fields
        private CustomItemTouchHelperCallback<MenuMealModel> _customItemTouchHelperCallback;
        private ItemTouchHelper _itemTouchHelper;
        #endregion

        #region Event Handlers
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (ViewModel is MenuMealListViewModel menuMealListViewModel)
            {
                switch (item.ItemId)
                {
                    case Resource.Id.menu_add_meal:
                        menuMealListViewModel.AddMealButtonClickCommand.Execute();
                        return true;

                    case Resource.Id.menu_regenerate_menu:
                        menuMealListViewModel.RegenerateMenuButtonClickCommand.Execute();
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
            => new GridLayoutManager(Context, DimensionHelper.ListViewHorizontalCount);
        #endregion

        #region Lifecycle
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _customItemTouchHelperCallback = new CustomItemTouchHelperCallback<MenuMealModel>();
            _itemTouchHelper = new ItemTouchHelper(_customItemTouchHelperCallback);

            _itemTouchHelper.AttachToRecyclerView(RecyclerView);
        }
        #endregion
    }
}