using AutoMapper;
using CadUserAPI.ApplicationCore.DTOs;
using CadUserAPI.ApplicationCore.Models;
using CadUserAPI.ApplicationCore.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CadUserAPI.Application.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IConfiguration _configuration;
        private IUsuarioRepository UsuarioRepository;
        private readonly IMapper _mapper;

        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager,
            IConfiguration configuration, IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            UsuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO userInfo)
        {
            var user = await UsuarioRepository.GetByEmailAsync(userInfo.Email);
            if (user.Email != null)
            {
                var result = await _signInManager.PasswordSignInAsync(userInfo.Email,
                userInfo.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return Ok(GeraToken(user));
                }
                else
                {
                    MensagemDTO message = new MensagemDTO("Senha inválida!");
                    return Unauthorized(message);
                }
            }
            else
            {
                MensagemDTO message = new MensagemDTO("Usuário e/ou senha inválidos");
                return BadRequest(message);
            }

        }

        [AllowAnonymous]
        [HttpPost("registrar")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO userInfo)
        {
            var user = new Usuario
            {
                UserName = userInfo.Email,
                Name = userInfo.Name,
                Email = userInfo.Email,
                EmailConfirmed = true,
                PhoneNumber = userInfo.Phone.Number,
                PhoneNumberConfirmed = true
            };

            if (await UsuarioRepository.GetByEmailAsync(user.Email) != null)
            {
                MensagemDTO mensagem = new MensagemDTO("Já existe um usuário com esse email cadastrado!");
                return Conflict(mensagem);
            }

            var result = await _userManager.CreateAsync(user, userInfo.Password);

            if (!result.Succeeded)
            {
                MensagemDTO message = new MensagemDTO("Não foi possível registrar esse usuário!");
                return BadRequest(message);
            }

            await _signInManager.SignInAsync(user, false);

            var aux = GeraToken(user);

            return Ok(aux);
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UsuarioDTO usuarioDTO)
        {
            if (id != usuarioDTO.UserId)
            {
                MensagemDTO message = new MensagemDTO("Erro na alteração: ids diferentes!");
                return BadRequest(message);
            }

            try
            {
                var usuario = _mapper.Map<Usuario>(usuarioDTO);
                await UsuarioRepository.UpdateAsync(usuario);
                return Ok(usuarioDTO);
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {

            if (id == null)
            {
                return NotFound();
            }

            try
            {
                await UsuarioRepository.DeleteAsync(id);
                return Ok();
            }
            catch (Exception Ex)
            {
                return BadRequest(Ex.Message);
            }
        }

        private object GeraToken(Usuario userInfo)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, userInfo.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiracao = _configuration["TokenConfiguration:ExpireMinutes"];
            var expiration = DateTime.UtcNow.AddMinutes(double.Parse(expiracao));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["TokenConfiguration:Issuer"],
                audience: _configuration["TokenConfiguration:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            var userDto = _mapper.Map<UsuarioDTO>(userInfo);

            return new UsuarioTokenDTO()
            {
                User = userDto,
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Mensagem =  new MensagemDTO("Token JWT OK")
            };

        }
    }
}