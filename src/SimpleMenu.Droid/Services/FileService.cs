using SimpleMenu.Core.Services;
using System;

namespace SimpleMenu.Droid.Services
{
    public class FileService : IFileService
    {
        #region Properties
        /// <summary>
        /// Gets the base storage path.
        /// </summary>
        public string StoragePath => Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        #endregion
    }
}