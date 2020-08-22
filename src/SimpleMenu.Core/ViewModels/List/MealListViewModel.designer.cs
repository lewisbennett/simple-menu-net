using MvvmCross.Commands;

namespace SimpleMenu.Core.ViewModels.List
{
    partial class MealListViewModel
    {
        #region Fields
        private IMvxCommand _addMealButtonClickCommand;
        #endregion

        #region Properties
        public IMvxCommand AddMealButtonClickCommand
        {
            get => _addMealButtonClickCommand;

            set => SetProperty(ref _addMealButtonClickCommand, value);
        }
        #endregion
    }
}
