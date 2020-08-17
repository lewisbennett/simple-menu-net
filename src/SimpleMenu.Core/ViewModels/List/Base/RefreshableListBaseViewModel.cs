using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace SimpleMenu.Core.ViewModels.List.Base
{
    public abstract class RefreshableListBaseViewModel<TModel> : ListBaseViewModel<TModel>
        where TModel : class
    {
        #region Fields
        private bool _showRefreshing;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the command triggered when the user swipes to refresh.
        /// </summary>
        public IMvxCommand RefreshCommand { get; private set; }

        /// <summary>
        /// Gets or sets whether or not data is currently being refreshed.
        /// </summary>
        public bool ShowRefreshing
        {
            get => _showRefreshing;

            set
            {
                if (_showRefreshing == value)
                    return;

                _showRefreshing = value;

                RaisePropertyChanged(() => ShowRefreshing);
                RaisePropertyChanged(() => MiddleMessage);
            }
        }
        #endregion

        #region Event Handlers
        protected virtual async void OnRefresh()
        {
            if (IsLoading)
                return;

            IsLoading = true;
            ShowRefreshing = true;

            await LoadInitialPageAsync().ConfigureAwait(false);

            IsLoading = false;
            ShowRefreshing = false;
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            RefreshCommand = new MvxCommand(OnRefresh);
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
