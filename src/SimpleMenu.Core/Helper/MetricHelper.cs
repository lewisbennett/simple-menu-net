using SimpleMenu.Core.Schema;
using System;

namespace SimpleMenu.Core.Helper
{
    public static class MetricHelper
    {
        #region Public Methods
        /// <summary>
        /// Converts one metric to another, where supported.
        /// </summary>
        /// <param name="fromMeasurable">The metric to convert from.</param>
        /// <param name="fromValue">The unit being converted.</param>
        /// <param name="toMeasurable">The metric to convert to.</param>
        public static double Convert(Metric fromMeasurable, double fromValue, Metric toMeasurable)
        {
            if (fromMeasurable == toMeasurable)
                return fromValue;

            return (fromMeasurable, toMeasurable) switch
            {
                // Temperature.
                (Metric.Fahrenheit, Metric.Celsius) => (fromValue - 32) * 5 / 9,
                (Metric.Fahrenheit, Metric.Kelvin) => (fromValue - 32) * 5 / 9 + 273.15,
                (Metric.Celsius, Metric.Fahrenheit) => (fromValue * 9 / 5) +  32,
                (Metric.Celsius, Metric.Kelvin) => fromValue + 273.15,
                (Metric.Kelvin, Metric.Fahrenheit) => (fromValue - 273.15) * 9 / 5 + 32,
                (Metric.Kelvin, Metric.Celsius) => fromValue - 273.15,

                // Volume.
                (Metric.Cup, Metric.Gallon) => fromValue / 19.215,
                (Metric.Cup, Metric.Liter) => fromValue / 4.22675,
                (Metric.Cup, Metric.Millileter) => fromValue * 236.558,
                (Metric.Cup, Metric.Tablespoon) => fromValue * 16,
                (Metric.Cup, Metric.Teaspoon) => fromValue * 48,

                (Metric.Gallon, Metric.Cup) => fromValue * 19.215,
                (Metric.Gallon, Metric.Liter) => fromValue * 4.54609,
                (Metric.Gallon, Metric.Millileter) => fromValue * 4546.09,
                (Metric.Gallon, Metric.Tablespoon) => fromValue * 307.443,
                (Metric.Gallon, Metric.Teaspoon) => fromValue * 922.33,

                (Metric.Liter, Metric.Cup) => fromValue * 4.22675,
                (Metric.Liter, Metric.Gallon) => fromValue / 4.54609,
                (Metric.Liter, Metric.Millileter) => fromValue * 1000,
                (Metric.Liter, Metric.Tablespoon) => fromValue * 67.628,
                (Metric.Liter, Metric.Teaspoon) => fromValue * 202.884,

                (Metric.Millileter, Metric.Cup) => fromValue / 236.558,
                (Metric.Millileter, Metric.Gallon) => fromValue / 4546.09,
                (Metric.Millileter, Metric.Liter) => fromValue / 1000,
                (Metric.Millileter, Metric.Tablespoon) => fromValue / 14.7868,
                (Metric.Millileter, Metric.Teaspoon) => fromValue / 4.92892,

                (Metric.Tablespoon, Metric.Cup) => fromValue / 16,
                (Metric.Tablespoon, Metric.Gallon) => fromValue / 307.443,
                (Metric.Tablespoon, Metric.Liter) => fromValue / 67.628,
                (Metric.Tablespoon, Metric.Millileter) => fromValue * 14.7868,
                (Metric.Tablespoon, Metric.Teaspoon) => fromValue * 3,

                (Metric.Teaspoon, Metric.Cup) => fromValue / 48,
                (Metric.Teaspoon, Metric.Gallon) => fromValue / 922.33,
                (Metric.Teaspoon, Metric.Liter) => fromValue / 202.884,
                (Metric.Teaspoon, Metric.Millileter) => fromValue * 4.92892,
                (Metric.Teaspoon, Metric.Tablespoon) => fromValue / 3,

                // Weight.
                (Metric.Gram, Metric.Kilogram) => fromValue / 1000,
                (Metric.Gram, Metric.Ounce) => fromValue / 28.3495,
                (Metric.Gram, Metric.Pound) => fromValue / 453.592,

                (Metric.Kilogram, Metric.Gram) => fromValue * 1000,
                (Metric.Kilogram, Metric.Ounce) => fromValue * 35.274,
                (Metric.Kilogram, Metric.Pound) => fromValue * 2.20462,

                (Metric.Ounce, Metric.Gram) => fromValue * 28.3495,
                (Metric.Ounce, Metric.Kilogram) => fromValue / 35.274,
                (Metric.Ounce, Metric.Pound) => fromValue / 16,

                (Metric.Pound, Metric.Gram) => fromValue * 453.592,
                (Metric.Pound, Metric.Kilogram) => fromValue / 2.20462,
                (Metric.Pound, Metric.Ounce) => fromValue * 16,

                _ => throw new NotImplementedException("Formula not implemented.")
            };
        }
        #endregion
    }
}
