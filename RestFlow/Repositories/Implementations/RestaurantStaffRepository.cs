using RestFlow.Models;
using RestFlow.Models.Dtos;
using RestFlow.Repositories.Interfaces;

namespace RestFlow.Repositories.Implementations
{
    public class RestaurantStaffRepository: IRestaurantStaffRepository
    {
        private readonly AppDbContext _context;

        public RestaurantStaffRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool>AsignRestaurantStaff(int RestaurantId, string UserId)
        {
            try
            {
                var newRestaurant = new RestaurantStaff
                {
                  UserId = UserId,
                  RestautrantId = RestaurantId,
                  AsignAt = DateTime.Now,
                };
                _context.RestaurantStaff.Add(newRestaurant);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) { return false; }

        }
    }
}
