using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Models.Base;
using SimpleMenu.Core.Properties;

namespace SimpleMenu.Core.Models
{
    public partial class MealModel : EntityDisplayBaseModel<MealEntity>
    {
        #region Event Handlers
        protected override void OnEntityChanged()
        {
            base.OnEntityChanged();

            Description = CalculateDescription();
            Image = CalculateImage();
            Title = CalculateTitle();
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            base.OnEntityPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(Entity.ImageUUID):
                    Image = CalculateImage();
                    return;

                case nameof(Entity.Name):
                    Title = CalculateTitle();
                    return;

                case nameof(Entity.PreparationTime):
                    Description = CalculateDescription();
                    return;

                default:
                    return;
            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(Description):
                    ShowDescription = !string.IsNullOrWhiteSpace(Description);
                    return;

                default:
                    return;
            }
        }
        #endregion

        #region Private Methods
        private string CalculateDescription()
        {
            if (Entity.PreparationTime.Hours < 1 && Entity.PreparationTime.Minutes < 1)
                return string.Empty;

            var minutes = string.Format(Entity.PreparationTime.Minutes == 1 ? Resources.HintMinute : Resources.HintMinutes, Entity.PreparationTime.Minutes.ToString());

            if (Entity.PreparationTime.Hours < 1)
                return $"{minutes}.";

            var hours = string.Format(Entity.PreparationTime.Hours == 1 ? Resources.HintHour : Resources.HintHours, Entity.PreparationTime.Hours.ToString());

            return Entity.PreparationTime.Minutes > 0 ? $"{hours}, {minutes}." : $"{hours}.";
        }

        private byte[] CalculateImage()
        {
            return Entity.GetImage();
        }

        private string CalculateTitle()
        {
            return Entity.Name;
        }
        #endregion
    }
}
