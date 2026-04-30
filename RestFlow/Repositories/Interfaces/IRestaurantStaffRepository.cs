namespace RestFlow.Repositories.Interfaces
{
    public interface IRestaurantStaffRepository
    {
        Task<bool>AsignRestaurantStaff(int RestaurantId,string UserId);
    }
}
