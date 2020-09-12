using Microsoft.AspNetCore.Identity;
using System;

namespace CadUserAPI.ApplicationCore.Models
{
    public class Usuario : IdentityUser<Guid>
    {
        //[Key]
        //public Guid UserId { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Last_Login { get; set; }
        public string Last_Token { get; set; }
        public string DDD { get; set; }
        

        public Usuario()
        {
            Created = DateTime.Now;
            Modified = DateTime.Now;
            Last_Login = DateTime.Now;
        }
        public Usuario(string name, string email, string ddd, string phone)
        {
            Name = name;
            Email = email;
            Created = DateTime.Now;
            Modified = DateTime.Now;
            Last_Login = DateTime.Now;
            DDD = ddd;
            PhoneNumber = phone;
        }

        public Usuario(Guid userId, string name, DateTime created, DateTime modified,
            DateTime last_Login, string email, string ddd, string phone)
        {
            Id = userId;
            Name = name;
            Created = created;
            Modified = modified;
            Last_Login = last_Login;
            Email = email;
            DDD = ddd;
            PhoneNumber = phone;
        }
    }
}
