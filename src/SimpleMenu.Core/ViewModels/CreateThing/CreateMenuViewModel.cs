using DialogMessaging;
using DialogMessaging.Interactions;
using MvvmCross;
using MvvmCross.Navigation;
using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Data.Operations;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.ViewModels.CreateThing.Base;
using SimpleMenu.Core.ViewModels.List;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMenu.Core.ViewModels.CreateThing
{
    public class CreateMenuViewModel : CreateThingBaseViewModel
    {
        #region Fields
        private readonly IMvxNavigationService _navigationService;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the enter name view model.
        /// </summary>
        public EnterNameViewModel EnterNameViewModel { get; } = Mvx.IoCProvider.IoCConstruct<EnterNameViewModel>();

        /// <summary>
        /// Gets the menu meal list view model.
        /// </summary>
        public MenuMealListViewModel MenuMealListViewModel { get; } = Mvx.IoCProvider.IoCConstruct<MenuMealListViewModel>();

        /// <summary>
        /// Gets the select time of day view model.
        /// </summary>
        public SelectTimeOfDayViewModel SelectTimeOfDayViewModel { get; } = Mvx.IoCProvider.IoCConstruct<SelectTimeOfDayViewModel>();
        #endregion

        #region Event Handlers
        protected override void OnNextButtonClicked()
        {
            if (CurrentStep == SelectTimeOfDayViewModel && MenuMealListViewModel.Data.Count < 1)
            {
                var config = new ActionSheetBottomConfig
                {
                    Title = Resources.TitleChooseDateRange,
                    CancelButtonText = Resources.ActionCancel,
                    ItemClickAction = (item) =>
                    {
                        MenuMealListViewModel.GenerateRandomMenu(item.Data is int days ? days : 0);

                        base.OnNextButtonClicked();
                    }
                };

                config.Items.Add(new ActionSheetItemConfig { Text = Resources.HintNextFiveDays, Data = 5 });
                config.Items.Add(new ActionSheetItemConfig { Text = Resources.HintNextSevenDays, Data = 7 });
                config.Items.Add(new ActionSheetItemConfig { Text = Resources.HintStartFromScratch, Data = 0 });

                MessagingService.Instance.ActionSheetBottom(config);

                return;
            }
            else if (CurrentStep == MenuMealListViewModel)
            {
                var startDate = MenuMealListViewModel.Dates.First();
                var finishDate = MenuMealListViewModel.Dates.Last();

                var startDateString = startDate.ToString("MMM dd");
                var finishDateString = finishDate.ToString("MMM dd");

                if (startDate.Year != finishDate.Year)
                {
                    startDateString = $"{startDateString} {startDate:yyyy}";
                    finishDateString = $"{finishDateString} {finishDate:yyyy}";
                }

                if (startDateString.Equals(finishDateString))
                    EnterNameViewModel.Name = startDateString;
                else
                    EnterNameViewModel.Name = string.Format(Resources.HintToFrom, startDateString, finishDateString);
            }

            base.OnNextButtonClicked();
        }

        private void SelectTimeOfDayViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectTimeOfDayViewModel.SelectedTimeOfDay):

                    if (SelectTimeOfDayViewModel.SelectedTimeOfDay.Data is TimeOfDayEntity timeOfDay)
                        MenuMealListViewModel.TimeOfDay = timeOfDay;

                    return;

                default:
                    return;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds the event handlers for this ViewModel.
        /// </summary>
        public override void AddEventHandlers()
        {
            base.AddEventHandlers();

            SelectTimeOfDayViewModel.PropertyChanged += SelectTimeOfDayViewModel_PropertyChanged;
        }

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

            var meals = MenuMealListViewModel.Data.Select(d => new MenuMealEntity { DateTime = MenuMealListViewModel.Dates[d.Index], MealUUID = d.Entity.UUID }).ToArray();

            await messagingService.ShowLoadingAsync(Resources.MessagingCreatingMenu, Task.Run(() => MenuOperations.Instance.CreateMenuAsync(EnterNameViewModel.Name, meals))).ConfigureAwait(false);

            await _navigationService.Close(this).ConfigureAwait(false);
            
            messagingService.Toast(Resources.MessageCreateMenuSuccess);
        }

        /// <summary>
        /// Removes the event handlers from this ViewModel.
        /// </summary>
        public override void RemoveEventHandlers()
        {
            base.RemoveEventHandlers();

            SelectTimeOfDayViewModel.PropertyChanged -= SelectTimeOfDayViewModel_PropertyChanged;
        }
        #endregion

        #region Protected Methods
        protected override ICreateThingStepViewModel[] CreateSteps()
        {
            return new ICreateThingStepViewModel[]
            {
                SelectTimeOfDayViewModel,
                MenuMealListViewModel,
                EnterNameViewModel
            };
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            EnterNameViewModel.Prepare();
            MenuMealListViewModel.Prepare();
            SelectTimeOfDayViewModel.Prepare();

            EnterNameViewModel.Prepare(new EnterNameViewModelNavigationParams
            {
                NameHint = Resources.HintMenuName,
                Title = Resources.MessageEnterMenuName
            });

            ShowNextButton = true;
            Title = Resources.TitleCreateMenu;
        }
        #endregion

        #region Constructors
        public CreateMenuViewModel(IMvxNavigationService navigationService)
            : base()
        {
            _navigationService = navigationService;
        }
        #endregion
    }
}
