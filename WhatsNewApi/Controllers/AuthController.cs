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

    /// <summary>
    /// An endpoint to login in Firebase will return 200 OK if the login was successful.
    /// </summary>
    /// <param name="userCrendentials">Credential to login with used an email, password</param>
    /// <returns>Return Unauthorized 401 or OK 200 depending on if the login was successful</returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] UserLoginCrendentialsDTO userCrendentials)
    {
        try
        {
            if (!string.IsNullOrEmpty(userCrendentials.Email) && !string.IsNullOrEmpty(userCrendentials.Password))
            {
                var user = await _authService.Authenticate(userCrendentials.Email, userCrendentials.Password);
                return Ok(user);
            }
            return Unauthorized();
        } 
        catch (Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }
    
    /// <summary>
    /// Register allows tyo create new account in the firebase project. Only admins should be able to create accounts for others
    /// </summary>
    /// <param name="userDto">Should have an email, a password and it's confirmation and the member role</param>
    /// <returns>A 200 OK if the registering has been successful or 400 if the user couldn't be created</returns>
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

    /// <summary>
    /// Refresh the JWT token provided 
    /// </summary>
    /// <param name="userDto"></param>
    /// <returns>the user of the requested refresh in a 200 OK, if an error occurs 400 Bad request</returns>
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
