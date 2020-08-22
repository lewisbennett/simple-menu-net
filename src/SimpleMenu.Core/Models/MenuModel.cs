using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Data.Operations;
using SimpleMenu.Core.Interfaces;
using SimpleMenu.Core.Models.Base;
using SimpleMenu.Core.Properties;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMenu.Core.Models
{
    public partial class MenuModel : EntityDisplayBaseModel<MenuEntity>, IIndexable
    {
        #region Fields
        private int _index;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the index of the model.
        /// </summary>
        public int Index
        {
            get => _index;

            set
            {
                _index = value;

                if (Entity != null)
                    Entity.Index = _index;
            }
        }

        /// <summary>
        /// Gets whether the image is currently being loaded.
        /// </summary>
        public bool IsLoading { get; private set; }
        #endregion

        #region Event Handlers
        protected override void OnEntityChanged()
        {
            base.OnEntityChanged();

            Title = CalculateTitle();

            LoadData();
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
        /// Loads the image for the menu.
        /// </summary>
        public async void LoadData()
        {
            if (IsLoading)
                return;

            IsLoading = ShowLoading = true;

            await Task.Run(LoadDataAsync).ConfigureAwait(false);

            IsLoading = ShowLoading = false;
        }
        #endregion

        #region Private Methods
        private string CalculateTitle()
            => Entity.Name;

        private async Task LoadDataAsync()
        {
            var mealOperations = MealOperations.Instance;

            if (Entity.Meals.FirstOrDefault(m => m.DateTime == DateTime.Now.Date) is MenuMealEntity todaysMenuMeal)
            {
                var todaysMeal = await mealOperations.GetMealAsync(todaysMenuMeal.MealUUID).ConfigureAwait(false);

                Image = await ImageOperations.Instance.GetImageAsync(todaysMeal.ImageUUID, 300, 300).ConfigureAwait(false);

                Subtitle = string.Format(Resources.MessageTodaysMeal, todaysMeal.Name);
            }

            if (Entity.Meals.FirstOrDefault(m => m.DateTime == DateTime.Now.Date.AddDays(1)) is MenuMealEntity tomorrowsMenuMeal)
            {
                var tomorrowsMeal = await mealOperations.GetMealAsync(tomorrowsMenuMeal.MealUUID).ConfigureAwait(false);

                Description = string.Format(Resources.MessageTomorrowsMeal, tomorrowsMeal.Name);
            }
        }
        #endregion
    }
}
