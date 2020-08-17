using MvvmCross.Commands;

namespace SimpleMenu.Core.ViewModels.CreateThing.Base
{
    partial class CreateThingBaseViewModel
    {
        #region Fields
        private IMvxCommand _backButtonClickCommand, _nextButtonClickCommand;
        private string _backButtonText, _nextButtonText;
        private bool _criteriaMet, _showBackButton, _showNextButton;
        #endregion

        #region Properties
        public IMvxCommand BackButtonClickCommand
        {
            get => _backButtonClickCommand;

            set => SetProperty(ref _backButtonClickCommand, value);
        }

        public string BackButtonText
        {
            get => _backButtonText;

            set => SetProperty(ref _backButtonText, value);
        }

        public bool CriteriaMet
        {
            get => _criteriaMet;

            set => SetProperty(ref _criteriaMet, value);
        }

        public IMvxCommand NextButtonClickCommand
        {
            get => _nextButtonClickCommand;

            set => SetProperty(ref _nextButtonClickCommand, value);
        }

        public string NextButtonText
        {
            get => _nextButtonText;

            set => SetProperty(ref _nextButtonText, value);
        }

        public bool ShowBackButton
        {
            get => _showBackButton;

            set => SetProperty(ref _showBackButton, value);
        }

        public bool ShowNextButton
        {
            get => _showNextButton;

            set => SetProperty(ref _showNextButton, value);
        }
        #endregion
    }
}
