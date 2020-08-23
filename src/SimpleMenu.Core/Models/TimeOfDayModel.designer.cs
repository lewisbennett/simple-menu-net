using SimpleMenu.Core.Schema;

namespace SimpleMenu.Core.Models
{
    partial class TimeOfDayModel
    {
        #region Fields
        private string _description, _title;
        private Icon _icon;
        private bool _showDescription;
        #endregion

        #region Properties
        public string Description
        {
            get => _description;

            set => SetProperty(ref _description, value);
        }

        public Icon Icon
        {
            get => _icon;

            set => SetProperty(ref _icon, value);
        }

        public bool ShowDescription
        {
            get => _showDescription;

            set => SetProperty(ref _showDescription, value);
        }

        public string Title
        {
            get => _title;

            set => SetProperty(ref _title, value);
        }
        #endregion
    }
}
