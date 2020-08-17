using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Models.Base;

namespace SimpleMenu.Core.Models
{
    public class MenuModel : EntityDisplayBaseModel<MenuEntity>
    {
        #region Fields
        private string _description = string.Empty, _title = string.Empty;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description
        {
            get => _description;

            set
            {
                value ??= string.Empty;

                if (_description.Equals(value))
                    return;

                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        /// <summary>
        /// Gets or sets the title.
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
                OnPropertyChanged(nameof(Title));
            }
        }
        #endregion
    }
}
