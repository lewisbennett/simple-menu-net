using SimpleMenu.Core.Models.Base;

namespace SimpleMenu.Core.Models
{
    public partial class TextModel : BaseModel
    {
        #region Properties
        /// <summary>
        /// Gets or sets an optional data payload.
        /// </summary>
        public object Data { get; set; }
        #endregion

        #region Event Handlers
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
    }
}
