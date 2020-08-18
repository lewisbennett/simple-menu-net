using MvvmCross.ViewModels;
using System.ComponentModel;

namespace SimpleMenu.Core.Models.Base
{
    public abstract class BaseModel : MvxNotifyPropertyChanged
    {
        #region Event Handlers
        private void BaseModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
            => OnPropertyChanged(e.PropertyName);

        protected virtual void OnPropertyChanged(string propertyName)
        {
        }
        #endregion

        #region Constructors
        protected BaseModel()
            : base()
        {
            PropertyChanged += BaseModel_PropertyChanged;
        }
        #endregion
    }
}
