using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestFlow.Models;
using RestFlow.Models.Dtos;
using RestFlow.Repositories.Interfaces;
using System.Security.Claims;

namespace RestFlow.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantRepository restaurantRepository;
        public RestaurantController(IRestaurantRepository _restaurantRepository)
        {
            restaurantRepository = _restaurantRepository;
        }


        [Authorize(Roles = "SystemAdmin")]
        [HttpPost("upload")]
        public async Task<IActionResult> CreateRestaurant([FromForm] CreateRestaurantDto restaurantDto)
        {
            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await restaurantRepository.CreateRestaurantAsync(restaurantDto, userId);

            if(result is not null)
            {
                return Ok(new { message = "Restaurant Created successfully" });
            }
            else
            {
                return BadRequest(new { message = "Restaurant Created Failed" });
            }
        }

        [Authorize(Roles = "SystemAdmin")]
        [HttpGet("CuisineTypes")]
        public async Task<List<CuisineTypeDto>> CuisineTypes()
        {

         return await restaurantRepository.GetCuisineTypesAsync();  


        }


        [Authorize(Roles = "SystemAdmin")]
        [HttpGet("fetchRestaurants")]
        public async Task<IEnumerable<RestaurantDto>> fetchRestaurants()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await restaurantRepository.GetAllRestaurantsAsync(userId);

        }

        [Authorize(Roles = "SystemAdmin")]
        [HttpPut("UpdateRestaurant")]
        public async Task<CreateRestaurantDto> UpdateRestaurant(int RestaurantId, [FromForm] CreateRestaurantDto restaurant)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await restaurantRepository.UpdateRestaurantAsync(RestaurantId, restaurant, userId);

        }

        [Authorize(Roles = "SystemAdmin")]
        [HttpPut("DeactivateRestaurant")]
        public async Task<bool> DeactivateRestaurant(int RestaurantId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await restaurantRepository.DeleteRestaurantAsync(RestaurantId,userId);

        }
    }
}
