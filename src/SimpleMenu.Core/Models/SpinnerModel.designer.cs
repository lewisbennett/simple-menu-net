namespace SimpleMenu.Core.Models
{
    partial class SpinnerModel
    {
        #region Fields
        private string _text;
        #endregion

        #region Properties
        public string Text
        {
            get => _text;

            set => SetProperty(ref _text, value);
        }
        #endregion
    }
}
