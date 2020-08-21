﻿using MvvmCross.Navigation;
using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Models;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.Services.Wrappers;
using SimpleMenu.Core.ViewModels.CreateThing;
using SimpleMenu.Core.ViewModels.List.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMenu.Core.ViewModels.List
{
    public class MenuListViewModel : RefreshableListBaseViewModel<MenuModel>
    {
        #region Fields
        private readonly IMvxNavigationService _navigationService;
        #endregion

        #region Event Handlers
        protected override void OnDataEmptyActionButtonClick()
        {
            base.OnDataEmptyActionButtonClick();

            NavigateToCreateMenuViewModel();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Navigates to the create meal view model.
        /// </summary>
        public void NavigateToCreateMenuViewModel()
        {
            _navigationService.Navigate<CreateMenuViewModel>();
        }
        #endregion

        #region Protected Methods
        protected override async Task LoadInitialPageAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);

            InvokeOnMainThread(() => UpdateCollection(CoreServiceWrapper.Instance.ActiveUser.Menus));
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            ShouldShowDataEmptyAction = true;

            DataEmptyActionButtonText = Resources.ActionAddOneNow;
            DataEmptyHint = Resources.HintNoMenusFound;
            LoadingHint = Resources.MessagingLoadingMenus;
            Title = Resources.TitleMenus;
        }

        public override void ViewCreated()
        {
            base.ViewCreated();

            UpdateCollection(CoreServiceWrapper.Instance.ActiveUser.Menus);
        }
        #endregion

        #region Constructors
        public MenuListViewModel(IMvxNavigationService navigationService)
            : base()
        {
            _navigationService = navigationService;
        }
        #endregion

        #region Private Methods
        private void UpdateCollection(IEnumerable<MenuEntity> menus)
        {
            var removals = Data.Where(d => !menus.Any(de => de.UUID == d.Entity.UUID)).ToArray();

            if (removals.Length > 0)
            {
                foreach (var removal in removals)
                    Data.Remove(removal);
            }

            var additions = menus.Where(d => !Data.Any(da => da.Entity.UUID == d.UUID)).ToArray();

            if (additions.Length > 0)
                Data.AddRange(additions.Select(a => new MenuModel { Entity = a }));
        }
        #endregion
    }
}
