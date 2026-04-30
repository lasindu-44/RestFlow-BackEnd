using System.ComponentModel.DataAnnotations;

namespace RestFlow.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedAt { get; set; }
        public bool IsCheckedOut { get; set; } 
    }
}
