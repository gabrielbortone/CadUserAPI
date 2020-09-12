using AutoMapper;
using CadUserAPI.ApplicationCore.Models;

namespace CadUserAPI.ApplicationCore.DTOs.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
        }
    }
}
