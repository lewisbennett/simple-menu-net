using SimpleMenu.Core.Interfaces;
using System.ComponentModel;

namespace SimpleMenu.Core.ViewModels.CreateThing.Base
{
    public interface ICreateThingStepViewModel : ICriteriaMet, INotifyPropertyChanged
    {
        #region Properties
        bool ShowNextButton { get; set; }
        #endregion
    }
}
