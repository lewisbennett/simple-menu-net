using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Java.Util;
using MvvmCross.Droid.Support.V7.RecyclerView;
using SimpleMenu.Core.Models.Base;
using System.Collections.ObjectModel;
using System.Linq;

namespace SimpleMenu.Droid.Helper
{
    public class CustomItemTouchHelperCallback<TModel> : ItemTouchHelper.Callback
        where TModel : BaseModel
    {
        #region Event Handlers
        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            if (recyclerView is MvxRecyclerView mvxRecyclerView)
            {
                if (mvxRecyclerView.ItemsSource is ObservableCollection<TModel> observableCollection)
                    observableCollection.Move(viewHolder.AdapterPosition, target.AdapterPosition);
                else
                {
                    Collections.Swap(mvxRecyclerView.ItemsSource.Cast<object>().ToList(), viewHolder.AdapterPosition, target.AdapterPosition);

                    mvxRecyclerView.GetAdapter().NotifyItemMoved(viewHolder.AdapterPosition, target.AdapterPosition);
                }
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