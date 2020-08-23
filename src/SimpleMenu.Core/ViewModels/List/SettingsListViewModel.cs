using MvvmCross.Commands;
using MvvmCross.ViewModels;
using SimpleMenu.Core.Models;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.Schema;
using SimpleMenu.Core.ViewModels.Base;

namespace SimpleMenu.Core.ViewModels.List
{
    public partial class SettingsListViewModel : BaseViewModel
    {
        #region Event Handlers
        private void Item_Click(TextIconModel item)
        {
            if (!(item.Data is string settingId))
                return;

            switch (settingId)
            {
                case AboutAppSettingId:
                    // Navigate to about app view model.
                    return;

                case ProfileSettingId:
                    // Navigate to profile view model.
                    return;

                case TimesOfDaySettingId:
                    // Navigate to times of day view model.
                    return;

                default:
                    return;
            }
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            ItemClickCommand = new MvxCommand<TextIconModel>(Item_Click);

            AccountSettings = new MvxObservableCollection<TextIconModel>
            {
                new TextIconModel { Title = Resources.TitleProfile, Icon = Icon.AccountCircle, Data = ProfileSettingId }
            };

            AppSettings = new MvxObservableCollection<TextIconModel>
            {
                new TextIconModel { Title = Resources.TitleAboutApp, Icon = Icon.DeviceInformation, Data = AboutAppSettingId }
            };

            ConfigurationSettings = new MvxObservableCollection<TextIconModel>
            {
                new TextIconModel { Title = Resources.TitleTimesOfDay, Icon = Icon.Timelapse, Data = TimesOfDaySettingId }
            };

            Title = Resources.TitleSettings;
        }
        #endregion

        #region Constant Values
        public const string AboutAppSettingId = "about_app";
        public const string TimesOfDaySettingId = "times_of_day";
        public const string ProfileSettingId = "profile";
        #endregion
    }
}
