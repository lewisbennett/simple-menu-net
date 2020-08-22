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

                var localMeal = fileServiceWrapper.Entities.FirstOrDefault(e => e is MealEntity m && m.UUID == uuid);

                if (localMeal != null)
                    fileServiceWrapper.RemoveEntity(localMeal);
            }
        }

        /// <summary>
        /// Gets a meal.
        /// </summary>
        /// <param name="mealUuid">The UUID of the meal.</param>
        public async ValueTask<MealEntity> GetMealAsync(Guid mealUuid)
        {
            var fileServiceWrapper = FileServiceWrapper.Instance;

            if (fileServiceWrapper.Entities.FirstOrDefault(e => e is MealEntity m && m.UUID == mealUuid) is MealEntity meal)
                return meal;

            meal = await FileServiceWrapper.Instance.ReadJsonAsync<MealEntity>(FileServiceWrapper.MealsDirectory, mealUuid.ToString()).ConfigureAwait(false);

            FileServiceWrapper.Instance.AddEntity(meal);

            return meal;
        }

        /// <summary>
        /// List all of the user's meals.
        /// </summary>
        public async Task<MealEntity[]> ListAllMealsAsync()
        {
            var savedMeals = await FileServiceWrapper.Instance.ListFilesAsync(FileServiceWrapper.MealsDirectory).ConfigureAwait(false);

            var meals = new List<MealEntity>();

            foreach (var savedMeal in savedMeals)
            {
                Guid mealUuid;

                if (savedMeal.EndsWith(".json"))
                {
                    // Remove ".json".
                    var mutableSavedMeal = savedMeal.Substring(0, savedMeal.Length - 5);

                    // Remove the path.
                    mutableSavedMeal = mutableSavedMeal.Substring(mutableSavedMeal.Length - 36);

                    mealUuid = Guid.Parse(mutableSavedMeal);
                }
                else
                    mealUuid = Guid.Parse(savedMeal);

                var meal = await GetMealAsync(mealUuid).ConfigureAwait(false);

                meals.Add(meal);
            }

            return meals.ToArray();
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
