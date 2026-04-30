using System.ComponentModel.DataAnnotations;

namespace RestFlow.Models
{
    public class FoodItemEntity
    {
        [Key]
        public int Id { get; set; }
        public int restaurantId { get; set; }
        public int categoryId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public double price { get; set; }
        public string prepTime { get; set; }
        public bool available { get; set; }        
        public string imagePreview { get; set; }
    }

    public class FoodItemDto
    {
        public int Id { get; set; }
        public int restaurantId { get; set; }
        public string restaurantName { get; set; }
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public double price { get; set; }
        public string prepTime { get; set; }
        public bool available { get; set; }
        public string imagePreview { get; set; }
    }
}
