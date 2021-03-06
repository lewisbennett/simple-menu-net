﻿using Android.App;
using Android.OS;
using Android.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using SimpleMenu.Core.ViewModels.CreateThing;
using SimpleMenu.Droid.Activities.CreateThing.Base;
using SimpleMenu.Droid.Attributes;
using SimpleMenu.Droid.Fragments;
using V4_Fragment = Android.Support.V4.App.Fragment;

namespace SimpleMenu.Droid.Activities.CreateThing
{
    [MvxActivityPresentation]
    [Activity(Label = "", WindowSoftInputMode = SoftInput.AdjustPan)]
    [ActivityLayout(EnableBackButton = true, LayoutResourceID = Resource.Layout.activity_create_thing)]
    public class CreateIngredientActivity : CreateThingBaseActivity<CreateIngredientViewModel>
    {
        #region Protected Methods
        protected override V4_Fragment[] CreateFragments()
        {
            return new V4_Fragment[]
            {
                new EnterNameFragment
                {
                    CachedDrawableIDEnd = Resource.Drawable.ic_restaurant,
                    ViewModel = ViewModel.EnterNameViewModel
                }
            };
        }
        #endregion

        #region Lifecycle
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            TabLayout.DisableTabClicks();
        }
        #endregion
    }
}