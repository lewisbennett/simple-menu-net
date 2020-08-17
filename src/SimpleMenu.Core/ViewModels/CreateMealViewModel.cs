using DialogMessaging;
using MvvmCross;
using SimpleMenu.Core.Data.Operations;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.Services.Wrappers;
using SimpleMenu.Core.ViewModels.Base;

namespace SimpleMenu.Core.ViewModels
{
    public class CreateMealViewModel : CreateThingBaseViewModel
    {
        #region Properties
        /// <summary>
        /// Gets or sets the add picture view model.
        /// </summary>
        public AddPictureViewModel AddPictureViewModel { get; set; }

        /// <summary>
        /// Convenience property for CoreServiceWrapper.Instance.
        /// </summary>
        public CoreServiceWrapper CoreServiceWrapper => CoreServiceWrapper.Instance;

        /// <summary>
        /// Gets or sets the enter name view model.
        /// </summary>
        public EnterNameViewModel EnterNameViewModel { get; set; }

        /// <summary>
        /// Gets the messaging service.
        /// </summary>
        public IMessagingService MessagingService => DialogMessaging.MessagingService.Instance;

        /// <summary>
        /// Convenience property for PersonalizationOperations.Instance.
        /// </summary>
        public PersonalizationOperations PersonalizationOperations => PersonalizationOperations.Instance;
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates the thing and closes this view model.
        /// </summary>
        public override async void CreateThingAndClose()
        {
            if (string.IsNullOrWhiteSpace(EnterNameViewModel.Name))
            {
                MessagingService.Snackbar(Resources.ErrorEmptyMealName);
                return;
            }

            await MessagingService.ShowLoadingAsync(Resources.MessagingCreatingMeal, MealOperations.Instance.CreateMealAsync(EnterNameViewModel.Name, AddPictureViewModel.Image)).ConfigureAwait(false);

            await NavigationService.Close(this).ConfigureAwait(false);

            MessagingService.Toast(Resources.MessageCreateMealSuccess);
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            AddPictureViewModel = Mvx.IoCProvider.IoCConstruct<AddPictureViewModel>();
            EnterNameViewModel = Mvx.IoCProvider.IoCConstruct<EnterNameViewModel>();

            AddPictureViewModel.Prepare();
            EnterNameViewModel.Prepare();

            AddPictureViewModel.Prepare(new AddPictureViewModelNavigationParams
            {
                CalculateCompliment = () => PersonalizationOperations.GetRandomImageCompliment(false),
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
    }
}
