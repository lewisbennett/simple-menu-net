using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using SimpleMenu.Droid.Attributes;
using SimpleMenu.Droid.Fragments.Base;

namespace SimpleMenu.Droid.Fragments
{
    [FragmentLayout(LayoutResourceID = Resource.Layout.frag_enter_name)]
    public class EnterNameFragment : BaseFragment
    {
        #region Properties
        /// <summary>
        /// Gets or sets a drawable resource ID to be applied to the text input edit text once loaded.
        /// </summary>
        public int CachedDrawableIDEnd { get; set; }

        /// <summary>
        /// Gets this fragment's text input edit text.
        /// </summary>
        public TextInputEditText TextInputEditText { get; private set; }
        #endregion

        #region Lifecycle
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            TextInputEditText = view.FindViewById<TextInputEditText>(Resource.Id.textinputedittext);

            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            TextInputEditText.SetCompoundDrawablesRelativeWithIntrinsicBounds(0, 0, CachedDrawableIDEnd, 0);
        }
        #endregion
    }
}