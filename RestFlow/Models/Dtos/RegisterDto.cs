namespace RestFlow.Models.Dtos
{
    public class RegisterDto
    {
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public string email { get; set; } 
        public int phone { get; set; }

        public string password { get; set; } 
        public bool isRestaurantOwner {  get; set; }
    }
}
