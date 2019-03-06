using AutoMapper;
using jwtApi.Dto;
using jwtApi.Entities;

namespace jwtApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<User, UserAuthenticatedDto>();
            CreateMap<UserAuthenticatedDto, User>();

            CreateMap<User, UserAuthenticationDto>();
            CreateMap<UserAuthenticationDto, User>();

            CreateMap<User, UserNewRegisterDto>();
            CreateMap<UserNewRegisterDto, User>();
        }
    }
}
