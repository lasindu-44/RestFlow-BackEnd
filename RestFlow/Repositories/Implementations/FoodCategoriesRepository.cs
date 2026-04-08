using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using RestFlow.Models;
using RestFlow.Repositories.Interfaces;

namespace RestFlow.Repositories.Implementations
{
    public class FoodCategoriesRepository : IFoodCategoriesRepository
    {
        private readonly AppDbContext _context;
        public FoodCategoriesRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<FoodCategoriesEntity> CreateFoodCategoryAsync(FoodCategoriesDto foodCategoryDto, string userId)
        {
            if (foodCategoryDto != null && foodCategoryDto.categoryId == 0)
            {
                FoodCategoriesEntity vm = new FoodCategoriesEntity();

                vm.categoryName = foodCategoryDto.categoryName;
                vm.description = foodCategoryDto.description;
                vm.displayOrder = foodCategoryDto.displayOrder;
                vm.restaurantId = foodCategoryDto.restaurantId;
                vm.CreatedBy = userId;
                vm.CreatedAt = DateTime.Now;
                vm.isActive = true;


                await _context.FoodCategories.AddAsync(vm);
                await _context.SaveChangesAsync();
                return vm;

            }
            else
            {
                var ExistingRecord = await _context.FoodCategories.Where(f => f.categoryId == foodCategoryDto.categoryId).FirstOrDefaultAsync();

                ExistingRecord.categoryName = foodCategoryDto.categoryName;
                ExistingRecord.description = foodCategoryDto.description;
                ExistingRecord.displayOrder = foodCategoryDto.displayOrder;
                ExistingRecord.restaurantId = foodCategoryDto.restaurantId;
                await _context.SaveChangesAsync();
                return ExistingRecord;
            }




        }

        public async Task<List<FoodCategoriesDto>> GetFoodCategoriesAsync(int RestaurantId)
        {
            var result = await (from fc in _context.FoodCategories
                                join r in _context.Restaurants on fc.restaurantId equals r.Id
                                where fc.restaurantId == RestaurantId
                                select new FoodCategoriesDto
                                {
                                    categoryId = fc.categoryId,
                                    categoryName = fc.categoryName,
                                    description = fc.description,
                                    displayOrder = fc.displayOrder,
                                    isActive = fc.isActive,
                                    restaurantId = r.Id,
                                    resturantName = r.name
                                }).OrderBy(d => d.displayOrder).ToListAsync();
            return result;
        }

        public async Task<bool> DeactivateFoodCategoriesAsync(int Id)
        {
            var ExistingRecord = await _context.FoodCategories.Where(f => f.categoryId == Id).FirstOrDefaultAsync();
            if (ExistingRecord != null)
            {
                ExistingRecord.isActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}