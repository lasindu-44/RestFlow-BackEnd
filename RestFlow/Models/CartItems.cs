using System.ComponentModel.DataAnnotations;

namespace RestFlow.Models
{
    public class CartItems
    {
        [Key]
        public int Id { get; set; }
        public int CartId { get; set; }
        public int RestaurantId { get; set; }
        public int FoodCategoryId { get; set; }
        public int FoodItemId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }

    public class CartItemsDto
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public int FoodCategoryId { get; set; }
        public string FoodCategoryName { get; set; }
        public int FoodItemId { get; set; }
        public string FoodItemName { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public string ImagePreview { get; set; }

    }

    public class saveCartdto
    {
        public int FoodItemId { get; set; }
        public int RestaurantId { get; set; }
        public int FoodCategoryId { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    
    }

    public class ViewCart
    {
        public double subtotal { get; set; }
        public List<CartItemsDto> Items { get; set; }
    }
}
