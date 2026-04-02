using RestFlow.Models;
using RestFlow.Models.Dtos;

namespace RestFlow.Repositories.Interfaces
{
    public interface IRestaurantRepository
    {
            Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync(string UserId);
            //Task<RestaurantEntity> GetRestaurantByIdAsync(int id);
            Task<RestaurantEntity> CreateRestaurantAsync(CreateRestaurantDto restaurant,string UserId);
            Task<List<CuisineTypeDto>> GetCuisineTypesAsync();
            Task<CreateRestaurantDto> UpdateRestaurantAsync(int id, CreateRestaurantDto restaurant,string UserId);
            Task<bool> DeleteRestaurantAsync(int id,string UserId);
    }
}
