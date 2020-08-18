using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace SimpleMenu.Core.ViewModels.List.Base
{
    partial class ListBaseViewModel
    {
        #region Fields
        private IMvxCommand _dataEmptyActionButtonClickCommand;
        private string _dataEmptyActionButtonText, _dataEmptyHint, _loadingHint, _middleMessage;
        private bool _isDataEmpty, _showDataEmptyAction, _showLoading;
        #endregion

        #region Properties
        public IMvxCommand DataEmptyActionButtonClickCommand
        {
            get => _dataEmptyActionButtonClickCommand;

            set => SetProperty(ref _dataEmptyActionButtonClickCommand, value);
        }

        public string DataEmptyActionButtonText
        {
            get => _dataEmptyActionButtonText;

            set => SetProperty(ref _dataEmptyActionButtonText, value);
        }

        public string DataEmptyHint
        {
            get => _dataEmptyHint;

            set => SetProperty(ref _dataEmptyHint, value);
        }

        public bool IsDataEmpty
        {
            get => _isDataEmpty;

            set => SetProperty(ref _isDataEmpty, value);
        }

        public string LoadingHint
        {
            get => _loadingHint;

            set => SetProperty(ref _loadingHint, value);
        }

        public string MiddleMessage
        {
            get => _middleMessage;

            set => SetProperty(ref _middleMessage, value);
        }

        public bool ShowDataEmptyAction
        {
            get => _showDataEmptyAction;

            set => SetProperty(ref _showDataEmptyAction, value);
        }

        public bool ShowLoading
        {
            get => _showLoading;

            set => SetProperty(ref _showLoading, value);
        }
        #endregion
    }

    partial class ListBaseViewModel<TModel>
    {
        #region Fields
        private MvxObservableCollection<TModel> _data;
        private IMvxCommand _itemClickCommand, _itemLongClickCommand;
        #endregion

        #region Properties
        public MvxObservableCollection<TModel> Data
        {
            get => _data;

            set => SetProperty(ref _data, value);
        }

        public IMvxCommand ItemClickCommand
        {
            get => _itemClickCommand;

            set => SetProperty(ref _itemClickCommand, value);
        }

        public IMvxCommand ItemLongClickCommand
        {
            get => _itemLongClickCommand;

            set => SetProperty(ref _itemLongClickCommand, value);
        }
        #endregion
    }
}
