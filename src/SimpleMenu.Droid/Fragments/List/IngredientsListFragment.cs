using Android.Support.V7.Widget;
using Android.Views;
using SimpleMenu.Core.ViewModels.List;
using SimpleMenu.Droid.Attributes;
using SimpleMenu.Droid.Fragments.List.Base;
using SimpleMenu.Droid.Helper;

namespace SimpleMenu.Droid.Fragments.List
{
    [FragmentLayout(LayoutResourceID = Resource.Layout.frag_refreshable_list, MenuResourceID = Resource.Menu.menu_ingredient_list)]
    public class IngredientsListFragment : ListBaseFragment<GridLayoutManager>
    {
        #region Event Handlers
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_create_ingredient:

                    if (ViewModel is IngredientsListViewModel ingredientListViewModel)
                        ingredientListViewModel.NavigateToCreateIngredientViewModel();

                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
        #endregion

        #region Protected Methods
        protected override RecyclerView.LayoutManager CreateLayoutManager()
            => new StaggeredGridLayoutManager(DimensionHelper.StaggeredViewHorizontalCount, StaggeredGridLayoutManager.Vertical);
        #endregion
    }
}