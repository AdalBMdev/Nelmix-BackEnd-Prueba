using Nelmix.Models;
using static Nelmix.DTOs.UserDTO;

namespace Nelmix.Interfaces
{
    public interface IUserService
    {
        Task RegisterUser(RegisterUserRequestDto usuario);
        Task<bool> Login(string email, string password);
        Task<(bool, string)> ChangePassword(string email, string password, string newPassword);
        Task<(bool, string)> AssignAdultResponsible(string mailUserMinor, string mailUserAdult);
        Task ChangeUserStatusInactiveAsync(int userId);
    }
}
