using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using SimpleMenu.Core.Properties;

namespace SimpleMenu.Core.ViewModels.Base
{
    public abstract class CreateThingBaseViewModel : BaseViewModel
    {
        #region Fields
        private bool _showNextButton;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the text displayed on the finish button.
        /// </summary>
        public string FinishButtonText => Resources.ActionFinish;

        /// <summary>
        /// Gets the navigation service.
        /// </summary>
        public IMvxNavigationService NavigationService { get; } = Mvx.IoCProvider.Resolve<IMvxNavigationService>();

        /// <summary>
        /// Gets the text displayed on the next button.
        /// </summary>
        public string NextButtonText => Resources.ActionNext;

        /// <summary>
        /// Gets or sets whether the next button should be shown.
        /// </summary>
        public bool ShowNextButton
        {
            get => _showNextButton;

            set
            {
                if (_showNextButton == value)
                    return;

                _showNextButton = value;
                RaisePropertyChanged(() => ShowNextButton);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates the thing and closes this view model.
        /// </summary>
        public abstract void CreateThingAndClose();
        #endregion
    }

    public abstract class CreateThingBaseViewModel<TNavigationParams> : CreateThingBaseViewModel, IMvxViewModel<TNavigationParams>
        where TNavigationParams : class
    {
        #region Lifecycle
        public virtual void Prepare(TNavigationParams parameter)
        {
        }
        #endregion
    }
}
