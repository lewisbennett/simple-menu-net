using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Services.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMenu.Core.Data.Operations
{
    public class MenuOperations : IEqualityComparer<MenuEntity>
    {
        #region Public Methods
        /// <summary>
        /// Creates a new menu.
        /// </summary>
        /// <param name="name">The name of the menu.</param>
        /// <param name="meals">The dates and meals for the menu.</param>
        public async Task<MenuEntity> CreateMealAsync(string name, Dictionary<DateTime, Guid> meals)
        {
            var menu = new MenuEntity
            {
                Meals = meals,
                Name = name,
                UUID = Guid.NewGuid()
            };

            await SaveMenuAsync(menu).ConfigureAwait(false);

            FileServiceWrapper.Instance.AddEntity(menu);

            return menu;
        }

        /// <summary>
        /// Deletes a menu.
        /// </summary>
        /// <param name="uuid">The UUID of the menu to delete.</param>
        public async Task DeleteMealAsync(Guid uuid)
        {
            var fileServiceWrapper = FileServiceWrapper.Instance;

            var fileName = $"{uuid}.json";

            var doesExist = await fileServiceWrapper.DoesExistAsync(FileServiceWrapper.MenusDirectory, fileName).ConfigureAwait(false);

            if (doesExist)
            {
                await fileServiceWrapper.DeleteFileAsync(FileServiceWrapper.MenusDirectory, fileName).ConfigureAwait(false);

                var coreServiceWrapper = CoreServiceWrapper.Instance;

                var localMenu = coreServiceWrapper.ActiveUser.Menus.FirstOrDefault(m => m.UUID == uuid);

                if (localMenu != null)
                {
                    coreServiceWrapper.ActiveUser.Menus.Remove(localMenu);

                    fileServiceWrapper.RemoveEntity(localMenu);
                }
            }
        }

        /// <summary>
        /// List all of the user's menus.
        /// </summary>
        public async Task<MenuEntity[]> ListAllMealsAsync()
        {
            var coreServiceWrapper = CoreServiceWrapper.Instance;
            var fileServiceWrapper = FileServiceWrapper.Instance;

            var savedMenuFiles = await fileServiceWrapper.ListFilesAsync(FileServiceWrapper.MenusDirectory).ConfigureAwait(false);

            var menus = new List<MenuEntity>(coreServiceWrapper.ActiveUser.Menus);

            foreach (var savedMenuFile in savedMenuFiles)
            {
                var menu = await fileServiceWrapper.ReadJsonAsync<MenuEntity>(FileServiceWrapper.MenusDirectory, savedMenuFile).ConfigureAwait(false);

                menus.Add(menu);
            }

            var finalMenus = menus.Distinct(this).ToArray();

            coreServiceWrapper.ActiveUser.Menus.Clear();

            foreach (var finalMenu in finalMenus)
            {
                coreServiceWrapper.ActiveUser.Menus.Add(finalMenu);

                await SaveMenuAsync(finalMenu).ConfigureAwait(false);

                fileServiceWrapper.AddEntity(finalMenu);
            }

            return finalMenus;
        }

        /// <summary>
        /// Saves a menu.
        /// </summary>
        /// <param name="menu">The menu to save.</param>
        public Task SaveMenuAsync(MenuEntity menu)
            => FileServiceWrapper.Instance.SaveJsonAsync(FileServiceWrapper.MealsDirectory, menu.UUID.ToString(), menu);
        #endregion

        #region Equality Methods
        bool IEqualityComparer<MenuEntity>.Equals(MenuEntity x, MenuEntity y)
            => x.UUID == y.UUID;

        int IEqualityComparer<MenuEntity>.GetHashCode(MenuEntity obj)
            => obj.GetHashCode();
        #endregion

        #region Static Properties
        public static MenuOperations Instance { get; } = new MenuOperations();
        #endregion
    }
}
