using SimpleMenu.Core.Data.Entities.Base;
using System;

namespace SimpleMenu.Core.Data.Entities
{
    public class MealEntity : BaseEntity
    {
        #region Fields
        private byte[] _image;
        private string _name = string.Empty, _notes = string.Empty;
        private long _preparationTime;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the image for this meal, if any.
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
        /// Gets or sets the name of this meal.
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
        /// Gets or sets the notes for this meal, if any.
        /// </summary>
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
        public long PreparationTime
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
        public Guid UUID { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the preparation time for this meal as a time span.
        /// </summary>
        public TimeSpan GetPreparationTimeSpan()
        {
            //return TimeSpan.FromMinutes(PreparationTime);

            return TimeSpan.FromMinutes(new Random().Next(0, 200));
        }
        #endregion
    }
}
