using MvvmCross.ViewModels;
using System.ComponentModel;

namespace SimpleMenu.Core.ViewModels.Base
{
    public abstract partial class BaseViewModel : MvxViewModel
    {
        #region Event Handlers
        private void BaseViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
            => OnPropertyChanged(e.PropertyName);

        protected virtual void OnPropertyChanged(string propertyName)
        {
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

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            PropertyChanged += BaseViewModel_PropertyChanged;
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);

            if (viewFinishing)
                PropertyChanged -= BaseViewModel_PropertyChanged;
        }
        #endregion
    }

    public abstract class BaseViewModel<TNavigationParams> : BaseViewModel, IMvxViewModel<TNavigationParams>
        where TNavigationParams : class
    {
        #region Lifecycle
        public virtual void Prepare(TNavigationParams parameter)
        {
        }
        #endregion
    }
}
