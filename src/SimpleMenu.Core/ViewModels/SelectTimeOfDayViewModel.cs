using MvvmCross.ViewModels;
using SimpleMenu.Core.Data.Operations;
using SimpleMenu.Core.Models;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.ViewModels.Base;
using SimpleMenu.Core.ViewModels.CreateThing.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleMenu.Core.ViewModels
{
    public partial class SelectTimeOfDayViewModel : BaseViewModel, ICreateThingStepViewModel
    {
        #region Properties
        /// <summary>
        /// Gets whether data is currently being loaded.
        /// </summary>
        public bool IsLoading { get; private set; }
        #endregion

        #region Event Handlers
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(SelectedTimeOfDay):
                    CriteriaMet = SelectedTimeOfDay != null;
                    return;

                default:
                    return;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Loads the times of day.
        /// </summary>
        public async void LoadTimesOfDay()
        {
            if (IsLoading)
                return;

            ShowLoading = IsLoading = true;

            if (TimesOfDay.Count > 0)
                TimesOfDay.Clear();

            SelectedTimeOfDay = null;

            await LoadTimesOfDayAsync().ConfigureAwait(false);

            ShowLoading = IsLoading = false;
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            TimesOfDay = new MvxObservableCollection<SpinnerModel>();

            ShowNextButton = true;

            ChangeInSettingsHint = Resources.MessageChangeInSettings;
            LoadingHint = Resources.MessagingLoadingTimesOfDay;
            Title = Resources.MessageSelectMenuTimeOfDay;
        }

        public override void ViewAppearing()
        {
            base.ViewAppearing();

            LoadTimesOfDay();
        }
        #endregion

        #region Private Methods
        private async Task LoadTimesOfDayAsync()
        {
            var settings = await SettingsOperations.Instance.GetSettingsAsync().ConfigureAwait(false);

            var timesOfDay = new List<SpinnerModel>();

            foreach (var timeOfDay in settings.TimesOfDay)
            {
                var timeOfDayTimeSpan = timeOfDay.GetTimeOfDay();

                timesOfDay.Add(new SpinnerModel
                {
                    Text = $"{timeOfDay.Name} ({timeOfDayTimeSpan.Hours:00}:{timeOfDayTimeSpan.Minutes:00})",
                    Data = timeOfDay
                });
            }

            InvokeOnMainThread(() => TimesOfDay.AddRange(timesOfDay));
        }
        #endregion
    }
}
