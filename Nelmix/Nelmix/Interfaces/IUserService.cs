using Nelmix.Models;
using static Nelmix.DTOs.UserDTO;

namespace Nelmix.Interfaces
{
    public interface IUserService
    {
        Task RegisterUser(RegisterUserRequestDto usuario);
        Task<bool> Login(LoginUserRequestDto usuario);
        Task ChangePassword(ChangePasswordRequestDto changePasswordUsuario);
        Task<(bool, string)> AssignAdultResponsible(string mailUserMinor, string mailUserAdult);
        Task ChangeUserStatusInactiveAsync(int userId);
    }
}
