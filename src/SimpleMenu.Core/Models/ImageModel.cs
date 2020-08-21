using SimpleMenu.Core.Models.Base;

namespace SimpleMenu.Core.Models
{
    public partial class ImageModel : BaseModel
    {
        #region Properties
        /// <summary>
        /// Gets or sets the URI of the image being shown.
        /// </summary>
        public string ImageUri { get; set; }
        #endregion
    }
}
