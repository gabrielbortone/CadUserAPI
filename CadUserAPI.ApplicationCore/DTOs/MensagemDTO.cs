using Microsoft.AspNetCore.Identity;

namespace CadUserAPI.ApplicationCore.DTOs
{
    public class MensagemDTO : IdentityError
    {
        private string Code { get; set; }
        private string Description { get; set; }

        public string Mensagem { get; }
        public MensagemDTO()
        {
            Mensagem = "Erro desconhecido!";
        }
        public MensagemDTO(string mesage)
        {
            Mensagem = mesage;
        }
        public override string ToString()
        {
            return Mensagem;
        }
    }
}
