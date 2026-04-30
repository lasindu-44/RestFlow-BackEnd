using System.ComponentModel.DataAnnotations;

namespace RestFlow.Models
{
    public class RestaurantStaff
    {
        [Key]
        public int Id { get; set; } 
        public string UserId { get; set; }
        public int RestautrantId { get; set; }
        public DateTime AsignAt {  get; set; }
    }
}
