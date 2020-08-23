using MvvmCross.Commands;
using MvvmCross.ViewModels;
using SimpleMenu.Core.Models;

namespace SimpleMenu.Core.ViewModels.List
{
    partial class SettingsListViewModel
    {
        #region Fields
        private MvxObservableCollection<TextIconModel> _accountSettings, _appSettings, _configurationSettings;
        private IMvxCommand _itemClickCommand;
        #endregion

        #region Properties
        public MvxObservableCollection<TextIconModel> AccountSettings
        {
            get => _accountSettings;

            set => SetProperty(ref _accountSettings, value);
        }

        public MvxObservableCollection<TextIconModel> AppSettings
        {
            get => _appSettings;

            set => SetProperty(ref _appSettings, value);
        }

        public MvxObservableCollection<TextIconModel> ConfigurationSettings
        {
            get => _configurationSettings;

            set => SetProperty(ref _configurationSettings, value);
        }

        public IMvxCommand ItemClickCommand
        {
            get => _itemClickCommand;

            set => SetProperty(ref _itemClickCommand, value);
        }
        #endregion
    }
}
