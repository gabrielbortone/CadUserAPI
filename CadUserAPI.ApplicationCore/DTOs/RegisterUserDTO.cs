using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CadUserAPI.ApplicationCore.DTOs
{
    public class RegisterUserDTO
    {
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public PhoneDTO Phone { get; set; }
    }
}
