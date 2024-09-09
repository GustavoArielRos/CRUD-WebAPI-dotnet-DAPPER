using AutoMapper;
using CrudDapperVideo.Dto;
using CrudDapperVideo.Models;

namespace CrudDapperVideo.Profiles
{
    public class ProfileAutoMapper : Profile
    {
        public ProfileAutoMapper()
        {   
            //transformando o usuario em usuariolistardto
            CreateMap<Usuario, UsuarioListarDto>();
            
        }
    }
}
