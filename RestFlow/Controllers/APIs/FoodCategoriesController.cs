using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestFlow.Models;
using RestFlow.Models.Dtos;
using RestFlow.Repositories.Implementations;
using RestFlow.Repositories.Interfaces;
using System.Security.Claims;

namespace RestFlow.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodCategoriesController : ControllerBase
    {
        private readonly IFoodCategoriesRepository foodCategoriesRepository;
        public FoodCategoriesController(IFoodCategoriesRepository _foodCategoriesRepository)
        {
            foodCategoriesRepository = _foodCategoriesRepository;
        }


        [Authorize(Roles = "SystemAdmin")]
        [HttpPost("SaveCategory")]
        public async Task<IActionResult> CreateCategory([FromBody]FoodCategoriesDto dto)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await foodCategoriesRepository.CreateFoodCategoryAsync(dto, userId);

            if (result is not null)
            {
                return Ok(new { message = "Category Created successfully" });
            }
            else
            {
                return BadRequest(new { message = "Category Created Failed" });
            }
        }

        [Authorize(Roles = "SystemAdmin")]
        [HttpGet("GetAllFoodCategories")]
        public async Task<List<FoodCategoriesDto>> GetAllFoodCategories(int RestId)
        {

            return await foodCategoriesRepository.GetFoodCategoriesAsync(RestId);
        }

        [Authorize(Roles = "SystemAdmin")]
        [HttpPut("DeactivateFoodCategory")]
        public async Task<bool> DeactivateFoodCategory(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return await foodCategoriesRepository.DeactivateFoodCategoriesAsync(id);

        }
    }
}
