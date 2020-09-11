using System;

namespace CadUserAPI.ApplicationCore.DTOs
{
    public class UsuarioToken
    {
        public UsuarioDTO User { get; set; }
        public bool Authenticated { get; set; }
        public DateTime Expiration { get; set; }
        public string Token { get; set; }
        public MensagemDTO Mensagem { get; set; }
    }
}
