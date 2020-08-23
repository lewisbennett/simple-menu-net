using SimpleMenu.Core.Schema;

namespace SimpleMenu.Core.Models
{
    partial class TextIconModel
    {
        #region Fields
        private Icon _icon;
        #endregion

        #region Properties
        public Icon Icon
        {
            get => _icon;

            set => SetProperty(ref _icon, value);
        }
        #endregion
    }
}
