﻿using SimpleMenu.Core.Data.Entities;
using SimpleMenu.Core.Services.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMenu.Core.Data.Operations
{
    public class MealOperations : IEqualityComparer<MealEntity>
    {
        #region Public Methods
        /// <summary>
        /// Creates a new meal.
        /// </summary>
        /// <param name="name">The name of the meal.</param>
        /// <param name="image">The image for the meal.</param>
        public async Task<MealEntity> CreateMealAsync(string name, byte[] image)
        {
            var meal = new MealEntity
            {
                ImageUUID = Guid.NewGuid(),
                Name = name,
                PreparationTime = TimeSpan.FromMinutes(83),
                UUID = Guid.NewGuid()
            };

            await FileServiceWrapper.Instance.SaveImageAsync(FileServiceWrapper.ImagesDirectory, meal.ImageUUID.ToString(), image).ConfigureAwait(false);

            await SaveMealAsync(meal).ConfigureAwait(false);

            return meal;
        }

        /// <summary>
        /// List all of the user's meals.
        /// </summary>
        /// <param name="save">Whether to save the meals.</param>
        public async Task<MealEntity[]> ListAllMealsAsync()
        {
            var fileServiceWrapper = FileServiceWrapper.Instance;

            var savedMealFiles = await fileServiceWrapper.ListFilesAsync(FileServiceWrapper.MealsDirectory).ConfigureAwait(false);

            var meals = new List<MealEntity>(CoreServiceWrapper.Instance.ActiveUser.Meals);

            foreach (var savedMealFile in savedMealFiles)
            {
                var meal = await fileServiceWrapper.ReadJsonAsync<MealEntity>(FileServiceWrapper.MealsDirectory, savedMealFile).ConfigureAwait(false);

                meals.Add(meal);
            }

            var finalMeals = meals.Distinct(this).ToArray();

            foreach (var finalMeal in finalMeals)
                await SaveMealAsync(finalMeal).ConfigureAwait(false);

            return finalMeals;
        }

        /// <summary>
        /// Saves a meal.
        /// </summary>
        /// <param name="meal">The meal to save.</param>
        public Task SaveMealAsync(MealEntity meal)
            => FileServiceWrapper.Instance.SaveJsonAsync(FileServiceWrapper.MealsDirectory, meal.UUID.ToString(), meal);
        #endregion

        #region Equality Methods
        bool IEqualityComparer<MealEntity>.Equals(MealEntity x, MealEntity y)
            => x.UUID == y.UUID;

        int IEqualityComparer<MealEntity>.GetHashCode(MealEntity obj)
            => obj.GetHashCode();
        #endregion

        #region Static Properties
        public static MealOperations Instance { get; } = new MealOperations();
        #endregion
    }
}
