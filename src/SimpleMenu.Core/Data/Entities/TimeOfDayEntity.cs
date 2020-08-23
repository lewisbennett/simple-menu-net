using MvvmCross.ViewModels;
using System;
using System.Text.Json.Serialization;

namespace SimpleMenu.Core.Data.Entities
{
    public class TimeOfDayEntity : MvxNotifyPropertyChanged
    {
        #region Fields
        private bool _canDelete;
        private string _name;
        private long _timeOfDay;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets whether this time of day can be deleted.
        /// </summary>
        [JsonPropertyName("canDelete")]
        public bool CanDelete
        {
            get => _canDelete;

            set => SetProperty(ref _canDelete, value);
        }

        /// <summary>
        /// Gets or sets the name of the time of day.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name
        {
            get => _name;

            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// Gets or sets the time of day (in ticks).
        /// </summary>
        [JsonPropertyName("timeOfDay")]
        public long TimeOfDay
        {
            get => _timeOfDay;

            set => SetProperty(ref _timeOfDay, value);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the time of day as a <see cref="TimeSpan" />.
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTimeOfDay()
            => TimeSpan.FromTicks(TimeOfDay);
        #endregion
    }
}
