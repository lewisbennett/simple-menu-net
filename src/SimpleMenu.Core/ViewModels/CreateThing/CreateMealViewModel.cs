using DialogMessaging;
using MvvmCross;
using MvvmCross.Navigation;
using SimpleMenu.Core.Data.Operations;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.ViewModels.CreateThing.Base;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SimpleMenu.Core.ViewModels.CreateThing
{
    public class CreateMealViewModel : CreateThingBaseViewModel
    {
        #region Fields
        private readonly IMvxNavigationService _navigationService;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the add picture view model.
        /// </summary>
        public AddPictureViewModel AddPictureViewModel { get; } = Mvx.IoCProvider.IoCConstruct<AddPictureViewModel>();

        /// <summary>
        /// Gets or sets the enter name view model.
        /// </summary>
        public EnterNameViewModel EnterNameViewModel { get; } = Mvx.IoCProvider.IoCConstruct<EnterNameViewModel>();
        #endregion

        #region Event Handlers
        private void EnterNameViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            AddPictureViewModel.ImageSearchCriteria = EnterNameViewModel.Name;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates the thing and closes this view model.
        /// </summary>
        public override async void CreateThingAndClose()
        {
            var messagingService = MessagingService.Instance;

            if (string.IsNullOrWhiteSpace(EnterNameViewModel.Name))
            {
                messagingService.Snackbar(Resources.ErrorEmptyMealName);
                return;
            }

            await messagingService.ShowLoadingAsync(Resources.MessagingCreatingMeal, Task.Run(() => MealOperations.Instance.CreateMealAsync(EnterNameViewModel.Name, AddPictureViewModel.Image))).ConfigureAwait(false);

            await _navigationService.Close(this).ConfigureAwait(false);

            messagingService.Toast(Resources.MessageCreateMealSuccess);
        }
        #endregion

        #region Protected Methods
        public override void AddEventHandlers()
        {
            base.AddEventHandlers();

            EnterNameViewModel.PropertyChanged += EnterNameViewModel_PropertyChanged;
        }

        protected override ICreateThingStepViewModel[] CreateSteps()
        {
            return new ICreateThingStepViewModel[]
            {
                EnterNameViewModel,
                AddPictureViewModel
            };
        }

        public override void RemoveEventHandlers()
        {
            base.RemoveEventHandlers();

            EnterNameViewModel.PropertyChanged -= EnterNameViewModel_PropertyChanged;
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            AddPictureViewModel.Prepare();
            EnterNameViewModel.Prepare();

            AddPictureViewModel.Prepare(new AddPictureViewModelNavigationParams
            {
                CalculateCompliment = () => PersonalizationOperations.Instance.GetRandomImageCompliment(false),
                Title = Resources.MessageAddMealPicture
            });

            EnterNameViewModel.Prepare(new EnterNameViewModelNavigationParams
            {
                NameHint = Resources.HintMealName,
                Title = Resources.MessageEnterMealName
            });

            ShowNextButton = true;
            Title = Resources.TitleCreateMeal;
        }
        #endregion

        #region Constructors
        public CreateMealViewModel(IMvxNavigationService navigationService)
            : base()
        {
            _navigationService = navigationService;
        }
        #endregion
    }
}
