using MvvmCross.ViewModels;

namespace SimpleMenu.Core.Data.Entities
{
    public class WebImageEntity : MvxNotifyPropertyChanged
    {
        #region Fields
        private string _imageUri;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the image URI.
        /// </summary>
        public string ImageUri
        {
            get => _imageUri;

            set => SetProperty(ref _imageUri, value);
        }
        #endregion
    }
}
