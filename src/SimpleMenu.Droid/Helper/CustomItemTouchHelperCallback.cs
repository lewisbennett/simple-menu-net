using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Java.Util;
using MvvmCross.Droid.Support.V7.RecyclerView;
using System.Linq;

namespace SimpleMenu.Droid.Helper
{
    public class CustomItemTouchHelperCallback : ItemTouchHelper.Callback
    {
        #region Event Handlers
        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            if (recyclerView is MvxRecyclerView mvxRecyclerView)
            {
                Collections.Swap(mvxRecyclerView.ItemsSource.Cast<object>().ToList(), viewHolder.AdapterPosition, target.AdapterPosition);

                mvxRecyclerView.GetAdapter().NotifyItemMoved(viewHolder.AdapterPosition, target.AdapterPosition);
            }
            
            return true;
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
        }
        #endregion

        #region Public Methods
        public override int GetMovementFlags(RecyclerView p0, RecyclerView.ViewHolder p1)
        {
            return MakeFlag(ItemTouchHelper.ActionStateDrag, ItemTouchHelper.Down | ItemTouchHelper.Up | ItemTouchHelper.Start | ItemTouchHelper.End);
        }
        #endregion
    }
}