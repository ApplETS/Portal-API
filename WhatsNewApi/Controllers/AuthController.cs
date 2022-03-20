using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsNewApi.Models.DTOs;
using WhatsNewApi.Services.Abstractions;

namespace WhatsNewApi.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthentificationService _authService;
        private readonly IFirebaseService _firebaseService;

        public AuthController(IAuthentificationService authService, IFirebaseService firebaseService)
        {
            _authService = authService;
            _firebaseService = firebaseService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginCrendentialsDTO userCrendentials)
        {
            if(!string.IsNullOrEmpty(userCrendentials.Email) && !string.IsNullOrEmpty(userCrendentials.Password))
            {
                var user = await _authService.Authenticate(userCrendentials.Email, userCrendentials.Password);
                return Ok(user);
            }
            
            return BadRequest();
        }

        // Only admins should be able to create accounts for others
        [HttpPost("register")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Register([FromBody] UserCreationDTO userDto)
        {
            if(!string.IsNullOrEmpty(userDto.Email) && !string.IsNullOrEmpty(userDto.Password) && userDto.Password.Equals(userDto.PasswordConfirmation))
            {
                var created = await _firebaseService.CreateUser(userDto.Email, userDto.Password, userDto.Role);
                if(created) return Ok();
            }

            return BadRequest();
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshAuth([FromBody] UserDTO userDto)
        {
            var user = await _authService.RefreshAuth(userDto.FirebaseToken, userDto.RefreshToken);
            return Ok(user);
        }
    }
}