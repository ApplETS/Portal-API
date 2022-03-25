using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsNewApi.Models.DTOs;
using WhatsNewApi.Models.FirestoreModels;
using WhatsNewApi.Repos.Abstractions;
using WhatsNewApi.Services.Abstractions;

namespace WhatsNewApi.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/projects/{projectId}/whatsnew")]
public class WhatsNewController : ControllerBase
{
    private readonly IProjectService _projectService;

    public WhatsNewController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet("{version}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetWhatsNew(string projectId, string version)
    {
        try
        {
            var whatsNew = await _projectService.GetWhatsNew(projectId, version);
            return Ok(whatsNew);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateWhatsNew(string projectId, [FromBody]WhatsNewCreationDTO dto)
    {
        try
        {
            if(!string.IsNullOrEmpty(dto.Version) && dto.Pages != null && dto.Pages.Any())
            {
                await _projectService.AddWhatsNew(projectId, dto.Version, dto.Pages.Select(page => new WhatsNewPage
                {
                    Title = page.Title,
                    Description = page.Description,
                    MediaUrl = page.MediaUrl,
                    Color = page.Color
                }));
                return Ok();
            }

            return BadRequest();
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}


