namespace SimpleMenu.Core.Models
{
    partial class TextModel
    {
        #region Fields
        private string _description, _title;
        private bool _showDescription;
        #endregion

        #region Properties
        public string Description
        {
            get => _description;

            set => SetProperty(ref _description, value);
        }

        public string Title
        {
            get => _title;

            set => SetProperty(ref _title, value);
        }

        public bool ShowDescription
        {
            get => _showDescription;

            set => SetProperty(ref _showDescription, value);
        }
        #endregion
    }
}
