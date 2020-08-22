using MvvmCross.Commands;
using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Data.Operations;
using SimpleMenu.Core.Models;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.Services.Wrappers;
using SimpleMenu.Core.ViewModels.CreateThing.Base;
using SimpleMenu.Core.ViewModels.List.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            LoadInitialPage();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Loads the initial page of data.
        /// </summary>
        public override void LoadInitialPage()
        {
            if (Data.Count > 0)
                Data.Clear();

            base.LoadInitialPage();
        }
        #endregion

        #region Protected Methods
        protected override async Task LoadInitialPageAsync()
        {
            var coreServiceWrapper = CoreServiceWrapper.Instance;

            var meals = await MealOperations.Instance.ListAllMealsAsync().ConfigureAwait(false);

            var menuMeals = new List<MenuMealModel>();

            MealEntity previousMeal = null;

            var random = new Random();

            for (var i = 0; i < _dates.Length; i++)
            {
                MealEntity meal = null;

                while (meal == null || meal == previousMeal)
                {
                    var mutableMeal = meals[random.Next(0, meals.Length)];

                    if (previousMeal == null || mutableMeal.IsGoodMatchWith(previousMeal))
                        meal = mutableMeal;
                }

                menuMeals.Add(new MenuMealModel(_dates) { Entity = meal, Index = i });
                previousMeal = meal;
            }

            InvokeOnMainThread(() => Data.AddRange(menuMeals));
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            RegenerateMenuButtonClickCommand = new MvxCommand(RegenerateMenuButton_Click);

            DataEmptyHint = Resources.HintNoMealsFound;
            LoadingHint = Resources.MessagingGeneratingMenu;
            Title = Resources.MessageConfigureMenu;

            CreateDates();
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
