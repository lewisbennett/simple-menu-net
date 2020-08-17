using MvvmCross.Commands;

namespace SimpleMenu.Core.ViewModels
{
    partial class AddPictureViewModel
    {
        #region Fields
        private IMvxCommand _addPictureButtonClickCommand;
        private string _compliment;
        private bool _criteriaMet, _showCompliment, _showNextButton;
        private byte[] _image;
        #endregion

        #region Properties
        public IMvxCommand AddPictureButtonClickCommand
        {
            get => _addPictureButtonClickCommand;

            set => SetProperty(ref _addPictureButtonClickCommand, value);
        }

        public string Compliment
        {
            get => _compliment;

            set => SetProperty(ref _compliment, value);
        }

        public bool CriteriaMet
        {
            get => _criteriaMet;

            set => SetProperty(ref _criteriaMet, value);
        }

        public byte[] Image
        {
            get => _image;

            set => SetProperty(ref _image, value);
        }

        public bool ShowCompliment
        {
            get => _showCompliment;

            set => SetProperty(ref _showCompliment, value);
        }

        public bool ShowNextButton
        {
            get => _showNextButton;

            set => SetProperty(ref _showNextButton, value);
        }
        #endregion
    }
}
