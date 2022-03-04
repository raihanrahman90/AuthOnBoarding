using AutoMapper;
using IdentityModel.Client;
using TestingAuthOnBoard.DTOs;
using TestingAuthOnBoard.Models;

namespace TestingAuthOnBoard.Config
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap<TokenResponse, UserTokenDTO>();
            CreateMap<User, UserInfoTokenDTO>();
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
        }
    }
}
