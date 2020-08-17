using SimpleMenu.Core.Properties;
using System;

namespace SimpleMenu.Core.Data.Operations
{
    public class PersonalizationOperations
    {
        #region Public Methods
        /// <summary>
        /// Gets a set of compliments that can be used on images.
        /// </summary>
        public string[] GetImageCompliments()
        {
            return new[]
            {
                Resources.MessageLooksGood,
                Resources.MessageDelicious,
                Resources.MessageGreatShot
            };
        }

        /// <summary>
        /// Gets a random compliment for an image.
        /// </summary>
        /// <param name="personalized">Whether to personalize the compliment with the user's name, if available.</param>
        public string GetRandomImageCompliment(bool personalized)
        {
            var compliments = GetImageCompliments();

            return compliments[new Random().Next(0, compliments.Length)];
        }
        #endregion

        #region Static Properties
        public static PersonalizationOperations Instance { get; } = new PersonalizationOperations();
        #endregion
    }
}
