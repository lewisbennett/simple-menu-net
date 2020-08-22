using MvvmCross.Navigation;
using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Models;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.ViewModels.CreateThing;
using SimpleMenu.Core.ViewModels.List.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMenu.Core.ViewModels.List
{
    public class IngredientsListViewModel : RefreshableListBaseViewModel<IngredientModel>
    {
        #region Fields
        private readonly IMvxNavigationService _navigationService;
        #endregion

        #region Event Handlers
        protected override void OnDataEmptyActionButtonClick()
        {
            base.OnDataEmptyActionButtonClick();

            NavigateToCreateIngredientViewModel();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Navigates to the create ingredient view model.
        /// </summary>
        public void NavigateToCreateIngredientViewModel()
        {
            _navigationService.Navigate<CreateIngredientViewModel>();
        }
        #endregion

        #region Protected Methods
        protected override async Task LoadInitialPageAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            ShouldShowDataEmptyAction = true;

            DataEmptyActionButtonText = Resources.ActionAddOneNow;
            DataEmptyHint = Resources.HintNoIngredientsFound;
            LoadingHint = Resources.MessagingLoadingIngredients;
            Title = Resources.TitleIngredients;
        }
        #endregion

        #region Constructors
        public IngredientsListViewModel(IMvxNavigationService navigationService)
            : base()
        {
            _navigationService = navigationService;
        }
        #endregion

        #region Private Methods
        private void UpdateCollection(IEnumerable<IngredientEntity> ingredients)
        {
            var removals = Data.Where(d => !ingredients.Any(de => de.UUID == d.Entity.UUID)).ToArray();

            if (removals.Length > 0)
            {
                foreach (var removal in removals)
                    Data.Remove(removal);
            }

            var additions = ingredients.Where(d => !Data.Any(da => da.Entity.UUID == d.UUID)).ToArray();

            if (additions.Length > 0)
                Data.AddRange(additions.Select(a => new IngredientModel { Entity = a }));
        }
        #endregion
    }
}
