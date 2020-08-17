using DialogMessaging;
using DialogMessaging.Interactions;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.PictureChooser;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.ViewModels.Base;
using System;
using System.IO;

namespace SimpleMenu.Core.ViewModels
{
    public class AddPictureViewModelNavigationParams
    {
        #region Properties
        /// <summary>
        /// Gets or sets a function to retrieve a compliment shown when a picture is loaded.
        /// </summary>
        public Func<string> CalculateCompliment { get; set; }

        /// <summary>
        /// Gets or sets the title for this step.
        /// </summary>
        public string Title { get; set; }
        #endregion
    }

    public class AddPictureViewModel : BaseViewModel<AddPictureViewModelNavigationParams>
    {
        #region Fields
        private Func<string> _calculateCompliment;
        private string _compliment = string.Empty;
        private byte[] _image;
        private IMvxPictureChooserTask _pictureChooserTask;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the command triggered when the add picture button is clicked.
        /// </summary>
        public IMvxCommand AddPictureButtonClickCommand { get; private set; }

        /// <summary>
        /// Gets or sets the compliment, if any.
        /// </summary>
        public string Compliment
        {
            get => _compliment;

            set
            {
                value ??= string.Empty;

                if (_compliment.Equals(value))
                    return;

                _compliment = value;

                RaisePropertyChanged(() => Compliment);
                RaisePropertyChanged(() => ShowCompliment);
            }
        }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        public byte[] Image
        {
            get => _image;

            set
            {
                if (_image == value)
                    return;

                _image = value;
                RaisePropertyChanged(() => Image);

                Recalculate();
            }
        }

        /// <summary>
        /// Gets the messaging service.
        /// </summary>
        public IMessagingService MessagingService => DialogMessaging.MessagingService.Instance;

        /// <summary>
        /// Gets whether or not to show the compliment.
        /// </summary>
        public bool ShowCompliment => !string.IsNullOrWhiteSpace(Compliment);
        #endregion

        #region Event Handlers
        private void AddPictureButton_Click()
        {
            var config = new ActionSheetBottomConfig
            {
                Title = Resources.TitleChooseAction,
                CancelButtonText = Resources.ActionCancel
            };

            config.Items.Add(new ActionSheetItemConfig { Text = Resources.HintChoosePicture, ClickAction = ChoosePicture });
            config.Items.Add(new ActionSheetItemConfig { Text = Resources.HintTakePicture, ClickAction = TakePicture });

            MessagingService.ActionSheetBottom(config);
        }

        private void Camera_ImageSelected(Stream rawImage)
        {
            using var memoryStream = new MemoryStream();

            rawImage.CopyTo(memoryStream);

            Image = memoryStream.ToArray();
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            AddPictureButtonClickCommand = new MvxCommand(AddPictureButton_Click);
        }

        public override void Prepare(AddPictureViewModelNavigationParams parameter)
        {
            base.Prepare(parameter);

            _calculateCompliment = parameter.CalculateCompliment;
            Title = parameter.Title;
        }
        #endregion

        #region Private Methods
        private void ChoosePicture()
        {
            // Resolve a task every time and keep an object reference to avoid unwanted garbage collection.
            _pictureChooserTask = Mvx.IoCProvider.Resolve<IMvxPictureChooserTask>();

            InvokeOnMainThread(() =>
            {
                _pictureChooserTask.ChoosePictureFromLibrary(ImageMaxPixelDimension, ImagePercentQuality, Camera_ImageSelected, () => { });
            });
        }

        private void Recalculate()
        {
            Compliment = Image == null || Image.Length < 1 ? string.Empty : _calculateCompliment?.Invoke();
        }

        private void TakePicture()
        {
            // Resolve a task every time and keep an object reference to avoid unwanted garbage collection.
            _pictureChooserTask = Mvx.IoCProvider.Resolve<IMvxPictureChooserTask>();

            InvokeOnMainThread(() =>
            {
                try
                {
                    _pictureChooserTask.TakePicture(ImageMaxPixelDimension, ImagePercentQuality, Camera_ImageSelected, () => { });
                }
                catch (Exception)
                {
                    MessagingService.Alert(new AlertConfig
                    {
                        Title = Resources.TitleOpenCameraFailed,
                        Message = Resources.ErrorOpeningCameraService,
                        OkButtonText = Resources.ActionOk
                    });
                }
            });
        }
        #endregion

        #region Constant Values
        public const int ImageMaxPixelDimension = 1080;
        public const int ImagePercentQuality = 100;
        #endregion
    }
}
