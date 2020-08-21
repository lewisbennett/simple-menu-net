﻿using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Interfaces;
using SimpleMenu.Core.Models.Base;
using System;

namespace SimpleMenu.Core.Models
{
    public partial class MenuMealModel : EntityDisplayBaseModel<MealEntity>, IIndexable
    {
        #region Fields
        private readonly DateTime[] _dates;
        private int _index;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the index of the model.
        /// </summary>
        public int Index
        {
            get => _index;

            set
            {
                _index = value;
                Recalculate();
            }
        }
        #endregion

        #region Event Handlers
        protected override void OnEntityChanged()
        {
            base.OnEntityChanged();

            Description = CalculateDescription();
            Image = CalculateImage();
            Title = CalculateTitle();
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(Description):
                    ShowDescription = !string.IsNullOrWhiteSpace(Description);
                    return;

                default:
                    return;
            }
        }
        #endregion

        #region Constructors
        public MenuMealModel(DateTime[] dates)
            : base()
        {
            _dates = dates;
        }
        #endregion

        #region Private Methods
        private string CalculateDescription()
            => null;

        private byte[] CalculateImage()
            => Entity.GetImage();

        private string CalculateTitle()
            => Entity.Name;

        private void Recalculate()
        {
            var date = _dates[Index];

            var day = date.Day switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                21 => "st",
                22 => "nd",
                23 => "rd",
                31 => "st",
                _ => "th"
            };

            DateTitle = $"{date.ToString($"dddd d")}{day} {date:MMMM}";
        }
        #endregion
    }
}
