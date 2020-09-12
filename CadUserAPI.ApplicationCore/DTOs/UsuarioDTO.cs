using System;
using System.Collections.Generic;
using System.Text;

namespace CadUserAPI.ApplicationCore.DTOs
{
    public class UsuarioDTO
    {
        public Guid Id { get; }
        public string Name { get; }
        public DateTime Created { get; }
        public DateTime Modified { get; }
        public DateTime Last_Login { get; set; }
        public PhoneDTO Phone { get; set; }
    }
}
