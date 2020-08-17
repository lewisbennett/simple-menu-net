using SimpleMenu.Core.Data.Entities.Base;
using SimpleMenu.Core.Services.Wrappers;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimpleMenu.Core.Data.Entities
{
    public class MealEntity : BaseEntity
    {
        #region Fields
        private Guid _imageUuid;
        private string _name = string.Empty, _notes = string.Empty;
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

            set
            {
                if (_imageUuid == value)
                    return;

                _imageUuid = value;
                OnPropertyChanged(nameof(ImageUUID));
            }
        }

        /// <summary>
        /// Gets or sets the name of this meal.
        /// </summary>
        [JsonPropertyName("name")]
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
        /// Gets or sets the notes for this meal, if any.
        /// </summary>
        [JsonPropertyName("notes")]
        public string Notes
        {
            get => _notes;

            set
            {
                value ??= string.Empty;

                if (_notes.Equals(value))
                    return;

                _notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }

        /// <summary>
        /// Gets or sets the preparation time for this meal.
        /// </summary>
        [JsonPropertyName("preparationTime")]
        public TimeSpan PreparationTime
        {
            get => _preparationTime;

            set
            {
                if (_preparationTime == value)
                    return;

                _preparationTime = value;
                OnPropertyChanged(nameof(PreparationTime));
            }
        }

        /// <summary>
        /// Gets or sets the UUID for this meal.
        /// </summary>
        [JsonPropertyName("uuid")]
        public Guid UUID { get; set; }
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
