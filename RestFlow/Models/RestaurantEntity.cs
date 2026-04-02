using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestFlow.Models
{
    public class RestaurantEntity
    {
        [Key]
        public int Id { get; set; } 
        public string name { get; set; }
        public cuisineType cuisine { get; set; }
        [NotMapped]
        public string cuisinename { get; set; }
        public string? email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string imageUrl { get; set; }
        public string openTime { get; set; }
        public string closeTime { get; set; }
        public bool IsActive { get; set; } = true;  
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedAt { get; set; } 
        public string? LastUpdatedBy { get; set; }   

    }
}
