using TestingAuthOnBoard.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestingAuthOnBoard.Security.Interfaces
{
    public interface IUserAuthorizationService
    {
        Task<UserInfoTokenDTO> LoginAsync(UserLoginDTO user);
        Guid GetUserId();
        string GetUserName();
        string GetEmail();
        List<string> GetRole();
    }
}
