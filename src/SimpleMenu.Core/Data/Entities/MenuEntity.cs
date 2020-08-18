using MvvmCross.ViewModels;
using System;

namespace SimpleMenu.Core.Data.Entities
{
    public class MenuEntity : MvxNotifyPropertyChanged
    {
        #region Fields
        private string _name;
        private Guid _uuid;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name of this menu.
        /// </summary>
        public string Name
        {
            get => _name;

            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// Gets or sets the UUID for this menu.
        /// </summary>
        public Guid UUID
        {
            get => _uuid;

            set => SetProperty(ref _uuid, value);
        }
        #endregion
    }
}
