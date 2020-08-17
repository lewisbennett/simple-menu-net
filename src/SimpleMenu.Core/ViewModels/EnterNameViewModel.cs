using SimpleMenu.Core.ViewModels.Base;
using SimpleMenu.Core.ViewModels.CreateThing.Base;

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

    public partial class EnterNameViewModel : BaseViewModel<EnterNameViewModelNavigationParams>, ICreateThingStepViewModel
    {
        #region Event Handlers
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(Name):
                    CriteriaMet = !string.IsNullOrWhiteSpace(Name);
                    return;

                default:
                    return;
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

        public override void ViewCreated()
        {
            base.ViewCreated();

            ShowNextButton = true;
        }
        #endregion
    }
}
