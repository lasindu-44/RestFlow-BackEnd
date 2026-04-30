using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestFlow.Repositories.Interfaces;

namespace RestFlow.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRestaurantStaffRepository restaurantStaffRepository;
        public UserController(UserManager<ApplicationUser> userManager,IRestaurantStaffRepository restaurantStaffRepository, AppDbContext db)
        {
            _userManager = userManager;
            this.restaurantStaffRepository = restaurantStaffRepository;
            _db = db;
        }

        [Authorize(Roles = "SystemAdmin")]
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var staffUserIds = _db.RestaurantStaff
    .Select(rs => rs.UserId);

            var users = await _userManager.Users
                .Where(u => !staffUserIds.Contains(u.Id)&& u.IsRestaurantOwner == false)
                .Select(u => new {
                    u.Id,
                    u.UserName,
                    u.Email
                })
                .ToListAsync();

            return Ok(users);
        }

        [Authorize(Roles = "SystemAdmin")]
        [HttpPost("AsigntotheStaff")]
        public async Task<IActionResult> AsignStaff(string UserId,int RestId)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
                throw new Exception("User not found");

            // Remove existing roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeResult.Succeeded)
                throw new Exception("Failed to remove existing roles");

            // Add new role
            var addResult = await _userManager.AddToRoleAsync(user, "KitchenSupervisor");

            if (!addResult.Succeeded)
                throw new Exception("Failed to assign new role");

            var REslut =await restaurantStaffRepository.AsignRestaurantStaff(RestId,UserId);

            if (!REslut)
            {
                throw new Exception("Failed to assign new role");
            }

            return Ok();
        }
    }
}
