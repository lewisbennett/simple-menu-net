﻿using MvvmCross;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.ViewModels.CreateThing.Base;
using SimpleMenu.Core.ViewModels.List;
using System.Linq;

namespace SimpleMenu.Core.ViewModels.CreateThing
{
    public class CreateMenuViewModelNavigationParams
    {
        #region Properties
        /// <summary>
        /// Gets or sets the number of days to create a menu for.
        /// </summary>
        public int Days { get; set; }
        #endregion
    }

    public class CreateMenuViewModel : CreateThingBaseViewModel<CreateMenuViewModelNavigationParams>
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
            if (CurrentStep == MenuMealListViewModel)
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

            EnterNameViewModel.Prepare(new EnterNameViewModelNavigationParams
            {
                NameHint = Resources.HintMenuName,
                Title = Resources.MessageEnterMenuName
            });

            ShowNextButton = true;
            Title = Resources.TitleCreateMenu;
        }

        public override void Prepare(CreateMenuViewModelNavigationParams parameter)
        {
            base.Prepare(parameter);

            if (parameter.Days > 0)
                MenuMealListViewModel.GenerateRandomMenu(parameter.Days);
        }
        #endregion
    }
}
