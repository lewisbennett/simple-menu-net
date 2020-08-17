using Android.App;

namespace SimpleMenu.Droid.Helper
{
    public static class DimensionHelper
    {
        #region Constant Values
        public const int ScreenSizeTypical = 320;
        public const int ScreenSizeLargePhone = 480;
        public const int ScreenSizeSmallTablet = 600;
        public const int ScreenSizeTypicalTablet = 720;
        public const int ScreenSizeSmallTabletLandscape = 960;
        public const int ScreenSizeTypicalTabletLandscape = 1080;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the number of items that should be shown horizontally in grid view.
        /// </summary>
        public static int GridViewHorizontalCount
        {
            get
            {
                var widthDp = PxToDp(Application.Context.Resources.DisplayMetrics.WidthPixels);

                if (widthDp <= ScreenSizeTypical)
                    return 2;

                if (widthDp <= ScreenSizeLargePhone)
                    return 3;

                if (widthDp <= ScreenSizeSmallTablet)
                    return 3;

                if (widthDp <= ScreenSizeTypicalTablet)
                    return 4;

                if (widthDp <= ScreenSizeSmallTabletLandscape)
                    return 5;

                if (widthDp <= ScreenSizeTypicalTabletLandscape)
                    return 6;

                return 8;
            }
        }

        /// <summary>
        /// Gets the number of items that should be shown horizontally in list view.
        /// </summary>
        public static int ListViewHorizontalCount
        {
            get
            {
                var widthDp = PxToDp(Application.Context.Resources.DisplayMetrics.WidthPixels);

                if (widthDp > ScreenSizeSmallTabletLandscape)
                    return 3;

                if (widthDp > ScreenSizeLargePhone)
                    return 2;

                return 1;
            }
        }

        /// <summary>
        /// Gets the number of items that should be shown horizontally in staggered view.
        /// </summary>
        public static int StaggeredViewHorizontalCount
        {
            get
            {
                var widthDp = PxToDp(Application.Context.Resources.DisplayMetrics.WidthPixels);

                if (widthDp <= ScreenSizeTypical)
                    return 1;

                if (widthDp <= ScreenSizeLargePhone)
                    return 2;

                if (widthDp <= ScreenSizeSmallTablet)
                    return 3;

                if (widthDp <= ScreenSizeSmallTabletLandscape)
                    return 4;

                return 5;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Converts a pixel value to DP.
        /// </summary>
        /// <param name="pixelValue">The pixel value to be converted</param>
        public static float PxToDp(float pixelValue)
        {
            return pixelValue / Application.Context.Resources.DisplayMetrics.Density;
        }

        /// <summary>
        /// Converts a DP value to pixels.
        /// </summary>
        /// <param name="dpValue">The DP value to be converted</param>
        public static float DpToPx(float dpValue)
        {
            return dpValue * Application.Context.Resources.DisplayMetrics.Density;
        }
        #endregion
    }
}