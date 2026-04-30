using RestFlow.Models;

namespace RestFlow.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<bool> CreateCartForUserAsync(string userId, saveCartdto cartItems);
        Task<ViewCart> GetUserCart(string userId);
        Task<bool> ChangeQuntityoftheCartItem(int CartItemId,bool increaseQty);
        Task<bool> RemoveCartItem(int CartItemId);



    }
}
