using MvvmCross.Commands;

namespace SimpleMenu.Core.ViewModels.Base
{
    partial class CalculatorBaseViewModel
    {
        #region Fields
        private string _fromMetricButtonText, _fromUnits, _fromUnitsHint, _toMetricButtonText, _toUnits, _toUnitsHint;
        private IMvxCommand _fromMetricButtonClickCommand, _swapButtonClickCommand, _toMetricButtonClickCommand;
        #endregion

        #region Properties
        public IMvxCommand FromMetricButtonClickCommand
        {
            get => _fromMetricButtonClickCommand;

            set => SetProperty(ref _fromMetricButtonClickCommand, value);
        }

        public string FromMetricButtonText
        {
            get => _fromMetricButtonText;

            set => SetProperty(ref _fromMetricButtonText, value);
        }

        public string FromUnits
        {
            get => _fromUnits;

            set => SetProperty(ref _fromUnits, value);
        }

        public string FromUnitsHint
        {
            get => _fromUnitsHint;

            set => SetProperty(ref _fromUnitsHint, value);
        }

        public IMvxCommand SwapButtonClickCommand
        {
            get => _swapButtonClickCommand;

            set => SetProperty(ref _swapButtonClickCommand, value);
        }

        public IMvxCommand ToMetricButtonClickCommand
        {
            get => _toMetricButtonClickCommand;

            set => SetProperty(ref _toMetricButtonClickCommand, value);
        }

        public string ToMetricButtonText
        {
            get => _toMetricButtonText;

            set => SetProperty(ref _toMetricButtonText, value);
        }

        public string ToUnits
        {
            get => _toUnits;

            set => SetProperty(ref _toUnits, value);
        }

        public string ToUnitsHint
        {
            get => _toUnitsHint;

            set => SetProperty(ref _toUnitsHint, value);
        }
        #endregion
    }
}
