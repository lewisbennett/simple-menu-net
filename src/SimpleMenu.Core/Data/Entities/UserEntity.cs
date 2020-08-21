using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;

namespace SimpleMenu.Core.Data.Entities
{
    public class UserEntity : MvxNotifyPropertyChanged
    {
        #region Fields
        private string _familyName, _givenName;
        private Guid _uuid;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the family (last) name for this user.
        /// </summary>
        public string FamilyName
        {
            get => _familyName;

            set => SetProperty(ref _familyName, value);
        }

        /// <summary>
        /// Gets or sets the given (first) name for this user.
        /// </summary>
        public string GivenName
        {
            get => _givenName;

            set => SetProperty(ref _givenName, value);
        }

        /// <summary>
        /// Gets the ingredients for this user.
        /// </summary>
        public IList<IngredientEntity> Ingredients { get; } = new List<IngredientEntity>();

        /// <summary>
        /// Gets the meals for this user.
        /// </summary>
        public IList<MealEntity> Meals { get; } = new List<MealEntity>();

        /// <summary>
        /// Gets the menus for this user.
        /// </summary>
        public IList<MenuEntity> Menus { get; } = new List<MenuEntity>();

        /// <summary>
        /// Gets or sets the UUID for this user.
        /// </summary>
        public Guid UUID
        {
            get => _uuid;

            set => SetProperty(ref _uuid, value);
        }
        #endregion
    }
}
