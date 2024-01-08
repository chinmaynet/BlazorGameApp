using BlazorGameApp.Shared;

namespace BlazorGameApp.Server.Services
{
    public interface IUtilityService
    {
        Task<User> GetUser();
    }
}
