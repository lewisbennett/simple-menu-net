using DialogMessaging;
using DialogMessaging.Interactions;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Data.Operations;
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
    public partial class MenuListViewModel : RefreshableListBaseViewModel<MenuModel>
    {
        #region Fields
        private readonly IMvxNavigationService _navigationService;
        #endregion

        #region Event Handlers
        private void AddMenuButton_Click()
        {
            NavigateToCreateMenuViewModel();
        }

        protected override void OnDataEmptyActionButtonClick()
        {
            base.OnDataEmptyActionButtonClick();

            NavigateToCreateMenuViewModel();
        }

        protected override void OnItemClicked(MenuModel item)
        {
            base.OnItemClicked(item);

            // Temporary.
            MessagingService.Instance.Delete(new DeleteConfig
            {
                Title = Resources.TitleConfirmDeleteMenu,
                Message = Resources.MessageConfirmDeleteMenu,
                DeleteButtonText = Resources.ActionDelete,
                CancelButtonText = Resources.ActionCancel,
                DeleteButtonClickAction = () => DeleteMenu(item.Entity)
            });
        }
        #endregion

        #region Protected Methods
        protected override async Task LoadInitialPageAsync()
        {
            var menus = await MenuOperations.Instance.ListAllMenusAsync().ConfigureAwait(false);

            InvokeOnMainThread(() => UpdateCollection(menus));
        }
        #endregion

        #region Lifecycle
        public override void Prepare()
        {
            base.Prepare();

            AddMenuButtonClickCommand = new MvxCommand(AddMenuButton_Click);

            ShouldShowDataEmptyAction = true;

            DataEmptyActionButtonText = Resources.ActionAddOneNow;
            DataEmptyHint = Resources.HintNoMenusFound;
            LoadingHint = Resources.MessagingLoadingMenus;
            Title = Resources.TitleMenus;
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
        private async void DeleteMenu(MenuEntity menu)
        {
            await MessagingService.Instance.ShowLoadingAsync(Resources.MessagingDeletingMenu, DeleteMenuAsync(menu.UUID)).ConfigureAwait(false);
        }

        private async Task DeleteMenuAsync(Guid menuUuid)
        {
            await MenuOperations.Instance.DeleteMenuAsync(menuUuid).ConfigureAwait(false);

            await LoadInitialPageAsync().ConfigureAwait(false);
        }

        private void NavigateToCreateMenuViewModel()
        {
            if (FileServiceWrapper.Instance.Entities.Count(e => e is MealEntity) < 2)
            {
                MessagingService.Instance.Alert(new AlertConfig
                {
                    Title = Resources.ErrorNotEnoughMeals,
                    Message = Resources.MessageNotEnoughMeals,
                    OkButtonText = Resources.ActionOk
                });

                return;
            }

            var config = new ActionSheetBottomConfig
            {
                Title = Resources.TitleChooseDateRange,
                CancelButtonText = Resources.ActionCancel,
                ItemClickAction = (item) =>
                {
                    _navigationService.Navigate<CreateMenuViewModel, CreateMenuViewModelNavigationParams>(new CreateMenuViewModelNavigationParams
                    {
                        Days = item.Data is int days ? days : 0
                    });
                }
            };

            config.Items.Add(new ActionSheetItemConfig { Text = Resources.HintNextFiveDays, Data = 5 });
            config.Items.Add(new ActionSheetItemConfig { Text = Resources.HintNextSevenDays, Data = 7 });
            config.Items.Add(new ActionSheetItemConfig { Text = Resources.HintStartFromScratch, Data = 0 });

            MessagingService.Instance.ActionSheetBottom(config);
        }

        private void UpdateCollection(IEnumerable<MenuEntity> menus)
        {
            var removals = Data.Where(d => !menus.Any(de => de.UUID == d.Entity.UUID)).ToArray();

            if (removals.Length > 0)
            {
                foreach (var removal in removals)
                    Data.Remove(removal);
            }

            var additions = menus.Where(d => !Data.Any(da => da.Entity.UUID == d.UUID)).OrderByDescending(m => m.Index).ToArray();

            foreach (var addition in additions)
                Data.Insert(0, new MenuModel { Entity = addition });
        }
        #endregion
    }
}
