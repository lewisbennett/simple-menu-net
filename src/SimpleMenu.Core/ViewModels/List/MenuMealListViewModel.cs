using DialogMessaging;
using DialogMessaging.Interactions;
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
        #region Properties
        /// <summary>
        /// Gets or sets the dates that the menu is being created for.
        /// </summary>
        public DateTime[] Dates { get; set; }
        #endregion

        #region Event Handlers
        protected override void OnItemClicked(MenuMealModel item)
        {
            base.OnItemClicked(item);

            // Navigate to choose menu meal view model to replace meal.
        }

        private void RegenerateMenuButton_Click()
        {
            var config = new ActionSheetBottomConfig
            {
                Title = Resources.TitleChooseDateRange,
                CancelButtonText = Resources.ActionCancel,
                ItemClickAction = (item) =>
                {
                    if (item.Data is int days)
                    {
                        var dates = new List<DateTime>();

                        for (var i = 0; i < days; i++)
                            dates.Add(DateTime.Now.Date.AddDays(i));

                        Dates = dates.ToArray();

                        LoadInitialPage();
                    }
                }
            };

            config.Items.Add(new ActionSheetItemConfig { Text = Resources.HintNextFiveDays, Data = 5 });
            config.Items.Add(new ActionSheetItemConfig { Text = Resources.HintNextSevenDays, Data = 7 });
            config.Items.Add(new ActionSheetItemConfig { Text = Resources.HintStartFromScratch, Data = 0 });

            MessagingService.Instance.ActionSheetBottom(config);
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
            if (Dates == null || Dates.Length < 1)
                return;

            var coreServiceWrapper = CoreServiceWrapper.Instance;

            var meals = await MealOperations.Instance.ListAllMealsAsync().ConfigureAwait(false);

            var menuMeals = new List<MenuMealModel>();

            MealEntity previousMeal = null;

            var random = new Random();

            for (var i = 0; i < Dates.Length; i++)
            {
                MealEntity meal = null;

                while (meal == null || meal == previousMeal)
                {
                    var mutableMeal = meals[random.Next(0, meals.Length)];

                    if (previousMeal == null || mutableMeal.IsGoodMatchWith(previousMeal))
                        meal = mutableMeal;
                }

                menuMeals.Add(new MenuMealModel(Dates) { Entity = meal, Index = i });
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
        }
        #endregion
    }
}
