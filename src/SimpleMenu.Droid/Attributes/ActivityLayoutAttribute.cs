using System;

namespace SimpleMenu.Droid.Attributes
{
    public sealed class ActivityLayoutAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// Gets or sets whether or not the back button should be visible.
        /// </summary>
        public bool EnableBackButton { get; set; }

        /// <summary>
        /// Gets or sets the resource ID of the layout file to display.
        /// </summary>
        public int LayoutResourceID { get; set; }

        /// <summary>
        /// Gets or sets the resource ID of the menu to be displayed.
        /// </summary>
        /// <remarks>The activity must have a valid SupportActionBar to display the menu.</remarks>
        public int MenuResourceID { get; set; }
        #endregion
    }
}