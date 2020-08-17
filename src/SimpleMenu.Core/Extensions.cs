using DialogMessaging;
using DialogMessaging.Interactions;
using SimpleMenu.Core.Helper;
using SimpleMenu.Core.Properties;
using SimpleMenu.Core.Schema;
using System;
using System.Threading.Tasks;

namespace SimpleMenu.Core
{
    public static class Extensions
    {
        #region Public Methods
        /// <summary>
        /// Gets a human readable title for this metric.
        /// </summary>
        public static string GetTitle(this Metric metric)
        {
            return metric switch
            {
                Metric.Celsius => Resources.HintCelsius,
                Metric.Cup => Resources.HintCup,
                Metric.Fahrenheit => Resources.HintFahrenheit,
                Metric.Gallon => Resources.HintGallon,
                Metric.Gram => Resources.HintGram,
                Metric.Kelvin => Resources.HintKelvin,
                Metric.Kilogram => Resources.HintKilogram,
                Metric.Liter => Resources.HintLiter,
                Metric.Millileter => Resources.HintMilliliter,
                Metric.Ounce => Resources.HintOunce,
                Metric.Pound => Resources.HintPound,
                Metric.Tablespoon => Resources.HintTablespoon,
                Metric.Teaspoon => Resources.HintTeaspoon,
                _ => throw new NotImplementedException("Title not implemented.")
            };
        }

        /// <summary>
        /// Checks whether this metric is compatible with another.
        /// </summary>
        /// <param name="toMetric">The metric to convert to.</param>
        public static bool IsCompatible(this Metric metric, Metric toMetric)
        {
            try
            {
                MetricHelper.Convert(metric, 1, toMetric);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Displays a loading spinner to the user asynchronously.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="task">The task to execute.</param>
        public static TTask ShowLoadingAsync<TTask>(this IMessagingService messagingService, string message, TTask task)
            where TTask : Task
        {
            return messagingService.ShowLoadingAsync(new LoadingAsyncConfig { Message = message }, task);
        }

        /// <summary>
        /// Displays a snackbar to the user.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public static void Snackbar(this IMessagingService messagingService, string message)
        {
            messagingService.Snackbar(new SnackbarConfig { Message = message });
        }

        /// <summary>
        /// Displays a toast to the user.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public static void Toast(this IMessagingService messagingService, string message)
        {
            messagingService.Toast(new ToastConfig { Message = message });
        }
        #endregion
    }
}
