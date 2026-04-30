using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestFlow.Models;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<RestaurantEntity> Restaurants { get; set; }
    public DbSet<FoodCategoriesEntity> FoodCategories { get; set; }
    public DbSet<FoodItemEntity> FoodItems { get; set; }
    public DbSet<RestaurantStaff> RestaurantStaff { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItems> CartItems { get; set; }





}