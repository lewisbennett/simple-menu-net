using System;

namespace SimpleMenu.Core.Schema
{
    [Flags]
    public enum Metric
    {
        // Temperature
        Celsius = 1,
        Fahrenheit = 1 << 1,
        Kelvin = 1 << 2,

        // Volume
        Cup = 1 << 3,
        Gallon = 1 << 4,
        Liter = 1 << 5,
        Millileter = 1 << 6,
        Tablespoon = 1 << 7,
        Teaspoon = 1 << 8,

        // Weight
        Gram = 1 << 9,
        Kilogram = 1 << 10,
        Ounce = 1 << 11,
        Pound = 1 << 12
    }
}
