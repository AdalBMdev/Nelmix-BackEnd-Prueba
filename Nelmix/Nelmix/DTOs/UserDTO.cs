using System.ComponentModel.DataAnnotations;

namespace Nelmix.DTOs
{
    public class UserDTO
    {
        public class RegisterUserRequestDto
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class LoginUserRequestDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class ChangePasswordRequestDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string NewPassword { get; set; }
        }

        public class AssignAdultResponsableRequestDto
        {
            public string MailUserMinor { get; set; }
            public string MailUserAdult { get; set; }
        }
    }
}
