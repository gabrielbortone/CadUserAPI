using CadUserAPI.ApplicationCore.DTOs;
using Microsoft.AspNetCore.Identity;

namespace CadUserAPI.ApplicationCore.Models
{
    public class PortugueseIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DefaultError() 
        {
            return new MensagemDTO($"Um erro desconhecido ocorreu."); 
        }
        public override IdentityError ConcurrencyFailure() 
        { 
            return new MensagemDTO ("Falha de concorrência otimista, o objeto foi modificado."); 
        }
        public override IdentityError PasswordMismatch() 
        { 
            return new MensagemDTO ("Senha incorreta."); 
        }
        public override IdentityError InvalidToken() 
        { 
            return new MensagemDTO("Token inválido."); 
        }
        public override IdentityError LoginAlreadyAssociated() 
        { 
            return new MensagemDTO ("Já existe um usuário com este login."); 
        }
        public override IdentityError InvalidUserName(string userName) 
        { 
            return new MensagemDTO ($"Login '{userName}' é inválido, pode conter apenas letras ou dígitos."); 
        }
        public override IdentityError InvalidEmail(string email) 
        { 
            return new MensagemDTO ($"Email '{email}' é inválido."); 
        }
        public override IdentityError DuplicateUserName(string userName) 
        { 
            return new MensagemDTO ($"Login '{userName}' já está sendo utilizado.");
        }
        public override IdentityError DuplicateEmail(string email) 
        { 
            return new MensagemDTO ($"Email '{email}' já está sendo utilizado."); 
        }
        public override IdentityError InvalidRoleName(string role) 
        { 
            return new MensagemDTO ($"A permissão '{role}' é inválida."); 
        }
        public override IdentityError DuplicateRoleName(string role) 
        { 
            return new MensagemDTO ($"A permissão '{role}' já está sendo utilizada."); 
        }
        public override IdentityError UserAlreadyHasPassword() 
        { 
            return new MensagemDTO ("Usuário já possui uma senha definida."); 
        }
        public override IdentityError UserLockoutNotEnabled() 
        { 
            return new MensagemDTO ("Lockout não está habilitado para este usuário." ); 
        }
        public override IdentityError UserAlreadyInRole(string role) 
        { 
            return new MensagemDTO($"Usuário já possui a permissão '{role}'."); 
        }
        public override IdentityError UserNotInRole(string role) 
        { 
            return new MensagemDTO ($"Usuário não tem a permissão '{role}'."); 
        }
        public override IdentityError PasswordTooShort(int length) 
        { 
            return new MensagemDTO($"Senhas devem conter ao menos {length} caracteres."); 
        }
        public override IdentityError PasswordRequiresNonAlphanumeric() 
        { 
            return new MensagemDTO ("Senhas devem conter ao menos um caracter não alfanumérico."); 
        }
        public override IdentityError PasswordRequiresDigit() 
        { 
            return new MensagemDTO ("Senhas devem conter ao menos um digito ('0'-'9')."); 
        }
        public override IdentityError PasswordRequiresLower() 
        { 
            return new MensagemDTO ("Senhas devem conter ao menos um caracter em caixa baixa ('a'-'z')."); 
        }
        public override IdentityError PasswordRequiresUpper() 
        { 
            return new MensagemDTO ("Senhas devem conter ao menos um caracter em caixa alta ('A'-'Z')."); 
        }
    }
}
