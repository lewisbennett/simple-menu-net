using MvvmCross.Navigation;
using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Data.Operations;
using SimpleMenu.Core.Models;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.ViewModels.List.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMenu.Core.ViewModels.List
{
    public class SelectMealListViewModelNavigationParams
    {
        #region Properties
        /// <summary>
        /// Gets or sets the action invoked when a meal is selected.
        /// </summary>
        public Action<MealEntity> MealSelectedCallback { get; set; }
        #endregion
    }

    public class SelectMealListViewModel : RefreshableListBaseViewModel<MealModel, SelectMealListViewModelNavigationParams>
    {
        #region Fields
        private Action<MealEntity> _mealSelectedCallback;
        private readonly IMvxNavigationService _navigationService;
        #endregion

        #region Event Handlers
        protected override void OnItemClicked(MealModel item)
        {
            base.OnItemClicked(item);

            _mealSelectedCallback?.Invoke(item.Entity);

            _navigationService.Close(this);
        }
        #endregion

        #region Protected Methods
        protected override async Task LoadInitialPageAsync()
        {
            var meals = await MealOperations.Instance.ListAllMealsAsync().ConfigureAwait(false);

            InvokeOnMainThread(() => UpdateCollection(meals));
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            DataEmptyHint = Resources.HintNoMealsFound;
            LoadingHint = Resources.MessagingLoadingMeals;
            Title = Resources.TitleSelectMeal;
        }

        public override void Prepare(SelectMealListViewModelNavigationParams parameter)
        {
            base.Prepare(parameter);

            _mealSelectedCallback = parameter.MealSelectedCallback;
        }
        #endregion

        #region Constructors
        public SelectMealListViewModel(IMvxNavigationService navigationService)
            : base()
        {
            _navigationService = navigationService;
        }
        #endregion

        #region Private Methods
        private void UpdateCollection(IEnumerable<MealEntity> meals)
        {
            var removals = Data.Where(d => !meals.Any(de => de.UUID == d.Entity.UUID)).ToArray();

            if (removals.Length > 0)
            {
                foreach (var removal in removals)
                    Data.Remove(removal);
            }

            var additions = meals.Where(d => !Data.Any(da => da.Entity.UUID == d.UUID)).OrderByDescending(m => m.Index).ToArray();

            foreach (var addition in additions)
                Data.Insert(0, new MealModel { Entity = addition });
        }
        #endregion
    }
}
