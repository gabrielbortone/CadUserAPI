namespace CadUserAPI.ApplicationCore.Models
{
    public class Phone
    {
        public int PhoneId { get; set; }
        public string DDD { get; }
        public string Number { get; }
        public Usuario Usuario { get; }
        public Phone()
        {

        }
        public Phone(int phoneId, string dDD, string number, Usuario usuario)
        {
            PhoneId = phoneId;
            DDD = dDD;
            Number = number;
            Usuario = usuario;
        }
    }
}