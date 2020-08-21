using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Data.Operations;
using SimpleMenu.Core.Interfaces;
using SimpleMenu.Core.Models.Base;
using SimpleMenu.Core.Properties;

namespace SimpleMenu.Core.Models
{
    public partial class MealModel : EntityDisplayBaseModel<MealEntity>, IIndexableModel
    {
        #region Properties
        /// <summary>
        /// Gets or sets the index of the model.
        /// </summary>
        public int Index { get; set; }
        #endregion

        #region Event Handlers
        protected override void OnEntityChanged()
        {
            base.OnEntityChanged();

            Index = Entity.Index;

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

        #region Public Methods
        /// <summary>
        /// Saves the model.
        /// </summary>
        public async void Save()
        {
            Entity.Index = Index;

            // Fire and forget.
            await MealOperations.Instance.SaveMealAsync(Entity).ConfigureAwait(false);
        }
        #endregion

        #region Private Methods
        private string CalculateDescription()
        {
            var preparationTime = Entity.GetPreparationTime();

            if (preparationTime.Hours < 1 && preparationTime.Minutes < 1)
                return string.Empty;

            var minutes = string.Format(preparationTime.Minutes == 1 ? Resources.HintMinute : Resources.HintMinutes, preparationTime.Minutes.ToString());

            if (preparationTime.Hours < 1)
                return $"{minutes}.";

            var hours = string.Format(preparationTime.Hours == 1 ? Resources.HintHour : Resources.HintHours, preparationTime.Hours.ToString());

            return preparationTime.Minutes > 0 ? $"{hours}, {minutes}." : $"{hours}.";
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
