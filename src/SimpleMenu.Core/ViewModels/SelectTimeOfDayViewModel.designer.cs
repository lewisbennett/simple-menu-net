using MvvmCross.ViewModels;
using SimpleMenu.Core.Models;

namespace SimpleMenu.Core.ViewModels
{
    partial class SelectTimeOfDayViewModel
    {
        #region Fields
        private string _changeInSettingsHint, _loadingHint;
        private bool _criteriaMet, _showLoading, _showNextButton;
        private SpinnerModel _selectedTimeOfDay;
        private MvxObservableCollection<SpinnerModel> _timesOfDay;
        #endregion

        #region Properties
        public string ChangeInSettingsHint
        {
            get => _changeInSettingsHint;

            set => SetProperty(ref _changeInSettingsHint, value);
        }

        public bool CriteriaMet
        {
            get => _criteriaMet;

            set => SetProperty(ref _criteriaMet, value);
        }

        public string LoadingHint
        {
            get => _loadingHint;

            set => SetProperty(ref _loadingHint, value);
        }

        public bool ShowLoading
        {
            get => _showLoading;

            set => SetProperty(ref _showLoading, value);
        }

        public bool ShowNextButton
        {
            get => _showNextButton;

            set => SetProperty(ref _showNextButton, value);
        }

        public SpinnerModel SelectedTimeOfDay
        {
            get => _selectedTimeOfDay;

            set => SetProperty(ref _selectedTimeOfDay, value);
        }

        public MvxObservableCollection<SpinnerModel> TimesOfDay
        {
            get => _timesOfDay;

            set => SetProperty(ref _timesOfDay, value);
        }
        #endregion
    }
}
