using MvvmCross.Commands;
using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Models;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.Services.Wrappers;
using SimpleMenu.Core.ViewModels.CreateThing.Base;
using SimpleMenu.Core.ViewModels.List.Base;
using System;
using System.Collections.Generic;

namespace SimpleMenu.Core.ViewModels.List
{
    public partial class MenuMealListViewModel : ListBaseViewModel<MenuMealModel>, ICreateThingStepViewModel
    {
        #region Fields
        private DateTime[] _dates;
        #endregion

        #region Event Handlers
        protected override void OnItemClicked(MenuMealModel item)
        {
            base.OnItemClicked(item);

            // Navigate to choose menu meal view model to replace meal.
        }

        private void RegenerateMenuButton_Click()
        {
            GenerateRandomMenu();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Generates a random menu.
        /// </summary>
        public void GenerateRandomMenu()
        {
            var coreServiceWrapper = CoreServiceWrapper.Instance;

            var menuMeals = new List<MenuMealModel>();

            MealEntity previousMeal = null;

            var random = new Random();

            for (var i = 0; i < _dates.Length; i++)
            {
                MealEntity meal = null;

                while (meal == null || meal == previousMeal)
                {
                    var mutableMeal = coreServiceWrapper.ActiveUser.Meals[random.Next(0, coreServiceWrapper.ActiveUser.Meals.Count)];

                    if (previousMeal == null || mutableMeal.IsGoodMatchWith(previousMeal))
                        meal = mutableMeal;
                }

                menuMeals.Add(new MenuMealModel(_dates) { Entity = meal, Index = i });
                previousMeal = meal;
            }

            if (Data.Count > 0)
                Data.Clear();

            Data.AddRange(menuMeals);
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            RegenerateMenuButtonClickCommand = new MvxCommand(RegenerateMenuButton_Click);

            Title = Resources.MessageConfigureMenu;

            CreateDates();

            GenerateRandomMenu();
        }
        #endregion

        #region Private Methods
        private void CreateDates()
        {
            _dates = new DateTime[7];

            for (var i = 0; i < _dates.Length; i++)
                _dates[i] = DateTime.Now.Date.AddDays(i);
        }
        #endregion
    }
}
