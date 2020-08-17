using Android.Support.V7.Widget;
using MvvmCross.Droid.Support.V7.RecyclerView;
using SimpleMenu.Droid.Helper;
using SimpleMenu.Droid.TemplateSelectors;

namespace SimpleMenu.Droid
{
    public static class Extensions
    {
        #region Public Methods
        /// <summary>
        /// Applies the correct item template selector and layout manager.
        /// </summary>
        public static void Setup<TLayoutManager>(this RecyclerView recyclerView)
            where TLayoutManager : RecyclerView.LayoutManager
        {
            if (recyclerView is MvxRecyclerView mvxRecyclerView)
                mvxRecyclerView.ItemTemplateSelector = new BaseModelItemTemplateSelector();

            if (typeof(TLayoutManager) == typeof(LinearLayoutManager))
                recyclerView.SetLayoutManager(new GridLayoutManager(recyclerView.Context, DimensionHelper.ListViewHorizontalCount));

            else if (typeof(TLayoutManager) == typeof(GridLayoutManager))
                recyclerView.SetLayoutManager(new GridLayoutManager(recyclerView.Context, DimensionHelper.GridViewHorizontalCount));

            else if (typeof(TLayoutManager) == typeof(StaggeredGridLayoutManager))
                recyclerView.SetLayoutManager(new StaggeredGridLayoutManager(DimensionHelper.StaggeredViewHorizontalCount, StaggeredGridLayoutManager.Vertical));
        }
        #endregion
    }
}