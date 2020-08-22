using DialogMessaging;
using DialogMessaging.Interactions;
using MvvmCross;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.ViewModels.CreateThing.Base;
using SimpleMenu.Core.ViewModels.List;
using System;
using System.Collections.Generic;

namespace SimpleMenu.Core.ViewModels.CreateThing
{
    public class CreateMenuViewModel : CreateThingBaseViewModel
    {
        #region Properties
        /// <summary>
        /// Gets the enter name view model.
        /// </summary>
        public EnterNameViewModel EnterNameViewModel { get; } = Mvx.IoCProvider.IoCConstruct<EnterNameViewModel>();

        /// <summary>
        /// Gets the menu meal list view model.
        /// </summary>
        public MenuMealListViewModel MenuMealListViewModel { get; } = Mvx.IoCProvider.IoCConstruct<MenuMealListViewModel>();
        #endregion

        #region Event Handlers
        protected override void OnNextButtonClicked()
        {
            if (CurrentStep == EnterNameViewModel)
            {
                var config = new ActionSheetBottomConfig
                {
                    Title = Resources.TitleChooseDateRange,
                    CancelButtonText = Resources.ActionCancel,
                    ItemClickAction = (item) =>
                    {
                        base.OnNextButtonClicked();

                        if (item.Data is int days)
                        {
                            var dates = new List<DateTime>();

                            for (var i = 0; i < days; i++)
                                dates.Add(DateTime.Now.Date.AddDays(i));

                            MenuMealListViewModel.Dates = dates.ToArray();

                            MenuMealListViewModel.LoadInitialPage();
                        }
                    }
                };

                config.Items.Add(new ActionSheetItemConfig { Text = Resources.HintNextFiveDays, Data = 5 });
                config.Items.Add(new ActionSheetItemConfig { Text = Resources.HintNextSevenDays, Data = 7 });
                config.Items.Add(new ActionSheetItemConfig { Text = Resources.HintStartFromScratch, Data = 0 });

                MessagingService.Instance.ActionSheetBottom(config);
            }
            else
                base.OnNextButtonClicked();
        }
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
                EnterNameViewModel,
                MenuMealListViewModel
            };
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            EnterNameViewModel.Prepare();
            MenuMealListViewModel.Prepare();

            EnterNameViewModel.Prepare(new EnterNameViewModelNavigationParams
            {
                NameHint = Resources.HintMenuName,
                Title = Resources.MessageEnterMenuName
            });

            ShowNextButton = true;
            Title = Resources.TitleCreateMenu;
        }
        #endregion
    }
}
