using Android.Widget;
using MvvmCross;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters;
using SimpleMenu.Core.Services;
using SimpleMenu.Droid.Helper;
using SimpleMenu.Droid.Services;
using SimpleMenu.Droid.TargetBindings;

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

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterCustomBindingFactory<ImageView>("ResourceId", (imageView) => new ImageViewResourceIdTargetBinding(imageView));
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.IoCProvider.RegisterSingleton<IFileService>(() => new FileService());
        }
        #endregion
    }
}