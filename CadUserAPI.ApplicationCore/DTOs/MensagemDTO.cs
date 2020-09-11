namespace CadUserAPI.ApplicationCore.DTOs
{
    public class MensagemDTO
    {
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
