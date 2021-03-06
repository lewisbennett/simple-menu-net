﻿using DialogMessaging;
using DialogMessaging.Interactions;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Data.Operations;
using SimpleMenu.Core.Models;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.Services.Wrappers;
using SimpleMenu.Core.ViewModels.CreateThing;
using SimpleMenu.Core.ViewModels.List.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMenu.Core.ViewModels.List
{
    public partial class MealListViewModel : RefreshableListBaseViewModel<MealModel>
    {
        #region Fields
        private readonly IMvxNavigationService _navigationService;
        #endregion

        #region Event Handlers
        private void AddMealButton_Click()
        {
            NavigateToCreateMealViewModel();
        }

        protected override void OnDataEmptyActionButtonClick()
        {
            base.OnDataEmptyActionButtonClick();

            NavigateToCreateMealViewModel();
        }

        protected override void OnItemClicked(MealModel item)
        {
            base.OnItemClicked(item);

            // Temporary.

            // Find the number of menus that will be deleted if the clicked meal is also deleted.
            var deleteMenuCount = FileServiceWrapper.Instance.Entities.Count(e => e is MenuEntity m && m.Meals.Any(me => me.MealUUID == item.Entity.UUID));

            string message;

            if (deleteMenuCount > 0)
                message = string.Format(deleteMenuCount == 1 ? Resources.MessageConfirmDeleteMealMenu : Resources.MessageConfirmDeleteMealMenus, deleteMenuCount);
            else
                message = Resources.MessageConfirmDeleteMeal;

            MessagingService.Instance.Delete(new DeleteConfig
            {
                Title = Resources.TitleConfirmDeleteMeal,
                Message = message,
                DeleteButtonText = Resources.ActionDelete,
                CancelButtonText = Resources.ActionCancel,
                DeleteButtonClickAction = () => DeleteMeal(item.Entity)
            });
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

            AddMealButtonClickCommand = new MvxCommand(AddMealButton_Click);

            ShouldShowDataEmptyAction = true;

            DataEmptyActionButtonText = Resources.ActionAddOneNow;
            DataEmptyHint = Resources.HintNoMealsFound;
            LoadingHint = Resources.MessagingLoadingMeals;
            Title = Resources.TitleMeals;
        }
        #endregion

        #region Constructors
        public MealListViewModel(IMvxNavigationService navigationService)
            : base()
        {
            _navigationService = navigationService;
        }
        #endregion

        #region Private Methods
        private async void DeleteMeal(MealEntity meal)
        {
            await MessagingService.Instance.ShowLoadingAsync(Resources.MessagingDeletingMeal, DeleteMealAsync(meal.UUID)).ConfigureAwait(false);
        }

        private async Task DeleteMealAsync(Guid mealUuid)
        {
            await MealOperations.Instance.DeleteMealAsync(mealUuid).ConfigureAwait(false);

            await LoadInitialPageAsync().ConfigureAwait(false);
        }

        private void NavigateToCreateMealViewModel()
        {
            _navigationService.Navigate<CreateMealViewModel>();
        }

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
