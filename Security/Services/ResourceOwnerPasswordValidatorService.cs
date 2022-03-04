using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;
using TestingAuthOnBoard.Interfaces;
using TestingAuthOnBoard.Repositories;
using System;
using System.Threading.Tasks;

namespace TestingAuthOnBoard.Security.Services
{
    public class ResourceOwnerPasswordValidatorService : IResourceOwnerPasswordValidator
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ResourceOwnerPasswordValidatorService(IUnitOfWork unitOfWork, ILogger<ResourceOwnerPasswordValidatorService> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            _logger.LogDebug($"user {context.UserName} : authenticating ");
            Guid userId = await _unitOfWork.UserRepository.IsAuthenticatedAsync(context.UserName, context.Password);

            if (userId.Equals(Guid.Empty))
            {
                _logger.LogDebug($"user {context.UserName}: invalid username or password");
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid username or password");
            }
            else
            {
                _logger.LogDebug($"user {context.UserName}: authenticated");
                context.Result = new GrantValidationResult(userId.ToString(), "password");
            }

        }

    }
}
