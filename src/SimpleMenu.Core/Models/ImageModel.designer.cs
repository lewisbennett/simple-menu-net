namespace SimpleMenu.Core.Models
{
    partial class ImageModel
    {
        #region Fields
        private byte[] _image;
        #endregion

        #region Properties
        public byte[] Image
        {
            get => _image;

            set => SetProperty(ref _image, value);
        }
        #endregion
    }
}
