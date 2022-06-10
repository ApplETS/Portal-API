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
[AllowAnonymous]
[Route("api/projects/{projectId}/whatsnew")]
public class WhatsNewController : ControllerBase
{
    private readonly IWhatsNewService _whatsnewService;

    public WhatsNewController(IWhatsNewService whatsnewService)
    {
        _whatsnewService = whatsnewService;
    }

    [AllowAnonymous]
    [HttpGet("{version}")]
    public async Task<IActionResult> GetWhatsNewByVersion(string projectId, string version)
    {
        try
        {
            var whatsNew = await _whatsnewService.GetWhatsNew(projectId, version);
            return Ok(whatsNew);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWhatsNews(string projectId)
    {
        try
        {
            var whatsNews = await _whatsnewService.GetAllWhatsNews(projectId);
            return Ok(whatsNews);
        }
        catch (Exception ex)
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
                await _whatsnewService.CreateWhatsNew(projectId, dto.Version, dto.Pages.Select(page => new WhatsNewPage
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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWhatsNew(string projectId, string id, [FromBody] WhatsNewCreationDTO dto)
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

                await _whatsnewService.UpdateWhatsNew(id,
                    new WhatsNew { Version = dto.Version, Pages = pages.ToList(), ProjectId = projectId });
                return Ok();
            }

            return BadRequest();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWhatsNew(string id)
    {
        try
        {
            await _whatsnewService.DeleteWhatsNew(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}