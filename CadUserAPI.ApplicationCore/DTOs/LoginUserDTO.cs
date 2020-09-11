using System.ComponentModel.DataAnnotations;

namespace CadUserAPI.ApplicationCore.DTOs
{
    public class LoginUserDTO
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
