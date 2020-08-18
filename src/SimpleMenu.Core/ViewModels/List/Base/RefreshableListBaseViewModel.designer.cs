using MvvmCross.Commands;

namespace SimpleMenu.Core.ViewModels.List.Base
{
    partial class RefreshableListBaseViewModel<TModel>
    {
        #region Fields
        private IMvxCommand _refreshCommand;
        private bool _showRefreshing;
        #endregion

        #region Properties
        public IMvxCommand RefreshCommand
        {
            get => _refreshCommand;

            set => SetProperty(ref _refreshCommand, value);
        }

        public bool ShowRefreshing
        {
            get => _showRefreshing;

            set => SetProperty(ref _showRefreshing, value);
        }
        #endregion
    }
}
