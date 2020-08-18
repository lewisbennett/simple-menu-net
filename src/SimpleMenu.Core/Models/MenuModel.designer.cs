namespace SimpleMenu.Core.Models
{
    partial class MenuModel
    {
        #region Fields
        private string _description, _title;
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
        #endregion
    }
}
