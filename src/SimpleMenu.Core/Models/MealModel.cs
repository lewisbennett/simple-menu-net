using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Models.Base;
using SimpleMenu.Core.Properties;
using System.ComponentModel;

namespace SimpleMenu.Core.Models
{
    public class MealModel : EntityDisplayBaseModel<MealEntity>
    {
        #region Fields
        private string _description = string.Empty, _title = string.Empty;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the subtitle;
        /// </summary>
        public string Description
        {
            get => _description;

            set
            {
                value ??= string.Empty;

                if (_description.Equals(value))
                    return;

                _description = value;

                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(ShowDescription));
            }
        }

        /// <summary>
        /// Gets the image for this meal.
        /// </summary>
        public byte[] Image => Entity.GetImage();

        /// <summary>
        /// Gets whether the description should be shown.
        /// </summary>
        public bool ShowDescription => !string.IsNullOrWhiteSpace(Description);

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get => _title;

            set
            {
                value ??= string.Empty;

                if (_title.Equals(value))
                    return;

                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        #endregion

        #region Event Handlers
        protected override void OnEntityChanged()
        {
            base.OnEntityChanged();

            Title = CalculateTitle();
            Description = CalculateDescription();

            OnPropertyChanged(nameof(Image));
        }

        protected override void OnEntityPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnEntityPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case nameof(MealEntity.ImageUUID):
                    OnPropertyChanged(nameof(Image));
                    return;

                case nameof(MealEntity.Name):
                    Title = CalculateTitle();
                    return;

                case nameof(MealEntity.PreparationTime):
                    Description = CalculateDescription();
                    return;

                default:
                    return;
            }
        }
        #endregion

        #region Private Methods
        public string CalculateDescription()
        {
            if (Entity.PreparationTime.Hours < 1 && Entity.PreparationTime.Minutes < 1)
                return string.Empty;

            var minutes = string.Format(Entity.PreparationTime.Minutes == 1 ? Resources.HintMinute : Resources.HintMinutes, Entity.PreparationTime.Minutes.ToString());

            if (Entity.PreparationTime.Hours < 1)
                return $"{minutes}.";

            var hours = string.Format(Entity.PreparationTime.Hours == 1 ? Resources.HintHour : Resources.HintHours, Entity.PreparationTime.Hours.ToString());

            return Entity.PreparationTime.Minutes > 0 ? $"{hours}, {minutes}." : $"{hours}.";
        }

        private string CalculateTitle()
        {
            return Entity.Name;
        }
        #endregion
    }
}
