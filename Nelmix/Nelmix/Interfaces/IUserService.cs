using Nelmix.Models;

namespace Nelmix.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUser(Usuario usuario);
        Task<bool> Login(string email, string password);
        Task<(bool, string)> ChangePassword(string email, string password, string newPassword);
        Task<(bool, string)> AssignAdultResponsible(string mailUserMinor, string mailUserAdult);
        Task ChangeUserStatusInactiveAsync(int userId);
    }
}
