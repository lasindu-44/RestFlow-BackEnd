using Microsoft.EntityFrameworkCore;
using RestFlow.Models;
using RestFlow.Repositories.Interfaces;

namespace RestFlow.Repositories.Implementations
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext db;
        public CartRepository(AppDbContext context)
        {
            db = context;
        }
        public async Task<bool> CreateCartForUserAsync(string userId, saveCartdto cartItems)
        {
            try
            {
                //Check if user already has an active cart
                var existingCart = await db.Carts.FirstOrDefaultAsync(c => c.UserId == userId && !c.IsCheckedOut);
                if (existingCart != null)
                {
                    //Update existing cart with new item

                    //Check if item already exists in cart
                    var existingCartItem = await db.CartItems.FirstOrDefaultAsync(ci => ci.CartId == existingCart.Id && ci.FoodItemId == cartItems.FoodItemId);
                    if (existingCartItem != null)
                    {
                        existingCartItem.Quantity = existingCartItem.Quantity + cartItems.Quantity;
                        existingCartItem.UnitPrice = cartItems.UnitPrice; // Update price in case it has changed

                    }
                    else
                    {
                        var cartItem = new CartItems
                        {
                            CartId = existingCart.Id,
                            RestaurantId = cartItems.RestaurantId,
                            FoodCategoryId = cartItems.FoodCategoryId,
                            FoodItemId = cartItems.FoodItemId,
                            Quantity = cartItems.Quantity,
                            UnitPrice = cartItems.UnitPrice,
                        };
                        await db.CartItems.AddAsync(cartItem);
                    }

                    await db.SaveChangesAsync();
                }
                else
                {
                    var cart = new Cart
                    {
                        UserId = userId,
                        CreatedAt = DateTime.Now,
                        IsCheckedOut = false
                    };
                    await db.Carts.AddAsync(cart);
                    await db.SaveChangesAsync();

                    var cartItem = new CartItems
                    {
                        CartId = cart.Id,
                        RestaurantId = cartItems.RestaurantId,
                        FoodCategoryId = cartItems.FoodCategoryId,
                        FoodItemId = cartItems.FoodItemId,
                        Quantity = cartItems.Quantity,
                        UnitPrice = cartItems.UnitPrice,

                    };
                    await db.CartItems.AddAsync(cartItem);
                    await db.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<ViewCart> GetUserCart(string userId)
        {
            var viewcart = new ViewCart();
            try
            {
                var records = await (from c in db.Carts
                                     join ci in db.CartItems on c.Id equals ci.CartId
                                     join r in db.Restaurants on ci.RestaurantId equals r.Id
                                     join fc in db.FoodCategories on ci.FoodCategoryId equals fc.categoryId
                                     join fi in db.FoodItems on ci.FoodItemId equals fi.Id
                                     where c.UserId == userId && !c.IsCheckedOut
                                     select new CartItemsDto
                                     {
                                         Id = ci.Id,
                                         CartId = c.Id,
                                         RestaurantId = r.Id,
                                         RestaurantName = r.name,
                                         FoodCategoryId = fc.categoryId,
                                         FoodCategoryName = fc.categoryName,
                                         FoodItemId = fi.Id,
                                         FoodItemName = fi.name,
                                         Quantity = ci.Quantity,
                                         UnitPrice = ci.UnitPrice,
                                         ImagePreview = fi.imagePreview
                                     }).ToListAsync();
                var subtotal = records.Sum(x => x.Quantity * x.UnitPrice);
                viewcart.subtotal = subtotal;
                viewcart.Items = records;



            }
            catch (Exception ex)
            {
                return null;
            }


            return viewcart;

        }

        public async Task<bool> ChangeQuntityoftheCartItem(int CartItemId, bool increaseQty)
        {
            try
            {
                var cartItem = await db.CartItems.FindAsync(CartItemId);
                if (cartItem == null)
                    return false;
                if (increaseQty)
                    cartItem.Quantity = cartItem.Quantity + 1;
                else
                {
                    if (cartItem.Quantity > 1)
                        cartItem.Quantity = cartItem.Quantity - 1;
                    else
                        await RemoveCartItem(CartItemId);
                }

                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RemoveCartItem(int CartItemId)
        {
            try
            {
                var cartItem = await db.CartItems.FindAsync(CartItemId);
                if (cartItem == null)
                    return false;
                db.CartItems.Remove(cartItem);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

