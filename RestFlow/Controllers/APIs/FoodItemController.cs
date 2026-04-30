using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestFlow.Models;
using RestFlow.Repositories.Implementations;
using RestFlow.Repositories.Interfaces;
using System.Security.Claims;

namespace RestFlow.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemController : ControllerBase
    {
        private readonly IFoodItemRepository foodItemRepository;
        public FoodItemController(IFoodItemRepository _foodItemRepository)
        {
            foodItemRepository = _foodItemRepository;
        }


        [Authorize(Roles = "SystemAdmin")]
        [HttpPost("SaveFoodItem")]
        public async Task<IActionResult> CreateFoodItem([FromBody] FoodItemDto dto)
        {

          
            var result = await foodItemRepository.CreateFoodItemAsync(dto);

            if (result)
            {
                return Ok(new { message = "Category Created successfully" });
            }
            else
            {
                return BadRequest(new { message = "Category Created Failed" });
            }
        }

        [Authorize(Roles = "SystemAdmin")]
        [HttpGet("GetAllFoodItems")]
        public async Task<List<FoodItemDto>> GetAllFoodItems(int RestId,int CategoryId)
        {

            return await foodItemRepository.GetFoodItemsByRestaurantandCategoryIdAsync(RestId,CategoryId);
        }

        [Authorize(Roles = "SystemAdmin")]
        [HttpDelete("DeleteFoodItem")]
        public async Task<bool> GetAllFoodItems(int Id)
        {

            return await foodItemRepository.DeleteFoodItem(Id);
        }
    }
}
