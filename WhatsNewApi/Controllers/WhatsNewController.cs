using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsNewApi.Models.DTOs;
using WhatsNewApi.Models.FirestoreModels;
using WhatsNewApi.Services.Abstractions;

namespace WhatsNewApi.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = "Administrator")]
[Route("api/projects/{projectId}/whatsnew")]
public class WhatsNewController : ControllerBase
{
    private readonly IProjectService _projectService;

    public WhatsNewController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [AllowAnonymous]
    [HttpGet("{version}")]
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

    [HttpPut("{version}")]
    public async Task<IActionResult> UpdateWhatsNew(string projectId, string version, [FromBody] WhatsNewCreationDTO dto)
    {
        try
        {
            if (!string.IsNullOrEmpty(dto.Version) && dto.Pages != null && dto.Pages.Any())
            {
                var pages = dto.Pages.Select(page => new WhatsNewPage
                {
                    Title = page.Title,
                    Description = page.Description,
                    MediaUrl = page.MediaUrl,
                    Color = page.Color
                });

                await _projectService.UpdateWhatsNew(projectId, version,
                    new WhatsNew { Version = dto.Version, Pages = pages.ToList()});
                return Ok();
            }

            return BadRequest();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{version}")]
    public async Task<IActionResult> DeleteWhatsNew(string projectId, string version)
    {
        try
        {
            await _projectService.DeleteWhatsNew(projectId, version);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}


