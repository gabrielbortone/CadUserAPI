using AutoMapper;
using CadUserAPI.ApplicationCore.DTOs;
using CadUserAPI.ApplicationCore.Models;
using CadUserAPI.ApplicationCore.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CadUserAPI.Application.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await UsuarioRepository.GetAllAsync();
            
            if (await IsLoginInvalid())
            {
                MensagemDTO mensagem = new MensagemDTO("Não autorizado!");
                return Unauthorized(mensagem);
            }

            if (usuarios == null)
            {
                MensagemDTO mensagemDTO = new MensagemDTO("Operação inválida! Tente adicionar mais usuários!");
                return BadRequest(mensagemDTO);
            }

            var usuarioResumoDTOs = _mapper.Map<IEnumerable<UsuarioResumoDTO>>(usuarios);
            return Ok(usuarioResumoDTOs);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsyncById(Guid id)
        {
            var usuario = await UsuarioRepository.GetAsync(id);

            if (await IsLoginInvalid())
            {
                MensagemDTO mensagem = new MensagemDTO("Não autorizado!");
                return Unauthorized(mensagem);
            }

            if (usuario == null)
            {
                MensagemDTO mensagemDTO = new MensagemDTO("Usuário não encontrado!");
                return NotFound(mensagemDTO);
            }

            var usuarioDTO = _mapper.Map<UsuarioDTO>(usuario);
            return Ok(usuarioDTO);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO userInfo)
        {
            var user = await _userManager.FindByEmailAsync(userInfo.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email,
                userInfo.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var userDto = _mapper.Map<UsuarioDTO>(user);
                    UsuarioTokenDTO aux = GeraToken(userDto);
                    await UsuarioRepository.UpdateToken(user, aux.Token);
                    return Ok(aux);
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
                return Unauthorized(message);
            }

        }

        [AllowAnonymous]
        [HttpPost("registrar")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO userInfo)
        {
            if (await UsuarioRepository.GetByEmailAsync(userInfo.Email) != null)
            {
                MensagemDTO mensagem = new MensagemDTO("Já existe um usuário com esse email cadastrado!");
                return Conflict(mensagem);
            }

            var user = new Usuario
            {
                UserName = userInfo.Email,
                Name = userInfo.Name,
                Email = userInfo.Email,
                EmailConfirmed = true,
                DDD = userInfo.Phone.DDD,
                PhoneNumber = userInfo.Phone.Number,
                PhoneNumberConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, userInfo.Password);

            if (!result.Succeeded)
            {
                MensagemDTO message = new MensagemDTO("Não foi possível registrar esse usuário!");
                return BadRequest(message);
            }

            await _signInManager.SignInAsync(user, false);

            var userDto = _mapper.Map<UsuarioDTO>(user);

            UsuarioTokenDTO aux = GeraToken(userDto);
            await UsuarioRepository.UpdateToken(user, aux.Token);

            return Ok(aux);
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            MensagemDTO mensagem = new MensagemDTO("Logout com sucesso!");
            return Ok(mensagem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UsuarioDTO usuarioDTO)
        {
            if (id != usuarioDTO.Id)
            {
                MensagemDTO message = new MensagemDTO("Erro na alteração: ids diferentes!");
                return BadRequest(message);
            }

            if (await IsLoginInvalid())
            {
                MensagemDTO mensagem = new MensagemDTO("Não autorizado!");
                return Unauthorized(mensagem);
            }

            try
            {
                var usuario = _mapper.Map<Usuario>(usuarioDTO);
                await UsuarioRepository.UpdateAsync(usuario);
                return Ok(usuarioDTO);
            }
            catch (Exception Ex)
            {
                MensagemDTO mensagemDTO = new MensagemDTO(Ex.Message);
                return BadRequest(mensagemDTO);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await IsLoginInvalid())
            {
                MensagemDTO mensagem = new MensagemDTO("Não autorizado!");
                return Unauthorized(mensagem);
            }

            if (id == null)
            {
                MensagemDTO mensagem = new MensagemDTO
                    ("Conteúdo não encontrado! Verifique se o id está correto e tente novamente");
                return NotFound(mensagem);
            }

            try
            {
                await UsuarioRepository.DeleteAsync(id);
                return Ok();
            }
            catch (Exception Ex)
            {
                MensagemDTO mensagemDTO = new MensagemDTO(Ex.Message);
                return BadRequest(mensagemDTO);
            }
        }

        private async Task<bool> IsLoginInvalid()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await UsuarioRepository.GetAsync(userId);
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString();
            var token = accessToken.Substring(7, accessToken.Length - 7);

            if (ValidateToken(token))
            {
                if (user.Last_Token == token)
                {
                    var ultimoLoginFoiFeitoNoTempo = DateTime.Now
                        .AddMinutes(-double.Parse(_configuration["TokenConfiguration:ExpireMinutes"])) < user.Last_Login;
                    return !ultimoLoginFoiFeitoNoTempo;
                }
                else
                {
                    return true;
                }
                
            }
            else
            {
                return true;
            }
        }

        private bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudience = _configuration["TokenConfiguration:Audience"],
                    ValidIssuer = _configuration["TokenConfiguration:Issuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                         Encoding.UTF8.GetBytes(_configuration["Jwt:key"]))
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }


        private UsuarioTokenDTO GeraToken(UsuarioDTO userInfo)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email)
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

            return new UsuarioTokenDTO()
            {
                User = userInfo,
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Mensagem =  new MensagemDTO("Token JWT OK")
            };

        }
    }
}