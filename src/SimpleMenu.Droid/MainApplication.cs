using Android.App;
using Android.Runtime;
using DialogMessaging;
using MvvmCross.Droid.Support.V7.AppCompat;
using System;

namespace SimpleMenu.Droid
{
    [Application]
    public class MainApplication : MvxAppCompatApplication<Setup, Core.App>
    {
        #region Lifecycle
        public override void OnCreate()
        {
            base.OnCreate();

            ConfigureDialogMessaging();
        }
        #endregion

        #region Constructors
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
        #endregion

        #region Private Methods
        private void ConfigureDialogMessaging()
        {
            MessagingService.Init(this);

#if DEBUG
            MessagingService.VerboseLogging = true;
#endif
        }
        #endregion
    }
}