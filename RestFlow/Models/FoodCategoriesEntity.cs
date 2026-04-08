using System.ComponentModel.DataAnnotations;

namespace RestFlow.Models
{
    public class FoodCategoriesEntity
    {
        [Key]
        public int categoryId { get; set; }
        public int restaurantId { get; set; }   
        public string categoryName { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int displayOrder { get; set; }
        public bool isActive { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } 

    }

    public class FoodCategoriesDto
    {
      
        public int categoryId { get; set; }
        public int restaurantId { get; set; }
        public string resturantName { get; set; }  
        public string categoryName { get; set; } 
        public string description { get; set; } 
        public int displayOrder { get; set; }
        public bool isActive { get; set; } 
   
    }
}
