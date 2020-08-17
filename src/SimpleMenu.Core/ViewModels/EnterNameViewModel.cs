using SimpleMenu.Core.ViewModels.Base;

namespace SimpleMenu.Core.ViewModels
{
    public class EnterNameViewModelNavigationParams
    {
        #region Properties
        /// <summary>
        /// Gets or sets the entered name, if any.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the hint to display above the name field.
        /// </summary>
        public string NameHint { get; set; }

        /// <summary>
        /// Gets or sets the title for this step.
        /// </summary>
        public string Title { get; set; }
        #endregion
    }

    public class EnterNameViewModel : BaseViewModel<EnterNameViewModelNavigationParams>
    {
        #region Fields
        private string _name = string.Empty, _nameHint = string.Empty;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the entered name.
        /// </summary>
        public string Name
        {
            get => _name;

            set
            {
                value ??= string.Empty;

                if (_name.Equals(value))
                    return;

                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        /// <summary>
        /// Gets the placeholder text for the name field.
        /// </summary>
        public string NameHint
        {
            get => _nameHint;

            set
            {
                value ??= string.Empty;

                if (_nameHint.Equals(value))
                    return;

                _nameHint = value;
                RaisePropertyChanged(() => NameHint);
            }
        }
        #endregion

        #region Lifecycle
        public override void Prepare(EnterNameViewModelNavigationParams parameter)
        {
            base.Prepare(parameter);

            Name = parameter.Name;
            NameHint = parameter.NameHint;
            Title = parameter.Title;
        }
        #endregion
    }
}
