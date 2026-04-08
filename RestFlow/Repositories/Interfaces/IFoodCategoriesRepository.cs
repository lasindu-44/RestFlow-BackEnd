using RestFlow.Models;

namespace RestFlow.Repositories.Interfaces
{
    public interface IFoodCategoriesRepository
    {
        Task<FoodCategoriesEntity> CreateFoodCategoryAsync(FoodCategoriesDto foodCategoryDto, string userId);

        Task<List<FoodCategoriesDto>> GetFoodCategoriesAsync(int RestaurantId);
        Task<bool>DeactivateFoodCategoriesAsync(int Id);

    }
}
