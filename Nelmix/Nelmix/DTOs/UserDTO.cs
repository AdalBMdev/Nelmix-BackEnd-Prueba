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
    }
}
