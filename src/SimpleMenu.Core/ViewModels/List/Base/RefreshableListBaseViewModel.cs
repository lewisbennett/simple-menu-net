using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace SimpleMenu.Core.ViewModels.List.Base
{
    public abstract partial class RefreshableListBaseViewModel<TModel> : ListBaseViewModel<TModel>
        where TModel : class
    {
        #region Event Handlers
        private async void SwipeRefresh_Refresh()
        {
            // ShowRefreshing isn't updated when the user swipes to refresh so set the correct value here so that it can be correctly updated later.
            ShowRefreshing = true;

            if (IsLoading)
            {
                ShowRefreshing = false;
                return;
            }

            IsLoading = true;

            await LoadInitialPageAsync().ConfigureAwait(false);

            IsLoading = ShowRefreshing = false;
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            RefreshCommand = new MvxCommand(SwipeRefresh_Refresh);
        }
        #endregion
    }

    public abstract class RefreshableListBaseViewModel<TModel, TNavigationParams> : RefreshableListBaseViewModel<TModel>, IMvxViewModel<TNavigationParams>
        where TModel : class
        where TNavigationParams : class
    {
        #region Lifecycle
        public virtual void Prepare(TNavigationParams parameter)
        {
        }
        #endregion
    }
}
