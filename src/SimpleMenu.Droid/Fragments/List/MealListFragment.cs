using Android.Support.V7.Widget;
using Android.Views;
using SimpleMenu.Core.ViewModels.List;
using SimpleMenu.Droid.Attributes;
using SimpleMenu.Droid.Fragments.List.Base;

namespace SimpleMenu.Droid.Fragments.List
{
    [FragmentLayout(LayoutResourceID = Resource.Layout.frag_refreshable_list, MenuResourceID = Resource.Menu.menu_meal_list)]
    public class MealListFragment : ListBaseFragment<StaggeredGridLayoutManager>
    {
        #region Event Handlers
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_create_meal:

                    if (ViewModel is MealListViewModel mealListViewModel)
                        mealListViewModel.NavigateToCreateMealViewModel();

                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
        #endregion
    }
}