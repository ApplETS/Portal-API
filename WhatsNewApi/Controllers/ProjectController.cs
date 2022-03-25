using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsNewApi.Models.DTOs;
using WhatsNewApi.Services.Abstractions;

namespace WhatsNewApi.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = "Administrator")]
[Route("api/project")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }


    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] ProjectCreationDTO dto)
    {
        try
        {
            if(!string.IsNullOrEmpty(dto.Name) && !string.IsNullOrEmpty(dto.CurrentVersion))
            {
                await _projectService.CreateProject(dto.Name, dto.CurrentVersion);
                return Ok();
            }

            return BadRequest();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPut("update")]
    public async Task<IActionResult> UpdateProject([FromBody] ProjectUpdateDTO dto)
    {
        try
        {
            if (!string.IsNullOrEmpty(dto.Id) && !string.IsNullOrEmpty(dto.CurrentVersion))
            {
                await _projectService.UpdateVersion(dto.Id, dto.CurrentVersion);
                return Ok();
            }

            return BadRequest();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpDelete("{projectId}")]
    public async Task<IActionResult> DeleteProject(string projectId)
    {
        try
        {
            await _projectService.DeleteProject(projectId);
            return Ok();
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{projectId}")]
    public async Task<IActionResult> GetProject(string projectId)
    {
        try
        {
            var project = await _projectService.GetProject(projectId);
            return Ok(project);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        try
        {
            var projects = await _projectService.GetProjects();
            return Ok(projects);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}


