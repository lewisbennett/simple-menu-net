using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;
using SimpleMenu.Core.Models;
using SimpleMenu.Core.Models.Base;
using System;

namespace SimpleMenu.Droid.TemplateSelectors
{
    public class BaseModelItemTemplateSelector : MvxTemplateSelector<BaseModel>
    {
        #region Public Methods
        protected override int SelectItemViewType(BaseModel forItemObject)
        {
            return forItemObject switch
            {
                ImageModel _ => Resource.Layout.item_web_image,
                MealModel _ => Resource.Layout.item_meal,
                MenuMealModel _ => Resource.Layout.item_menu_meal,
                MenuModel _ => Resource.Layout.item_menu,
                TextIconModel _ => Resource.Layout.item_text_icon,
                TextModel _ => Resource.Layout.item_text,
                _ => throw new ArgumentException($"Layout not found for type: {forItemObject.GetType().Name}")
            };
        }

        public override int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }
        #endregion
    }
}