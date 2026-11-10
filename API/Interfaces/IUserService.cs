
using API.Models;

namespace API.Interfaces
{
    public interface IUserService
    {
        Task<AppUser> Register(string username, string password);
    }
}