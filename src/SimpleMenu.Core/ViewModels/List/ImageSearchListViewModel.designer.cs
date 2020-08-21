using MvvmCross.Commands;

namespace SimpleMenu.Core.ViewModels.List
{
    partial class ImageSearchListViewModel
    {
        #region Fields
        private IMvxCommand _helpButtonClickCommand;
        #endregion

        #region Properties
        public IMvxCommand HelpButtonClickCommand
        {
            get => _helpButtonClickCommand;

            set => SetProperty(ref _helpButtonClickCommand, value);
        }
        #endregion
    }
}
