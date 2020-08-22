using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Data.Operations;
using SimpleMenu.Core.Interfaces;
using SimpleMenu.Core.Models.Base;
using System;
using System.Threading.Tasks;

namespace SimpleMenu.Core.Models
{
    public partial class MenuMealModel : EntityDisplayBaseModel<MealEntity>, IIndexable
    {
        #region Fields
        private int _index;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the dates for the menu.
        /// </summary>
        public DateTime[] Dates { get; set; }

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

        /// <summary>
        /// Gets whether the image is currently being loaded.
        /// </summary>
        public bool IsLoadingImage { get; private set; }
        #endregion

        #region Event Handlers
        protected override void OnEntityChanged()
        {
            base.OnEntityChanged();

            Description = CalculateDescription();
            Title = CalculateTitle();

            LoadImage();
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            base.OnEntityPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(Entity.ImageUUID):
                    LoadImage();
                    return;

                case nameof(Entity.Name):
                    Title = CalculateTitle();
                    return;

                default:
                    return;
            }
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

        #region Public Methods
        /// <summary>
        /// Loads the meal's image.
        /// </summary>
        public async void LoadImage()
        {
            if (IsLoadingImage)
                return;

            IsLoadingImage = ShowLoading = true;

            Image = await Task.Run(() => ImageOperations.Instance.GetImageAsync(Entity.ImageUUID, 300, 300).Result).ConfigureAwait(false);

            IsLoadingImage = ShowLoading = false;
        }
        #endregion

        #region Private Methods
        private string CalculateDescription()
            => null;

        private string CalculateTitle()
            => Entity.Name;

        private void Recalculate()
        {
            var date = Dates[Index];

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
