using Microsoft.EntityFrameworkCore;
using RestFlow.Models;
using RestFlow.Models.Dtos;
using RestFlow.Repositories.Interfaces;

namespace RestFlow.Repositories.Implementations
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly AppDbContext _context;

        public RestaurantRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RestaurantEntity> CreateRestaurantAsync(CreateRestaurantDto restaurant, string UserId)
        {
            try
            {
                var newRestaurant = new RestaurantEntity
                {
                    name = restaurant.name,
                    cuisine = restaurant.cuisine,
                    email = restaurant.email,
                    phone = restaurant.phone,
                    address = restaurant.address,
                    imageUrl = restaurant.imageUrl,
                    openTime = restaurant.openTime,
                    closeTime = restaurant.closeTime,
                    CreatedBy = UserId
                };
                _context.Restaurants.Add(newRestaurant);
                await _context.SaveChangesAsync();
                return newRestaurant;
            }
            catch (Exception ex) { return null; }

        }
        public Task<List<CuisineTypeDto>> GetCuisineTypesAsync()
        {
            var data = Enum.GetValues(typeof(cuisineType))
                .Cast<cuisineType>()
                .Select(e => new CuisineTypeDto
                {
                    Id = (int)e,
                    Name = e.ToString()
                })
                .ToList();

            return Task.FromResult(data);
        }

        public async Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync(string UserId)
        {
            try
            {
                return await _context.Restaurants
           .Where(r => r.CreatedBy == UserId)
           .Select(r => new RestaurantDto
           {
               Id = r.Id,
               name = r.name,
               CuisineName = r.cuisine.ToString(), // convert enum to string
               email = r.email,
               phone = r.phone,
               address = r.address,
               imageUrl = r.imageUrl,
               openTime = r.openTime,
               closeTime = r.closeTime,
               IsActive = r.IsActive,
               cuisine = r.cuisine
           })
           .ToListAsync();

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<CreateRestaurantDto> UpdateRestaurantAsync(int id, CreateRestaurantDto restaurant,string userId)
        {
            try
            {
                if(id !=0)
                {
                    var Restaurant = await _context.Restaurants.Where(r => r.Id == id).FirstOrDefaultAsync();

                    if(Restaurant is not null)
                    {
                        Restaurant.name = restaurant.name;
                        Restaurant.phone = restaurant.phone;
                        Restaurant.email = restaurant.email;
                        Restaurant.address = restaurant.address;
                        Restaurant.cuisine = restaurant.cuisine;
                        Restaurant.closeTime = restaurant.closeTime;
                        Restaurant.openTime = restaurant.openTime;
                        Restaurant.imageUrl = restaurant.imageUrl;
                        Restaurant.LastUpdatedAt = DateTime.Now;
                        Restaurant.LastUpdatedBy = userId;

                        await _context.SaveChangesAsync();

                    }

                }
                return restaurant;


            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> DeleteRestaurantAsync(int id, string UserId)
        {
            try
            {
                var restdata = await _context.Restaurants.Where(d => d.Id == id).FirstOrDefaultAsync();

                if(restdata is not null)
                {
                    restdata.IsActive = false;
                    restdata.LastUpdatedAt = DateTime.Now;
                    restdata.LastUpdatedBy = UserId;

                    await _context.SaveChangesAsync();
                }

                return true;

            }catch(Exception e)
            {
                return false;

            }
        }
    }
}
