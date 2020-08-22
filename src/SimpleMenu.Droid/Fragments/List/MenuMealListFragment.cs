using Android.OS;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using SimpleMenu.Core.Models;
using SimpleMenu.Core.ViewModels.List;
using SimpleMenu.Droid.Attributes;
using SimpleMenu.Droid.Fragments.List.Base;
using SimpleMenu.Droid.Helper;

namespace SimpleMenu.Droid.Fragments.List
{
    [FragmentLayout(LayoutResourceID = Resource.Layout.frag_list, MenuResourceID = Resource.Menu.menu_menu_meal_list)]
    public class MenuMealListFragment : ListBaseFragment
    {
        #region Fields
        private CustomItemTouchHelperCallback<MenuMealModel> _customItemTouchHelperCallback;
        private ItemTouchHelper _itemTouchHelper;
        #endregion

        #region Event Handlers
        private void DragIndicatorImageView_Touch(object sender, View.TouchEventArgs e)
        {
            if (sender is View view)
                _itemTouchHelper.StartDrag(RecyclerView.FindContainingViewHolder(view));
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (ViewModel is MenuMealListViewModel menuMealListViewModel)
            {
                switch (item.ItemId)
                {
                    case Resource.Id.menu_regenerate_menu:
                        menuMealListViewModel.RegenerateMenuButtonClickCommand.Execute();
                        return true;

                    default:
                        break;
                }
            }

            return base.OnOptionsItemSelected(item);
        }

        private void RecyclerView_ChildViewAdded(object sender, ViewGroup.ChildViewAddedEventArgs e)
        {
            if (e.Child != null)
            {
                if (e.Child.FindViewById<ImageView>(Resource.Id.drag_indicator) is ImageView dragIndicatorImageView)
                    dragIndicatorImageView.Touch += DragIndicatorImageView_Touch;
            }
        }

        private void RecyclerView_ChildViewRemoved(object sender, ViewGroup.ChildViewRemovedEventArgs e)
        {
            if (e.Child != null)
            {
                if (e.Child.FindViewById<ImageView>(Resource.Id.drag_indicator) is ImageView dragIndicatorImageView)
                    dragIndicatorImageView.Touch -= DragIndicatorImageView_Touch;
            }
        }
        #endregion

        #region Protected Methods
        protected override RecyclerView.LayoutManager CreateLayoutManager()
            => new GridLayoutManager(Context, DimensionHelper.ListViewHorizontalCount);
        #endregion

        #region Lifecycle
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _customItemTouchHelperCallback = new CustomItemTouchHelperCallback<MenuMealModel>();
            _itemTouchHelper = new ItemTouchHelper(_customItemTouchHelperCallback);

            _itemTouchHelper.AttachToRecyclerView(RecyclerView);
        }

        public override void OnResume()
        {
            base.OnResume();

            RecyclerView.ChildViewAdded += RecyclerView_ChildViewAdded;
            RecyclerView.ChildViewRemoved += RecyclerView_ChildViewRemoved;
        }

        public override void OnPause()
        {
            base.OnPause();

            RecyclerView.ChildViewAdded -= RecyclerView_ChildViewAdded;
            RecyclerView.ChildViewRemoved -= RecyclerView_ChildViewRemoved;
        }
        #endregion
    }
}