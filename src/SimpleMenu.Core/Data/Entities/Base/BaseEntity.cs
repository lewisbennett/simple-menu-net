using MvvmCross.ViewModels;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SimpleMenu.Core.Data.Entities.Base
{
    public abstract class BaseEntity : MvxNotifyPropertyChanged
    {
        #region Properties
        /// <summary>
        /// Gets or sets whether the entity has been changed since it was last saved.
        /// </summary>
        public bool HasBeenChanged { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Saves the entity.
        /// </summary>
        public abstract Task SaveAsync();
        #endregion

        #region Protected Methods
        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            var result = base.SetProperty(ref storage, value, propertyName);

            if (result)
                HasBeenChanged = true;

            return result;
        }
        #endregion
    }
}
