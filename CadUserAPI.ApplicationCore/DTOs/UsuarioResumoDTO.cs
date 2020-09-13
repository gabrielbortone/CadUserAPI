using System;

namespace CadUserAPI.ApplicationCore.DTOs
{
    public class UsuarioResumoDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public PhoneDTO Phone { get; set; }
    }
}
