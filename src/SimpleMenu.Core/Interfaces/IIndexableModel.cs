namespace SimpleMenu.Core.Interfaces
{
    public interface IIndexableModel
    {
        #region Properties
        /// <summary>
        /// Gets or sets the index of the model.
        /// </summary>
        int Index { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Saves the model.
        /// </summary>
        void Save();
        #endregion
    }
}
