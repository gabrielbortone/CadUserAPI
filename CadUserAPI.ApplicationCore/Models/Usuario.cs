using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CadUserAPI.ApplicationCore.Models
{
    public class Usuario : IdentityUser
    {
        [Key]
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Last_Login { get; set; }
        public IEnumerable<Phone> Phones { get; set; }
        public Usuario()
        {
            Created = DateTime.Now;
            Modified = DateTime.Now;
            Last_Login = DateTime.Now;
        }
        public Usuario(string name, string email, IEnumerable<Phone> phones)
        {
            Name = name;
            Email = email;
            Created = DateTime.Now;
            Modified = DateTime.Now;
            Last_Login = DateTime.Now;
            Phones = phones;
        }

        public Usuario(Guid userId, string name, DateTime created, DateTime modified,
            DateTime last_Login, IEnumerable<Phone> phones)
        {
            UserId = userId;
            Name = name;
            Created = created;
            Modified = modified;
            Last_Login = last_Login;
            Phones = phones;
        }
    }
}
