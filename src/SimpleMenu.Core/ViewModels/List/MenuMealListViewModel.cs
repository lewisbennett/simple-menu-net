using DialogMessaging;
using DialogMessaging.Interactions;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Data.Operations;
using SimpleMenu.Core.Models;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.ViewModels.CreateThing.Base;
using SimpleMenu.Core.ViewModels.List.Base;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMenu.Core.ViewModels.List
{
    public partial class MenuMealListViewModel : ListBaseViewModel<MenuMealModel>, ICreateThingStepViewModel
    {
        #region Fields
        private DateTime[] _dates;
        private readonly IMvxNavigationService _navigationService;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the dates that the menu is being created for.
        /// </summary>
        public DateTime[] Dates
        {
            get => _dates;

            set
            {
                _dates = value;

                foreach (var item in Data)
                    item.Dates = _dates;
            }
        }
        #endregion

        #region Event Handlers
        private void AddMealButton_Click()
        {
            _navigationService.Navigate<SelectMealListViewModel, SelectMealListViewModelNavigationParams>(new SelectMealListViewModelNavigationParams
            {
                MealSelectedCallback = (meal) =>
                {
                    CreateDates(Data.Count + 1);

                    Data.Add(new MenuMealModel { Dates = Dates, Entity = meal });
                }
            });
        }

        private void Data_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CriteriaMet = Data.Count >= 2;
        }

        protected override void OnItemClicked(MenuMealModel item)
        {
            base.OnItemClicked(item);

            var config = new ActionSheetBottomConfig
            {
                Title = Resources.TitleChooseAction,
                CancelButtonText = Resources.ActionCancel
            };

            config.Items.Add(new ActionSheetItemConfig { Text = Resources.HintSwapMeal, ClickAction = () => ReplaceItem(item) });
            config.Items.Add(new ActionSheetItemConfig { Text = Resources.HintRemoveMeal, ClickAction = () => RemoveItem(item) });

            MessagingService.Instance.ActionSheetBottom(config);
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(IsDataEmpty):
                    CriteriaMet = !IsDataEmpty;
                    return;

                default:
                    return;
            }
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
                        if (days < 1)
                            Data.Clear();
                        else
                            GenerateRandomMenu(days);
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
        /// Adds the event handlers for this ViewModel.
        /// </summary>
        public override void AddEventHandlers()
        {
            base.AddEventHandlers();

            Data.CollectionChanged += Data_CollectionChanged;
        }

        /// <summary>
        /// Generate a random menu for a number of days.
        /// </summary>
        public async void GenerateRandomMenu(int days)
        {
            if (IsLoading)
                return;

            if (Data.Count > 0)
                Data.Clear();

            IsLoading = true;

            ShowLoading = IsDataEmpty;

            await GenerateRandomMenuAsync(days).ConfigureAwait(false);

            ShowLoading = IsLoading = false;
        }

        /// <summary>
        /// Removes the event handlers from this ViewModel.
        /// </summary>
        public override void RemoveEventHandlers()
        {
            base.RemoveEventHandlers();

            Data.CollectionChanged -= Data_CollectionChanged;
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            AddMealButtonClickCommand = new MvxCommand(AddMealButton_Click);
            RegenerateMenuButtonClickCommand = new MvxCommand(RegenerateMenuButton_Click);

            ShowNextButton = true;

            DataEmptyHint = Resources.HintNoMealsFound;
            LoadingHint = Resources.MessagingGeneratingMenu;
            Title = Resources.MessageConfigureMenu;
        }
        #endregion

        #region Constructors
        public MenuMealListViewModel(IMvxNavigationService navigationService)
            : base()
        {
            _navigationService = navigationService;
        }
        #endregion

        #region Private Methods
        private void CreateDates(int days)
        {
            var dates = new List<DateTime>();

            for (var i = 0; i < days; i++)
                dates.Add(DateTime.Now.Date.AddDays(i));

            Dates = dates.ToArray();
        }

        private async Task GenerateRandomMenuAsync(int days)
        {
            if (days < 1)
                return;

            var meals = await MealOperations.Instance.ListAllMealsAsync().ConfigureAwait(false);

            CreateDates(days);

            var selectedMeals = new List<MenuMealModel>();

            MealEntity previousMeal = null;

            var random = new Random();

            for (var i = 0; i < Dates.Length; i++)
            {
                MealEntity meal = null;

                while (meal == null || meal == previousMeal)
                {
                    var randomMeal = meals[random.Next(0, meals.Length)];

                    // If there are more meals available than days requested, do not allow duplicates.
                    if (meals.Length >= days && selectedMeals.Any(s => s.Entity == randomMeal))
                        randomMeal = null;

                    else if (previousMeal == null || randomMeal.IsGoodMatchWith(previousMeal))
                        meal = randomMeal;
                }

                selectedMeals.Add(new MenuMealModel { Dates = Dates, Entity = meal });

                previousMeal = meal;
            }

            InvokeOnMainThread(() => Data.AddRange(selectedMeals));
        }

        private void RemoveItem(MenuMealModel item)
        {
            Data.Remove(item);

            CreateDates(Data.Count);
        }

        private void ReplaceItem(MenuMealModel item)
        {
            _navigationService.Navigate<SelectMealListViewModel, SelectMealListViewModelNavigationParams>(new SelectMealListViewModelNavigationParams
            {
                MealSelectedCallback = (meal) =>
                {
                    item.Entity = meal;
                }
            });
        }
        #endregion
    }
}
