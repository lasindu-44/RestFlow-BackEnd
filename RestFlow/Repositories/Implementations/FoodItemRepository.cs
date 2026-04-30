using Microsoft.EntityFrameworkCore;
using RestFlow.Models;
using RestFlow.Repositories.Interfaces;

namespace RestFlow.Repositories.Implementations
{
    public class FoodItemRepository : IFoodItemRepository
    {
        private readonly AppDbContext _context;
        public FoodItemRepository(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<bool> CreateFoodItemAsync(FoodItemDto foodItemDto)
        {
            bool result = false;
            try
            {
                if (foodItemDto is not null && foodItemDto.Id > 0)
                {
                    //Update
                    var previousrecord = await _context.FoodItems.Where(f=>f.Id == foodItemDto.Id).FirstOrDefaultAsync();
                    if (previousrecord != null) 
                    {
                        previousrecord.restaurantId = foodItemDto.restaurantId;
                        previousrecord.categoryId = foodItemDto.categoryId;
                        previousrecord.name = foodItemDto.name;
                        previousrecord.description = foodItemDto.description;
                        previousrecord.price = foodItemDto.price;
                        previousrecord.prepTime = foodItemDto.prepTime;
                        previousrecord.available = foodItemDto.available;
                        previousrecord.imagePreview = foodItemDto.imagePreview;

                        await _context.SaveChangesAsync();
                        result = true;

                    }
                }
                else
                {
                    var foodItem = new FoodItemEntity
                    {
                        restaurantId = foodItemDto.restaurantId,
                        categoryId = foodItemDto.categoryId,
                        name = foodItemDto.name,
                        description = foodItemDto.description,
                        price = foodItemDto.price,
                        prepTime = foodItemDto.prepTime,
                        available = foodItemDto.available,
                        imagePreview = foodItemDto.imagePreview
                    };
                    await _context.FoodItems.AddAsync(foodItem);
                    await _context.SaveChangesAsync();
                    result = true;
                }
               

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating food item: {ex.Message}");
                return false;
            }

            return result;

        }

        public async Task<List<FoodItemDto>> GetFoodItemsByRestaurantandCategoryIdAsync(int restaurantId, int CategoryId)
        {
            var fooditems = await (from fi in _context.FoodItems
                                  join fc in _context.FoodCategories on fi.categoryId equals fc.categoryId
                                  join r in _context.Restaurants on fi.restaurantId equals r.Id
                                  where fi.restaurantId == restaurantId && fi.categoryId == CategoryId

                                  select new FoodItemDto
                                  {
                                      categoryId = fi.categoryId,
                                      categoryName = fc.categoryName,
                                      description = fi.description,
                                      available = fi.available,
                                      restaurantId = fi.Id,
                                      restaurantName = r.name,
                                      imagePreview = fi.imagePreview,
                                      name = fi.name,
                                      price = fi.price,
                                      prepTime = fi.prepTime,
                                      Id = fi.Id
                                     

                                  }).ToListAsync();
            return fooditems;
        }

        public async Task<bool> DeleteFoodItem(int id)
        {
            try
            {
                var foodItem = await _context.FoodItems.FindAsync(id);

                if (foodItem == null)
                    return false; // item not found

                _context.FoodItems.Remove(foodItem);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}