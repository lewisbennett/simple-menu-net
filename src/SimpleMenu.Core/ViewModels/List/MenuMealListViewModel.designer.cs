using MvvmCross.Commands;

namespace SimpleMenu.Core.ViewModels.List
{
    partial class MenuMealListViewModel
    {
        #region Fields
        private bool _criteriaMet, _showNextButton;
        private IMvxCommand _regenerateMenuButtonClickCommand;
        #endregion

        #region Properties
        public bool CriteriaMet
        {
            get => _criteriaMet;

            set => SetProperty(ref _criteriaMet, value);
        }

        public IMvxCommand RegenerateMenuButtonClickCommand
        {
            get => _regenerateMenuButtonClickCommand;

            set => SetProperty(ref _regenerateMenuButtonClickCommand, value);
        }

        public bool ShowNextButton
        {
            get => _showNextButton;

            set => SetProperty(ref _showNextButton, value);
        }
        #endregion
    }
}
