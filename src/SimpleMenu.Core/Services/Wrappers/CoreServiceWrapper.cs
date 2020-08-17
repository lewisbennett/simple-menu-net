using SimpleMenu.Core.Data.Entities;

namespace SimpleMenu.Core.Services.Wrappers
{
    public class CoreServiceWrapper
    {
        #region Properties
        /// <summary>
        /// Gets or sets the active user.
        /// </summary>
        public UserEntity ActiveUser { get; set; }
        #endregion

        #region Static Properties
        public static CoreServiceWrapper Instance { get; } = new CoreServiceWrapper();
        #endregion
    }
}
