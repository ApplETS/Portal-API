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
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthentificationService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthentificationService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost(Name = "Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginCrendentialsDTO userCrendentials)
        {
            if(!string.IsNullOrEmpty(userCrendentials.Email) && !string.IsNullOrEmpty(userCrendentials.Password))
            {
                var user = await _authService.Authenticate(userCrendentials.Email, userCrendentials.Password);
                return Ok(user);
            }
            
            return BadRequest();
        }

        [HttpPost("register", Name = "Register")]
        public async Task<IActionResult> Register()
        {
            return Ok();
        }
    }
}