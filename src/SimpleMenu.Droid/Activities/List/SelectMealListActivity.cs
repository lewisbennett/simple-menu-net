using Android.App;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using SimpleMenu.Core.ViewModels.List;
using SimpleMenu.Droid.Activities.List.Base;
using SimpleMenu.Droid.Attributes;
using SimpleMenu.Droid.Helper;

namespace SimpleMenu.Droid.Activities.List
{
    [MvxActivityPresentation]
    [Activity(Label = "", WindowSoftInputMode = SoftInput.AdjustPan)]
    [ActivityLayout(EnableBackButton = true, LayoutResourceID = Resource.Layout.activity_refreshable_list)]
    public class SelectMealListActivity : ListBaseActivity<SelectMealListViewModel>
    {
        #region Protected Methods
        protected override RecyclerView.LayoutManager CreateLayoutManager()
            => new StaggeredGridLayoutManager(DimensionHelper.StaggeredViewHorizontalCount, StaggeredGridLayoutManager.Vertical);
        #endregion
    }
}