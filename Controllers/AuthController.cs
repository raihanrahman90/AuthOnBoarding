using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestingAuthOnBoard.DTOs;
using TestingAuthOnBoard.Security.DTOs;
using TestingAuthOnBoard.Security.Interfaces;
using System.Threading.Tasks;

namespace OB_Net_Core_Auth_Tutorial.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuthorizationService AuthService;

        public AuthController(IUserAuthorizationService authorizationService)
        {
            AuthService = authorizationService;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="user">Model of user login object.</param>
        /// <response code="200">Request successful.</response>
        /// <response code="400">Request failed because of an exception.</response>
        [ProducesResponseType(typeof(UserInfoTokenDTO), 200)]
        [HttpPost]
        [Route("login")]
        public async Task<UserInfoTokenDTO> Get([FromBody] UserLoginDTO user)
        {
            UserInfoTokenDTO userInfo = await AuthService.LoginAsync(user);
            return userInfo;
        }

        /// <summary>
        /// Check authorization only
        /// </summary>
        /// <response code="200">Request successful.</response>
        /// <response code="400">Request failed because of an exception.</response>
        [ProducesResponseType(200)]
        [HttpGet]
        [Route("check")]
        [Authorize]
        public ActionResult CheckAuthRole()
        {
            return Ok();
        }

        /// <summary>
        /// Check authorization with role
        /// </summary>
        /// <response code="200">Request successful.</response>
        /// <response code="400">Request failed because of an exception.</response>
        [ProducesResponseType(200)]
        [HttpGet]
        [Route("check-with-role")]
        [AuthorizedByRole("Admin")]
        public ActionResult CheckAuthOnly()
        {
            return Ok();
        }

    }

}