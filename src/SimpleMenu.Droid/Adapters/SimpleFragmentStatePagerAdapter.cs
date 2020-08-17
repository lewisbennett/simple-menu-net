using Android.Support.V4.App;

namespace SimpleMenu.Droid.Adapters
{
    public class SimpleFragmentStatePagerAdapter : FragmentStatePagerAdapter
    {
        #region Fields
        private readonly Fragment[] _fragments;
        #endregion

        #region Properties
        public override int Count => _fragments.Length;
        #endregion

        #region Public Methods
        public override Fragment GetItem(int position)
        {
            return _fragments[position];
        }
        #endregion

        #region Constructors
        public SimpleFragmentStatePagerAdapter(FragmentManager fm, params Fragment[] fragments)
            : base(fm)
        {
            _fragments = fragments;
        }
        #endregion
    }
}