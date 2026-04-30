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
    public class CartController : ControllerBase
    {
        private readonly ICartRepository cartRepository;
        public CartController(ICartRepository _restaurantRepository)
        {
            cartRepository = _restaurantRepository;
        }


        [Authorize]
        [HttpPost("AddtoCart")]
        public async Task<IActionResult> CreateCart([FromBody] saveCartdto cartItems)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await cartRepository.CreateCartForUserAsync(userId,cartItems);

            if (result)
            {
                return Ok(new { message = "Cart Created successfully" });
            }
            else
            {
                return BadRequest(new { message = "Cart Created Failed" });
            }
        }


        [Authorize]
        [HttpGet("GetUserCart")]
        public async Task<ViewCart> GetUserCart()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
          return await cartRepository.GetUserCart(userId);

          
        }

        [Authorize]
        [HttpPost("ChangeQuntityoftheCartItem")]
        public async Task<IActionResult> ChangeQuntityoftheCartItem(int CartItemId, bool increaseQty)
        {

            
            var result = await cartRepository.ChangeQuntityoftheCartItem(CartItemId,increaseQty);

            if (result)
            {
                return Ok(new { message = "Cart updated successfully" });
            }
            else
            {
                return BadRequest(new { message = "Cart updated Failed" });
            }
        }

        [Authorize]
        [HttpPost("RemoveCartItem")]
        public async Task<IActionResult> RemoveCartItem(int CartItemId)
        {


            var result = await cartRepository.RemoveCartItem(CartItemId);

            if (result)
            {
                return Ok(new { message = "Cart updated successfully" });
            }
            else
            {
                return BadRequest(new { message = "Cart updated Failed" });
            }
        }
    }
}
