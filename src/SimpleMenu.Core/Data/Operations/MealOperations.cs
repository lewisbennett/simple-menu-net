using SimpleMenu.Core.Data.Entities;
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
                UUID = Guid.NewGuid()
            };

            await FileServiceWrapper.Instance.SaveImageAsync(FileServiceWrapper.ImagesDirectory, meal.ImageUUID.ToString(), image).ConfigureAwait(false);

            await SaveMealAsync(meal).ConfigureAwait(false);

            FileServiceWrapper.Instance.AddEntity(meal);

            return meal;
        }

        /// <summary>
        /// Deletes a meal.
        /// </summary>
        /// <param name="uuid">The UUID of the meal to delete.</param>
        public async Task DeleteMealAsync(Guid uuid)
        {
            var fileServiceWrapper = FileServiceWrapper.Instance;

            var fileName = $"{uuid}.json";

            var doesExist = await fileServiceWrapper.DoesExistAsync(FileServiceWrapper.MealsDirectory, fileName).ConfigureAwait(false);

            if (doesExist)
            {
                await fileServiceWrapper.DeleteFileAsync(FileServiceWrapper.MealsDirectory, fileName).ConfigureAwait(false);

                var coreServiceWrapper = CoreServiceWrapper.Instance;

                var localMeal = coreServiceWrapper.ActiveUser.Meals.FirstOrDefault(m => m.UUID == uuid);

                if (localMeal != null)
                {
                    coreServiceWrapper.ActiveUser.Meals.Remove(localMeal);

                    fileServiceWrapper.RemoveEntity(localMeal);
                }
            }
        }

        /// <summary>
        /// List all of the user's meals.
        /// </summary>
        public async Task<MealEntity[]> ListAllMealsAsync()
        {
            var coreServiceWrapper = CoreServiceWrapper.Instance;
            var fileServiceWrapper = FileServiceWrapper.Instance;

            var savedMealFiles = await fileServiceWrapper.ListFilesAsync(FileServiceWrapper.MealsDirectory).ConfigureAwait(false);

            var meals = new List<MealEntity>(coreServiceWrapper.ActiveUser.Meals);

            foreach (var savedMealFile in savedMealFiles)
            {
                var meal = await fileServiceWrapper.ReadJsonAsync<MealEntity>(FileServiceWrapper.MealsDirectory, savedMealFile).ConfigureAwait(false);

                meals.Add(meal);
            }

            var finalMeals = meals.Distinct(this).ToArray();

            coreServiceWrapper.ActiveUser.Meals.Clear();

            foreach (var finalMeal in finalMeals)
            {
                coreServiceWrapper.ActiveUser.Meals.Add(finalMeal);

                await SaveMealAsync(finalMeal).ConfigureAwait(false);

                fileServiceWrapper.AddEntity(finalMeal);
            }

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
