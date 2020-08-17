namespace SimpleMenu.Core.ViewModels
{
    partial class EnterNameViewModel
    {
        #region Fields
        private bool _criteriaMet, _showNextButton;
        private string _name, _nameHint;
        #endregion

        #region Properties
        public bool CriteriaMet
        {
            get => _criteriaMet;

            set => SetProperty(ref _criteriaMet, value);
        }

        public string Name
        {
            get => _name;

            set => SetProperty(ref _name, value);
        }

        public string NameHint
        {
            get => _nameHint;

            set => SetProperty(ref _nameHint, value);
        }

        public bool ShowNextButton
        {
            get => _showNextButton;

            set => SetProperty(ref _showNextButton, value);
        }
        #endregion
    }
}
