using AutoMapper;
using CadUserAPI.ApplicationCore.Models;

namespace CadUserAPI.ApplicationCore.DTOs.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<Usuario, UsuarioDTO>()
            //    .ForMember(userDto => userDto.Id, opts => opts.MapFrom(user => user.Id))
            //    .ForMember(userDto => userDto.Name, opts => opts.MapFrom(user => user.Name))
            //    .ForMember(userDto => userDto.Email, opts => opts.MapFrom(user => user.Email))
            //    .ForMember(userDto => userDto.Created, opts => opts.MapFrom(user => user.Created))
            //    .ForMember(userDto => userDto.Modified, opts => opts.MapFrom(user => user.Modified))
            //    .ForMember(userDto => userDto.Last_Login, opts => opts.MapFrom(user => user.Last_Login))
            //    .ForPath(userDTO => userDTO.Phone.DDD, opts => opts.MapFrom(user => user.DDD))
            //    .ForPath(userDTO => userDTO.Phone.Number, opts => opts.MapFrom(user => user.PhoneNumber))
            //    .ReverseMap();

            CreateMap<Usuario, UsuarioDTO>()
                .ForPath(userDTO => userDTO.Phone.DDD, opts => opts.MapFrom(user => user.DDD))
                .ForPath(userDTO => userDTO.Phone.Number, opts => opts.MapFrom(user => user.PhoneNumber))
                .ReverseMap();

            CreateMap<Usuario, UsuarioResumoDTO>()
                .ForPath(userRDTO => userRDTO.Phone.DDD, opts => opts.MapFrom(user => user.DDD))
                .ForPath(userRDTO => userRDTO.Phone.Number, opts => opts.MapFrom(user => user.PhoneNumber))
                .ReverseMap();
        }
    }
}
