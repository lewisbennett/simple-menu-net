using MvvmCross.ViewModels;
using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Services.Wrappers;
using SimpleMenu.Core.ViewModels;

namespace SimpleMenu.Core
{
    public class App : MvxApplication
    {
        #region Properties
        /// <summary>
        /// Convenience property for CoreServiceWrapper.Instance.
        /// </summary>
        public CoreServiceWrapper CoreServiceWrapper => CoreServiceWrapper.Instance;
        #endregion

        #region Lifecycle
        public override void Initialize()
        {
            base.Initialize();

            CoreServiceWrapper.ActiveUser = new UserEntity();

            var fileServiceWrapper = FileServiceWrapper.Instance;

            fileServiceWrapper.CreateDirectory(FileServiceWrapper.ImagesDirectory);
            fileServiceWrapper.CreateDirectory(FileServiceWrapper.MealsDirectory);

            RegisterAppStart<MainViewModel>();
        }
        #endregion
    }
}
