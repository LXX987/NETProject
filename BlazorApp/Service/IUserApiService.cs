using BlazorApp.Data;

namespace BlazorApp.Service
{
    public interface IUserApiService
    {
        Task<List<User>> GetHolidays(User userRequest);
        
    }
}
