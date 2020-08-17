using SimpleMenu.Core.Data.Entities.Base;
using System;

namespace SimpleMenu.Core.Data.Entities
{
    public class IngredientEntity : BaseEntity
    {
        #region Fields
        private byte[] _image;
        private string _name = string.Empty;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the image for this ingredient, if any.
        /// </summary>
        public byte[] Image
        {
            get => _image;

            set
            {
                if (_image == value)
                    return;

                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        /// <summary>
        /// Gets or sets the name of this ingredient.
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
        /// Gets or sets the UUID for this ingredient.
        /// </summary>
        public Guid UUID { get; set; }
        #endregion
    }
}
