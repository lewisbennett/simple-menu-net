using MvvmCross;
using MvvmCross.Binding;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters;
using SimpleMenu.Core.Services;
using SimpleMenu.Droid.Helper;
using SimpleMenu.Droid.Services;

namespace SimpleMenu.Droid
{
    public class Setup : MvxAppCompatSetup<Core.App>
    {
        #region Public Methods
        protected override MvxBindingBuilder CreateBindingBuilder()
        {
            return new BindingBuilder();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new MvxAppCompatViewPresenter(AndroidViewAssemblies);
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.IoCProvider.RegisterSingleton<IFileService>(() => new FileService());
        }
        #endregion
    }
}