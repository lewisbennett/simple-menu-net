using MvvmCross.ViewModels;
using System;

namespace SimpleMenu.Core.Data.Entities
{
    public class MenuMealEntity : MvxNotifyPropertyChanged
    {
        #region Fields
        private DateTime _dateTime;
        private Guid _mealUuid;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the datetime.
        /// </summary>
        public DateTime DateTime
        {
            get => _dateTime;

            set => SetProperty(ref _dateTime, value);
        }

        /// <summary>
        /// Gets or sets the meal UUID.
        /// </summary>
        public Guid MealUUID
        {
            get => _mealUuid;

            set => SetProperty(ref _mealUuid, value);
        }
        #endregion
    }
}
