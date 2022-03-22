using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsNewApi.Models.DTOs;
using WhatsNewApi.Models.FirestoreModels;
using WhatsNewApi.Repos.Abstractions;

namespace WhatsNewApi.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = "Administrator")]
[Route("api/projects/{projectId}/whatsnew")]
public class WhatsNewController : ControllerBase
{
    private readonly IProjectRepository _projectRepo;
    public WhatsNewController(IProjectRepository firestoreService)
    {
        _projectRepo = firestoreService;
    }

    [HttpGet("{version}")]
    [Authorize(Roles = "Administrator")]
    public IActionResult Get()
    {
        return Ok();
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult CreateWhatsNew(string projectId, [FromBody]WhatsNewCreationDTO dto)
    {
        _projectRepo.AddWhatsNew(projectId, dto.Version, dto.Pages.Select(page => new WhatsNewPage
        {
            Title = page.Title,
            Description = page.Description,
            MediaUrl = page.MediaUrl,
            Color = page.Color
        }));
        return Ok();
    }
}


