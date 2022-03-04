using Microsoft.AspNetCore.Authorization;

namespace TestingAuthOnBoard.Security.DTOs
{
    public class Permission : IAuthorizationRequirement
    {
        public Permission(string permissionName)
        {
            PermissionName = permissionName;
        }
        public string PermissionName { get; set; }
    }
}
