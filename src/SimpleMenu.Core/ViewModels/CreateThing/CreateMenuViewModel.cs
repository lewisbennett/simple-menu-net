using MvvmCross;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.ViewModels.CreateThing.Base;

namespace SimpleMenu.Core.ViewModels.CreateThing
{
    public class CreateMenuViewModel : CreateThingBaseViewModel
    {
        #region Properties
        /// <summary>
        /// Gets or sets the enter name view model.
        /// </summary>
        public EnterNameViewModel EnterNameViewModel { get; } = Mvx.IoCProvider.IoCConstruct<EnterNameViewModel>();
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates the thing and closes this view model.
        /// </summary>
        public override void CreateThingAndClose()
        {
        }
        #endregion

        #region Protected Methods
        protected override ICreateThingStepViewModel[] CreateSteps()
        {
            return new ICreateThingStepViewModel[]
            {
                EnterNameViewModel
            };
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            EnterNameViewModel.Prepare();

            EnterNameViewModel.Prepare(new EnterNameViewModelNavigationParams
            {
                NameHint = Resources.HintMenuName,
                Title = Resources.MessageEnterMenuName
            });   
        }

        public override void ViewCreated()
        {
            base.ViewCreated();

            ShowNextButton = true;
            Title = Resources.TitleCreateMenu;
        }
        #endregion
    }
}
