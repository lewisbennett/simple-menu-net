using System;

namespace SimpleMenu.Core.Events
{
    public class ItemClickedEventArgs<TItem> : EventArgs
        where TItem : class
    {
        #region Properties
        /// <summary>
        /// Gets the clicked item.
        /// </summary>
        public TItem Item { get; }
        #endregion

        #region Constructors
        public ItemClickedEventArgs(TItem item)
        {
            Item = item;
        }
        #endregion
    }
}
