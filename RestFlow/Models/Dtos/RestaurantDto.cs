namespace RestFlow.Models.Dtos
{
    public class RestaurantDto
    {
        public int Id { get; set; }
        public string name { get; set; }
        public cuisineType cuisine { get; set; }
        public string CuisineName { get; set; }
        public string? email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string? status { get; set; }
        public string imageUrl { get; set; }
        public string openTime { get; set; }
        public string closeTime { get; set; }
        public bool IsActive { get; set; }
        // File upload
        //public IFormFile image { get; set; }
    }

    public class CreateRestaurantDto
    {
        
        public string name { get; set; }
        public cuisineType cuisine { get; set; }
        
        public string? email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string? status { get; set; }
        public string imageUrl { get; set; }
        public string openTime { get; set; }
        public string closeTime { get; set; }
      
        // File upload
        //public IFormFile image { get; set; }
    }
}
