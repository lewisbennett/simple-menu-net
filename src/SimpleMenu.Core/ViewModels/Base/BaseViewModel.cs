using MvvmCross.ViewModels;

namespace SimpleMenu.Core.ViewModels.Base
{
    public abstract class BaseViewModel : MvxViewModel
    {
        #region Fields
        private string _title = string.Empty;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the title for this ViewModel.
        /// </summary>
        public string Title
        {
            get => _title;

            set
            {
                value ??= string.Empty;

                if (_title.Equals(value))
                    return;

                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds the event handlers for this ViewModel.
        /// </summary>
        public virtual void AddEventHandlers()
        {
        }

        /// <summary>
        /// Removes the event handlers from this ViewModel.
        /// </summary>
        public virtual void RemoveEventHandlers()
        {
        }
        #endregion
    }

    public abstract class BaseViewModel<TNavigationParams> : BaseViewModel, IMvxViewModel<TNavigationParams>
        where TNavigationParams : class
    {
        #region Properties
        public virtual void Prepare(TNavigationParams parameter)
        {
        }
        #endregion
    }
}
