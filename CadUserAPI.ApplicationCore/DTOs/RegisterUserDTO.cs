using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CadUserAPI.ApplicationCore.DTOs
{
    class RegisterUserDTO
    {
        public string Name { get; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; }

        [DataType(DataType.Password)]
        public string Password { get; }

        public IEnumerable<PhoneDTO> Phones { get; set; }
    }
}
