using MvvmCross.Commands;

namespace SimpleMenu.Core.ViewModels.List
{
    partial class MenuListViewModel
    {
        #region Fields
        private IMvxCommand _addMenuButtonClickCommand;
        #endregion

        #region Properties
        public IMvxCommand AddMenuButtonClickCommand
        {
            get => _addMenuButtonClickCommand;

            set => SetProperty(ref _addMenuButtonClickCommand, value);
        }
        #endregion
    }
}
