using AutoMapper;
using CadUserAPI.ApplicationCore.Models;

namespace CadUserAPI.ApplicationCore.DTOs.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Usuario, UsuarioDTO>()
                .ForPath(userDTO => userDTO.Phone.DDD, opts => opts.MapFrom(user => user.DDD))
                .ForPath(userDTO => userDTO.Phone.Number, opts => opts.MapFrom(user => user.PhoneNumber));
            CreateMap<UsuarioDTO, Usuario>()
                .ForPath(user => user.DDD, opts => opts.MapFrom(userDTO => userDTO.Phone.DDD))
                .ForPath(user => user.PhoneNumber, opts => opts.MapFrom(userDTO => userDTO.Phone.Number));
        }
    }
}
