using SimpleMenu.Core.Data.Entities.Base;
using SimpleMenu.Core.Data.Operations;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimpleMenu.Core.Data.Entities
{
    public class MealEntity : BaseEntity
    {
        #region Fields
        private Guid _imageUuid, _uuid;
        private int _index;
        private string _name, _notes;
        private long _preparationTime;
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
        /// Gets or sets the index of this meal.
        /// </summary>
        public int Index
        {
            get => _index;

            set => SetProperty(ref _index, value);
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
        /// Gets or sets the preparation time for this meal, in ticks.
        /// </summary>
        [JsonPropertyName("preparationTime")]
        public long PreparationTime
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
        /// Gets the preparation time as a <see cref="TimeSpan" />.
        /// </summary>
        public TimeSpan GetPreparationTime()
            => TimeSpan.FromTicks(_preparationTime);

        /// <summary>
        /// Saves the entity.
        /// </summary>
        public override Task SaveAsync()
            => MealOperations.Instance.SaveMealAsync(this);
        #endregion
    }
}
