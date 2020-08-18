using MvvmCross.ViewModels;
using SimpleMenu.Core.Services.Wrappers;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimpleMenu.Core.Data.Entities
{
    public class MealEntity : MvxNotifyPropertyChanged
    {
        #region Fields
        private Guid _imageUuid, _uuid;
        private string _name, _notes;
        private TimeSpan _preparationTime;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the image for this meal, if any.
        /// </summary>
        [JsonPropertyName("imageUuid")]
        public Guid ImageUUID
        {
            get => _imageUuid;

            set => SetProperty(ref _imageUuid, value);
        }

        /// <summary>
        /// Gets or sets the name of this meal.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name
        {
            get => _name;

            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// Gets or sets the notes for this meal, if any.
        /// </summary>
        [JsonPropertyName("notes")]
        public string Notes
        {
            get => _notes;

            set => SetProperty(ref _notes, value);
        }

        /// <summary>
        /// Gets or sets the preparation time for this meal.
        /// </summary>
        [JsonPropertyName("preparationTime")]
        public TimeSpan PreparationTime
        {
            get => _preparationTime;

            set => SetProperty(ref _preparationTime, value);
        }

        /// <summary>
        /// Gets or sets the UUID for this meal.
        /// </summary>
        [JsonPropertyName("uuid")]
        public Guid UUID
        {
            get => _uuid;

            set => SetProperty(ref _uuid, value);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets this meal's image.
        /// </summary>
        public byte[] GetImage()
            => FileServiceWrapper.Instance.ReadImage(FileServiceWrapper.ImagesDirectory, ImageUUID.ToString());

        /// <summary>
        /// Gets this meal's image.
        /// </summary>
        public Task<byte[]> GetImageAsync()
            => FileServiceWrapper.Instance.ReadImageAsync(FileServiceWrapper.ImagesDirectory, ImageUUID.ToString());
        #endregion
    }
}
