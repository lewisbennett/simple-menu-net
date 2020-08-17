using MvvmCross.Binding;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters;
using SimpleMenu.Droid.Helper;

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
        #endregion
    }
}