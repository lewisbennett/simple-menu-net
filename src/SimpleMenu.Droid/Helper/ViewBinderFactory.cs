using MvvmCross.Platforms.Android.Binding.Binders;

namespace SimpleMenu.Droid.Helper
{
    public class ViewBinderFactory : IMvxAndroidViewBinderFactory
    {
        #region Public Methods
        public IMvxAndroidViewBinder Create(object source)
        {
            return new ViewBinder(source);
        }
        #endregion
    }
}