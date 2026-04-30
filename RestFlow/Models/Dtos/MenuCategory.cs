namespace RestFlow.Models.Dtos
{
    public class MenuCategory
    {
        public FoodCategoriesDto category { get; set; }
        public List<FoodItemDto> items { get; set; }
    }
}
