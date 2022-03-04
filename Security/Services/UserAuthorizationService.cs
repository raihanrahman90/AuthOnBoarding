using AutoMapper;
using IdentityModel.Client;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TestingAuthOnBoard.DTOs;
using TestingAuthOnBoard.Interfaces;
using TestingAuthOnBoard.Models;
using TestingAuthOnBoard.Security.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TestingAuthOnBoard.Security.Services
{
    public class UserAuthorizationService : IUserAuthorizationService
    {

        private readonly IConfiguration _config;
        private readonly IClientStore _clientStore;
        private readonly ClaimsPrincipal _userPrincipal;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;


        public UserAuthorizationService(IConfiguration config, IHttpContextAccessor httpContextAccessor,
            IClientStore clientStore, IMapper mapper, ILogger<UserAuthorizationService> logger, IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _mapper = mapper;
            _clientStore = clientStore;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userPrincipal = httpContextAccessor?.HttpContext?.User;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<UserInfoTokenDTO> LoginAsync(UserLoginDTO userLoginDTO)
        {
            UserTokenDTO userToken = await RequestToken(userLoginDTO);

            User user = await _unitOfWork.UserRepository.GetByUserNameAsync(userLoginDTO.UserName);

            UserInfoTokenDTO userInfoToken = _mapper.Map<User, UserInfoTokenDTO>(user);
            userInfoToken.TokenResponse = userToken;

            return userInfoToken;
        }

        private async Task<UserTokenDTO> RequestToken(UserLoginDTO userLoginDTO)
        {
            try
            {
                DiscoveryDocumentRequest discoReq = new DiscoveryDocumentRequest()
                {
                    Address = _config.GetValue<string>("AuthorizationServer:Address"),
                    Policy = new DiscoveryPolicy()
                    {
                        RequireHttps = false,
                        ValidateEndpoints = false,
                        ValidateIssuerName = false
                    }
                };

                DiscoveryDocumentResponse discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync(discoReq);
                if (discoveryDocument.IsError)
                {
                    Console.WriteLine(discoveryDocument.Error);
                    throw new Exception(discoveryDocument.Error);
                }
                Client client = await _clientStore.FindEnabledClientByIdAsync(_config.GetValue<string>("Service:ClientId"));

                PasswordTokenRequest passwordTokenRequest = new PasswordTokenRequest()
                {
                    Address = discoveryDocument.TokenEndpoint,
                    ClientId = _config.GetValue<string>("Service:ClientId"),
                    ClientSecret = _config.GetValue<string>("Service:ClientSecret"),
                    GrantType = GrantTypes.ResourceOwnerPassword.First(),
                    Scope = client.AllowedScopes.Aggregate((p, n) => p + " " + n),
                    UserName = userLoginDTO.UserName,
                    Password = userLoginDTO.Password
                };

                TokenResponse tokenResponse = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

                if (tokenResponse.IsError)
                {
                    Console.WriteLine("cek apakah masuk");
                    Console.WriteLine(tokenResponse.Error);
                    throw new Exception(tokenResponse.ErrorDescription);
                }

                return _mapper.Map<TokenResponse, UserTokenDTO>(tokenResponse);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw new Exception(e.ToString());
            }

        }

        public Guid GetUserId()
        {
            string userId = _userPrincipal.Claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;
            return new Guid(userId);
        }

        public string GetUserName()
        {
            string userName = _userPrincipal.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Name)?.Value;
            return userName;
        }

        public string GetEmail()
        {
            string userName = _userPrincipal.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Email)?.Value;
            return userName;
        }

        public List<string> GetRole()
        {
            List<string> userName = _userPrincipal.Claims.Where(i => i.Type == ClaimTypes.Role).Select(r => r.Value).ToList();
            return userName;
        }

    }
}
