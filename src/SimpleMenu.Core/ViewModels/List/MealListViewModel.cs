using MvvmCross;
using MvvmCross.Navigation;
using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Models;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.Services.Wrappers;
using SimpleMenu.Core.ViewModels.List.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMenu.Core.ViewModels.List
{
    public class MealListViewModel : RefreshableListBaseViewModel<MealModel>
    {
        #region Properties
        /// <summary>
        /// Convenience property for CoreServiceWrapper.Instance.
        /// </summary>
        public CoreServiceWrapper CoreServiceWrapper => CoreServiceWrapper.Instance;

        /// <summary>
        /// Gets the navigation service.
        /// </summary>
        public IMvxNavigationService NavigationService { get; } = Mvx.IoCProvider.Resolve<IMvxNavigationService>();
        #endregion

        #region Event Handlers
        protected override void OnDataEmptyActionButtonClick()
        {
            base.OnDataEmptyActionButtonClick();

            NavigateToCreateMealViewModel();
        }
        #endregion

        #region Public Methods
        public override async Task LoadInitialPageAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);

            InvokeOnMainThread(() => UpdateCollection(CoreServiceWrapper.ActiveUser.Meals));
        }

        /// <summary>
        /// Navigates to the create meal view model.
        /// </summary>
        public void NavigateToCreateMealViewModel()
        {
            NavigationService.Navigate<CreateMealViewModel>();
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            ShouldShowDataEmptyAction = true;

            DataEmptyActionButtonText = Resources.ActionAddOneNow;
            DataEmptyHint = Resources.HintNoMealsFound;
            Title = Resources.TitleMeals;
        }

        public override void ViewCreated()
        {
            base.ViewCreated();

            UpdateCollection(CoreServiceWrapper.ActiveUser.Meals);
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

            var additions = meals.Where(d => !Data.Any(da => da.Entity.UUID == d.UUID)).ToArray();

            if (additions.Length > 0)
                Data.AddRange(additions.Select(a => new MealModel { Entity = a }));
        }
        #endregion
    }
}
