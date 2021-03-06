﻿using System;

namespace CadUserAPI.ApplicationCore.DTOs
{
    public class UsuarioDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Last_Login { get; set; }
        public PhoneDTO Phone { get; set; }
    }
}
