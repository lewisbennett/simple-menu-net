using SimpleMenu.Core.Data.Entities.Base;
using SimpleMenu.Core.Data.Operations;
using SimpleMenu.Core.Properties;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimpleMenu.Core.Data.Entities
{
    public class SettingsEntity : BaseEntity
    {
        #region Fields
        private TimeOfDayEntity[] _timesOfDay;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the times of day, if any.
        /// </summary>
        [JsonPropertyName("timesOfDay")]
        public TimeOfDayEntity[] TimesOfDay
        {
            get => _timesOfDay;

            set => SetProperty(ref _timesOfDay, value);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Saves the entity.
        /// </summary>
        public override Task SaveAsync()
            => SettingsOperations.Instance.SaveSettingsAsync(this);
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Gets the default times of day.
        /// </summary>
        public static TimeOfDayEntity[] GetDefaultTimesOfDay()
        {
            return new[]
            {
                new TimeOfDayEntity { Name = Resources.TitleBreakfast, TimeOfDay = TimeSpan.FromHours(8).Ticks },
                new TimeOfDayEntity { Name = Resources.TitleLunch, TimeOfDay = TimeSpan.FromHours(13).Ticks },
                new TimeOfDayEntity { Name = Resources.TitleDinner, TimeOfDay = TimeSpan.FromHours(18).Ticks }
            };
        }
        #endregion
    }
}
