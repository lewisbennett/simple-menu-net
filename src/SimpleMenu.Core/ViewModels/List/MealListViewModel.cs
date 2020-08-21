using DialogMessaging;
using DialogMessaging.Interactions;
using MvvmCross.Navigation;
using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Data.Operations;
using SimpleMenu.Core.Models;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.Services.Wrappers;
using SimpleMenu.Core.ViewModels.CreateThing;
using SimpleMenu.Core.ViewModels.List.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMenu.Core.ViewModels.List
{
    public class MealListViewModel : RefreshableListBaseViewModel<MealModel>
    {
        #region Fields
        private readonly IMvxNavigationService _navigationService;
        #endregion

        #region Event Handlers
        protected override void OnDataEmptyActionButtonClick()
        {
            base.OnDataEmptyActionButtonClick();

            NavigateToCreateMealViewModel();
        }

        protected override void OnItemClicked(MealModel item)
        {
            base.OnItemClicked(item);

            // Temporary.
            MessagingService.Instance.Delete(new DeleteConfig
            {
                Title = Resources.TitleConfirmDeleteMeal,
                Message = Resources.MessageConfirmDeleteMeal,
                DeleteButtonText = Resources.ActionDelete,
                CancelButtonText = Resources.ActionCancel,
                DeleteButtonClickAction = () => DeleteMeal(item.Entity)
            });
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Navigates to the create meal view model.
        /// </summary>
        public void NavigateToCreateMealViewModel()
        {
            _navigationService.Navigate<CreateMealViewModel>();
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

            ShouldShowDataEmptyAction = true;

            DataEmptyActionButtonText = Resources.ActionAddOneNow;
            DataEmptyHint = Resources.HintNoMealsFound;
            LoadingHint = Resources.MessagingLoadingMeals;
            Title = Resources.TitleMeals;
        }

        public override void ViewCreated()
        {
            base.ViewCreated();

            UpdateCollection(CoreServiceWrapper.Instance.ActiveUser.Meals);
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
            await MessagingService.Instance.ShowLoadingAsync(Resources.MessagingDeletingMeal, MealOperations.Instance.DeleteMealAsync(meal.UUID)).ConfigureAwait(false);

            UpdateCollection(CoreServiceWrapper.Instance.ActiveUser.Meals);
        }

        private void UpdateCollection(IEnumerable<MealEntity> meals)
        {
            var removals = Data.Where(d => !meals.Any(de => de.UUID == d.Entity.UUID)).ToArray();

            if (removals.Length > 0)
            {
                foreach (var removal in removals)
                    Data.Remove(removal);
            }

            var additions = meals.Where(d => !Data.Any(da => da.Entity.UUID == d.UUID)).OrderBy(m => m.Index).ToArray();

            foreach (var addition in additions)
                Data.Insert(addition.Index, new MealModel { Entity = addition });
        }
        #endregion
    }
}
