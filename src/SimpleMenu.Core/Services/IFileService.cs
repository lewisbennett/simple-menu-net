namespace SimpleMenu.Core.Services
{
    public interface IFileService
    {
        #region Properties
        /// <summary>
        /// Gets the base storage path.
        /// </summary>
        string StoragePath { get; }
        #endregion
    }
}
