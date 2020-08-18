using MvvmCross.ViewModels;
using System;

namespace SimpleMenu.Core.Data.Entities
{
    public class IngredientEntity : MvxNotifyPropertyChanged
    {
        #region Fields
        private byte[] _image;
        private string _name;
        private Guid _uuid;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the image for this ingredient, if any.
        /// </summary>
        public byte[] Image
        {
            get => _image;

            set => SetProperty(ref _image, value);
        }

        /// <summary>
        /// Gets or sets the name of this ingredient.
        /// </summary>
        public string Name
        {
            get => _name;

            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// Gets or sets the UUID for this ingredient.
        /// </summary>
        public Guid UUID
        {
            get => _uuid;

            set => SetProperty(ref _uuid, value);
        }
        #endregion
    }
}
