using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace SimpleMenu.Droid.TargetBindings
{
    public class ImageViewResourceIdTargetBinding : MvxTargetBinding<ImageView, int>
    {
        #region Properties
        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;
        #endregion

        #region Protected Methods
        protected override void SetValue(int value)
        {
            Target.SetImageResource(value);
        }
        #endregion

        #region Constructors
        public ImageViewResourceIdTargetBinding(ImageView imageView)
            : base(imageView)
        {
        }
        #endregion
    }
}