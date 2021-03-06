﻿using MvvmCross.Commands;

namespace SimpleMenu.Core.ViewModels.List
{
    partial class MenuMealListViewModel
    {
        #region Fields
        private IMvxCommand _addMealButtonClickCommand, _regenerateMenuButtonClickCommand;
        private bool _criteriaMet, _showNextButton;
        #endregion

        #region Properties
        public IMvxCommand AddMealButtonClickCommand
        {
            get => _addMealButtonClickCommand;

            set => SetProperty(ref _addMealButtonClickCommand, value);
        }

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
