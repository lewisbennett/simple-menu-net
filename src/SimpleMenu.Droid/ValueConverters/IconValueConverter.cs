﻿using MvvmCross.Converters;
using SimpleMenu.Core.Schema;
using System;
using System.Globalization;

namespace SimpleMenu.Droid.ValueConverters
{
    public class IconValueConverter : MvxValueConverter<Icon, int>
    {
        #region Public Methods
        protected override int Convert(Icon value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                Icon.AccountCircle => Resource.Drawable.ic_account_circle,
                Icon.AddCircle => Resource.Drawable.ic_add_circle,
                Icon.Brightness2 => Resource.Drawable.ic_brightness_2,
                Icon.Brightness3 => Resource.Drawable.ic_brightness_3,
                Icon.Brightness4 => Resource.Drawable.ic_brightness_4,
                Icon.Brightness5 => Resource.Drawable.ic_brightness_5,
                Icon.Brightness6 => Resource.Drawable.ic_brightness_6,
                Icon.Brightness7 => Resource.Drawable.ic_brightness_7,
                Icon.DeviceInformation => Resource.Drawable.ic_device_information,
                Icon.DragIndicator => Resource.Drawable.ic_drag_indicator,
                Icon.Help => Resource.Drawable.ic_help,
                Icon.Home => Resource.Drawable.ic_home,
                Icon.InsertPhoto => Resource.Drawable.ic_insert_photo,
                Icon.Kitchen => Resource.Drawable.ic_kitchen,
                Icon.MenuBook => Resource.Drawable.ic_menu_book,
                Icon.Restaurant => Resource.Drawable.ic_restaurant,
                Icon.Settings => Resource.Drawable.ic_settings,
                Icon.Sync => Resource.Drawable.ic_sync,
                Icon.Timelapse => Resource.Drawable.ic_timelapse,
                _ => 0
            };
        }
        #endregion
    }
}