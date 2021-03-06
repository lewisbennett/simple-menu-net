﻿using MvvmCross;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.ViewModels.CreateThing.Base;

namespace SimpleMenu.Core.ViewModels.CreateThing
{
    public class CreateIngredientViewModel : CreateThingBaseViewModel
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
                NameHint = Resources.HintIngredientName,
                Title = Resources.MessageEnterIngredientName
            });

            ShowNextButton = true;
            Title = Resources.TitleCreateIngredient;
        }
        #endregion
    }
}
