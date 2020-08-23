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
        #region Properties
        /// <summary>
        /// Gets the cached menus.
        /// </summary>
        public MenuEntity[] MenuCache => FileServiceWrapper.Instance.Entities.Where(e => e is MenuEntity).Cast<MenuEntity>().ToArray();
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates a new menu.
        /// </summary>
        /// <param name="name">The name of the menu.</param>
        /// <param name="meals">The dates and meals for the menu.</param>
        public async Task<MenuEntity> CreateMenuAsync(string name, params MenuMealEntity[] meals)
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
        public async Task DeleteMenuAsync(Guid uuid)
        {
            var fileServiceWrapper = FileServiceWrapper.Instance;

            var fileName = $"{uuid}.json";

            var doesExist = await fileServiceWrapper.DoesExistAsync(FileServiceWrapper.MenusDirectory, fileName).ConfigureAwait(false);

            if (doesExist)
            {
                await fileServiceWrapper.DeleteFileAsync(FileServiceWrapper.MenusDirectory, fileName).ConfigureAwait(false);

                var localMenu = fileServiceWrapper.Entities.FirstOrDefault(e => e is MenuEntity m && m.UUID == uuid);

                if (localMenu != null)
                    fileServiceWrapper.RemoveEntity(localMenu);
            }
        }

        /// <summary>
        /// Gets a menu. May return null if the menu if the menu is no longer available.
        /// </summary>
        /// <param name="menuUuid">The UUID of the menu.</param>
        public async ValueTask<MenuEntity> GetMenuAsync(Guid menuUuid)
        {
            var fileServiceWrapper = FileServiceWrapper.Instance;

            if (fileServiceWrapper.Entities.FirstOrDefault(e => e is MenuEntity m && m.UUID == menuUuid) is MenuEntity menu)
                return menu;

            menu = await FileServiceWrapper.Instance.ReadJsonAsync<MenuEntity>(FileServiceWrapper.MenusDirectory, menuUuid.ToString()).ConfigureAwait(false);

            // If the menu has run out of valid dates, delete and nullify it.
            if (!menu.Meals.Any(m => m.DateTime >= DateTime.Now.Date))
            {
                await DeleteMenuAsync(menu.UUID).ConfigureAwait(false);

                menu = null;
            }
            else
                FileServiceWrapper.Instance.AddEntity(menu);

            return menu;
        }

        /// <summary>
        /// List all of the user's menus.
        /// </summary>
        public async Task<MenuEntity[]> ListAllMenusAsync()
        {
            var savedMenus = await FileServiceWrapper.Instance.ListFilesAsync(FileServiceWrapper.MenusDirectory).ConfigureAwait(false);

            var menus = new List<MenuEntity>();

            foreach (var savedMenu in savedMenus)
            {
                Guid menuUuid;

                if (savedMenu.EndsWith(".json"))
                {
                    // Remove ".json".
                    var mutableSavedMenu = savedMenu.Substring(0, savedMenu.Length - 5);

                    // Remove the path.
                    mutableSavedMenu = mutableSavedMenu.Substring(mutableSavedMenu.Length - 36);

                    menuUuid = Guid.Parse(mutableSavedMenu);
                }
                else
                    menuUuid = Guid.Parse(savedMenu);

                var menu = await GetMenuAsync(menuUuid).ConfigureAwait(false);
                
                if (menu != null)
                    menus.Add(menu);
            }

            return menus.ToArray();
        }

        /// <summary>
        /// Saves a menu.
        /// </summary>
        /// <param name="menu">The menu to save.</param>
        public Task SaveMenuAsync(MenuEntity menu)
            => FileServiceWrapper.Instance.SaveJsonAsync(FileServiceWrapper.MenusDirectory, menu.UUID.ToString(), menu);
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
