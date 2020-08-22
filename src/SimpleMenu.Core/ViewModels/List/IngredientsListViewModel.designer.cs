using MvvmCross.Commands;

namespace SimpleMenu.Core.ViewModels.List
{
    partial class IngredientsListViewModel
    {
        #region Fields
        private IMvxCommand _addIngredientButtonClickCommand;
        #endregion

        #region Properties
        public IMvxCommand AddIngredientButtonClickCommand
        {
            get => _addIngredientButtonClickCommand;

            set => SetProperty(ref _addIngredientButtonClickCommand, value);
        }
        #endregion
    }
}
