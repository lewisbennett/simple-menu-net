using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Models.Base;
using SimpleMenu.Core.Schema;

namespace SimpleMenu.Core.Models
{
    public partial class TimeOfDayModel : EntityDisplayBaseModel<TimeOfDayEntity>
    {
        #region Event Handlers
        protected override void OnEntityChanged()
        {
            base.OnEntityChanged();

            Description = CalculateDescription();
            Icon = CalculateIcon();
            Title = CalculateTitle();
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
            var timeOfDay = Entity.GetTimeOfDay();

            return $"{timeOfDay.Hours:00}:{timeOfDay.Minutes:00}";
        }

        private Icon CalculateIcon()
        {
            var timeOfDay = Entity.GetTimeOfDay();

            // Night/morning.
            if (timeOfDay.Hours >= 0 && timeOfDay.Hours < 5)
                return Icon.Brightness3;

            // Morning.
            if (timeOfDay.Hours >= 5 && timeOfDay.Hours < 12)
                return Icon.Brightness5;

            // Afternoon.
            if (timeOfDay.Hours >= 12 && timeOfDay.Hours < 15)
                return Icon.Brightness7;

            // Evening.
            if (timeOfDay.Hours >= 15 && timeOfDay.Hours < 19)
                return Icon.Brightness6;

            // Night.
            if (timeOfDay.Hours >= 19 && timeOfDay.Hours < 21)
                return Icon.Brightness4;

            return Icon.Brightness2;
        }

        private string CalculateTitle()
            => Entity.Name;
        #endregion
    }
}
