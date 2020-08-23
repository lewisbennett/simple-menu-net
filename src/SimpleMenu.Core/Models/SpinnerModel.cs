using SimpleMenu.Core.Models.Base;

namespace SimpleMenu.Core.Models
{
    public partial class SpinnerModel : BaseModel
    {
        #region Properties
        /// <summary>
        /// An optional data payload.
        /// </summary>
        public object Data { get; set; }
        #endregion
    }
}
