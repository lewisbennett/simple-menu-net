using MvvmCross.Commands;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.ViewModels.Base;
using System;
using System.ComponentModel;

namespace SimpleMenu.Core.ViewModels.CreateThing.Base
{
    public abstract partial class CreateThingBaseViewModel : BaseViewModel
    {
        #region Fields
        private ICreateThingStepViewModel _currentStep;
        private ICreateThingStepViewModel[] _steps;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the current step.
        /// </summary>
        public ICreateThingStepViewModel CurrentStep
        {
            get => _currentStep;

            set
            {
                if (_currentStep != null)
                    _currentStep.PropertyChanged -= CurrentStep_PropertyChanged;

                if (_currentStep != value)
                {
                    _currentStep = value;

                    if (_currentStep != null)
                        _currentStep.PropertyChanged += CurrentStep_PropertyChanged;

                    RaisePropertyChanged(() => CurrentStep);

                    Recalculate();
                }
            }
        }

        /// <summary>
        /// Gets whether the current step is the final step.
        /// </summary>
        public bool IsFinalStep => Array.IndexOf(_steps, CurrentStep) == _steps.Length - 1;
        #endregion

        #region Event Handlers
        private void CurrentStep_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentStep.CriteriaMet):
                    Recalculate();
                    return;

                default:
                    return;
            }
        }

        private void BackButton_Click() => PreviousStep();

        protected virtual void OnNextButtonClicked()
        {
            if (IsFinalStep)
                CreateThingAndClose();
            else
                NextStep();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates the thing and closes the view model.
        /// </summary>
        public abstract void CreateThingAndClose();

        /// <summary>
        /// Moves to the next step, if available.
        /// </summary>
        public void NextStep()
        {
            var index = -1;

            for (var i = 0; i < _steps.Length; i++)
            {
                if (_steps[i] == CurrentStep)
                    index = i + 1;
            }

            if (index < _steps.Length)
                CurrentStep = _steps[index];
        }

        /// <summary>
        /// Moves to the previous step, if available.
        /// </summary>
        public void PreviousStep()
        {
            var index = -1;

            for (var i = 0; i < _steps.Length; i++)
            {
                if (_steps[i] == CurrentStep)
                    index = i - 1;
            }

            if (index >= 0)
                CurrentStep = _steps[index];
        }
        #endregion

        #region Protected Methods
        protected abstract ICreateThingStepViewModel[] CreateSteps();
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            _steps = CreateSteps();

            CurrentStep = _steps[0];

            BackButtonClickCommand = new MvxCommand(BackButton_Click);
            NextButtonClickCommand = new MvxCommand(OnNextButtonClicked);

            BackButtonText = Resources.ActionBack;
        }
        #endregion

        #region Private Methods
        private void Recalculate()
        {
            if (CurrentStep == null)
                CriteriaMet = ShowNextButton = true;
            else
            {
                CriteriaMet = CurrentStep.CriteriaMet;
                ShowNextButton = CurrentStep.ShowNextButton;
            }

            var stepIndex = Array.IndexOf(_steps, CurrentStep);

            NextButtonText = stepIndex == _steps.Length - 1 ? Resources.ActionFinish : Resources.ActionNext;

            ShowBackButton = stepIndex != 0;
        }
        #endregion
    }
}
