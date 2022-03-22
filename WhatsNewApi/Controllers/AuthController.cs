using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsNewApi.Models.DTOs;
using WhatsNewApi.Models.Entities;
using WhatsNewApi.Services.Abstractions;

namespace WhatsNewApi.Controllers;

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
        try
        {
            if (!string.IsNullOrEmpty(userDto.Email) && !string.IsNullOrEmpty(userDto.Password) && userDto.Password.Equals(userDto.PasswordConfirmation))
            {
                await _firebaseService.CreateUser(userDto.Email, userDto.Password, userDto.Role);
                return Ok();
            }

            return BadRequest();

        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshAuth([FromBody] UserDTO userDto)
    {
        try
        {
            User? user = null;
            if (!string.IsNullOrEmpty(userDto.FirebaseToken) && !string.IsNullOrEmpty(userDto.RefreshToken))
            {
                user = await _authService.RefreshAuth(userDto.FirebaseToken, userDto.RefreshToken);
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
