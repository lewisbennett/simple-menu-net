using DialogMessaging;
using DialogMessaging.Interactions;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.PictureChooser;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.ViewModels.Base;
using SimpleMenu.Core.ViewModels.CreateThing.Base;
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

    public partial class AddPictureViewModel : BaseViewModel<AddPictureViewModelNavigationParams>, ICreateThingStepViewModel
    {
        #region Fields
        private Func<string> _calculateCompliment;
        private IMvxPictureChooserTask _pictureChooserTask;
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

            MessagingService.Instance.ActionSheetBottom(config);
        }

        private void Camera_ImageSelected(Stream rawImage)
        {
            using var memoryStream = new MemoryStream();

            rawImage.CopyTo(memoryStream);

            Image = memoryStream.ToArray();
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(Compliment):
                    ShowCompliment = !string.IsNullOrWhiteSpace(Compliment);
                    return;

                case nameof(Image):

                    if (Image == null)
                    {
                        Compliment = null;
                        CriteriaMet = false;
                    }
                    else
                    {
                        Compliment = _calculateCompliment?.Invoke();
                        CriteriaMet = true;
                    }

                    return;

                default:
                    return;
            }
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

        public override void ViewCreated()
        {
            base.ViewCreated();

            ShowNextButton = true;
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
                    MessagingService.Instance.Alert(new AlertConfig
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
