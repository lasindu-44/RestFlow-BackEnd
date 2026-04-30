using RestFlow.Models;

namespace RestFlow.Repositories.Interfaces
{
    public interface IFoodItemRepository
    {
        Task<bool>CreateFoodItemAsync(FoodItemDto foodItemDto);
        Task<List<FoodItemDto>> GetFoodItemsByRestaurantandCategoryIdAsync(int restaurantId,int CategoryId);
        Task<bool> DeleteFoodItem(int Id);

    }
}
