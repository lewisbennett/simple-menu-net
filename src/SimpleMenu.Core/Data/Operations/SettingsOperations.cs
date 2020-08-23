using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Services.Wrappers;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMenu.Core.Data.Operations
{
    public class SettingsOperations
    {
        #region Properties
        /// <summary>
        /// Gets the settings.
        /// </summary>
        public SettingsEntity Settings => FileServiceWrapper.Instance.Entities.FirstOrDefault(e => e is SettingsEntity) as SettingsEntity;
        #endregion

        #region Public Methods
        /// <summary>
        /// Loads the settings.
        /// </summary>
        public async Task<SettingsEntity> GetSettingsAsync()
        {
            var fileServiceWrapper = FileServiceWrapper.Instance;

            if (fileServiceWrapper.Entities.FirstOrDefault(e => e is SettingsEntity) is SettingsEntity settings)
                return settings;

            var doesExist = await fileServiceWrapper.DoesExistAsync(null, "settings.json").ConfigureAwait(false);

            if (doesExist)
                settings = await fileServiceWrapper.ReadJsonAsync<SettingsEntity>(null, "settings").ConfigureAwait(false);

            else
            {
                settings = new SettingsEntity();

                await SaveSettingsAsync(settings).ConfigureAwait(false);
            }

            fileServiceWrapper.AddEntity(settings);

            return settings;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public Task SaveSettingsAsync(SettingsEntity settings)
            => FileServiceWrapper.Instance.SaveJsonAsync(null, "settings", settings);
        #endregion

        #region Static Properties
        public static SettingsOperations Instance { get; } = new SettingsOperations();
        #endregion
    }
}
