using SimpleMenu.Core.Data.Entities.Base;
using SimpleMenu.Core.Data.Operations;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimpleMenu.Core.Data.Entities
{
    public class MenuEntity : BaseEntity
    {
        #region Fields
        private Dictionary<DateTime, Guid> _meals;
        private string _name;
        private Guid _uuid;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the meals in this menu.
        /// </summary>
        [JsonPropertyName("meals")]
        public Dictionary<DateTime, Guid> Meals
        {
            get => _meals;

            set => SetProperty(ref _meals, value);
        }

        /// <summary>
        /// Gets or sets the name of this menu.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name
        {
            get => _name;

            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// Gets or sets the UUID for this menu.
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
        /// Saves the entity.
        /// </summary>
        public override Task SaveAsync()
            => MenuOperations.Instance.SaveMenuAsync(this);
        #endregion
    }
}
