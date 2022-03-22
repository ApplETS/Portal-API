using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsNewApi.Repos.Abstractions;

namespace WhatsNewApi.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/whatsnew")]
public class WhatsNewController : ControllerBase
{
    private readonly IProjectRepository _projectRepo;
    public WhatsNewController(IProjectRepository firestoreService)
    {
        _projectRepo = firestoreService;
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public IActionResult Get()
    {
        return Ok();
    }
}


