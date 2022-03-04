using TestingAuthOnBoard.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestingAuthOnBoard.Interface
{
    public interface IUserService
    {
        Task<UserDTO> CreateUser(UserDTO user);
        Task<UserDTO> GetUser(Guid id);
        Task<List<UserDTO>> GetUser();
        Task<UserDTO> UpdateUser(UserDTO user);
    }
}
