using Microsoft.EntityFrameworkCore;
using RestFlow.Models;
using RestFlow.Models.Dtos;

namespace RestFlow.Repositories.Interfaces
{
    public interface IRestaurantRepository
    {
        Task<RestaurantEntity> CreateRestaurantAsync(CreateRestaurantDto restaurant, string UserId);
        Task<List<CuisineTypeDto>> GetCuisineTypesAsync();
        Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync(string UserId);
        Task<CreateRestaurantDto> UpdateRestaurantAsync(int id, CreateRestaurantDto restaurant, string userId);
        Task<bool> DeleteRestaurantAsync(int id, string UserId);
        Task<IEnumerable<RestaurantDto>> GetAllActiveRestaurantsAsync();
        Task<List<MenuCategory>> GetRestaurantMenuAsync(int restaurantId);

    }
}