using SimpleMenu.Core.Data.Entities.Base;
using System;

namespace SimpleMenu.Core.Data.Entities
{
    public class MenuEntity : BaseEntity
    {
        #region Fields
        private string _name = string.Empty;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name of this menu.
        /// </summary>
        public string Name
        {
            get => _name;

            set
            {
                value ??= string.Empty;

                if (_name.Equals(value))
                    return;

                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// Gets or sets the UUID for this menu.
        /// </summary>
        public Guid UUID { get; set; }
        #endregion
    }
}
