using SimpleMenu.Core.Data.Entities.Base;
using System;
using System.Collections.Generic;

namespace SimpleMenu.Core.Data.Entities
{
    public class UserEntity : BaseEntity
    {
        #region Fields
        private string _familyName = string.Empty, _givenName = string.Empty;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the family (last) name for this user.
        /// </summary>
        public string FamilyName
        {
            get => _familyName;

            set
            {
                value ??= string.Empty;

                if (_familyName.Equals(value))
                    return;

                _familyName = value;
                OnPropertyChanged(nameof(FamilyName));
            }
        }

        /// <summary>
        /// Gets or sets the given (first) name for this user.
        /// </summary>
        public string GivenName
        {
            get => _givenName;

            set
            {
                value ??= string.Empty;

                if (_givenName.Equals(value))
                    return;

                _givenName = value;
                OnPropertyChanged(nameof(GivenName));
            }
        }

        /// <summary>
        /// Gets the ingredients for this user.
        /// </summary>
        public ICollection<IngredientEntity> Ingredients { get; } = new List<IngredientEntity>();

        /// <summary>
        /// Gets the meals for this user.
        /// </summary>
        public ICollection<MealEntity> Meals { get; } = new List<MealEntity>();

        /// <summary>
        /// Gets the menus for this user.
        /// </summary>
        public ICollection<MenuEntity> Menus { get; } = new List<MenuEntity>();

        /// <summary>
        /// Gets or sets the UUID for this user.
        /// </summary>
        public Guid UUID { get; set; }
        #endregion
    }
}
