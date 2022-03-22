using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsNewApi.Models.DTOs;
using WhatsNewApi.Repos.Abstractions;

namespace WhatsNewApi.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = "Administrator")]
[Route("api/project")]
public class ProjectController : ControllerBase
{
    private readonly IProjectRepository _projectRepo;
    public ProjectController(IProjectRepository firestoreService)
    {
        _projectRepo = firestoreService;
    }


    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] ProjectCreationDTO dto)
    {
        try
        {
            await _projectRepo.Create(dto.Name, dto.CurrentVersion);
            return Ok();
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
            await _projectRepo.UpdateVersion(dto.Id, dto.CurrentVersion);
            return Ok();
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
            await _projectRepo.Delete(projectId);
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
            var project = await _projectRepo.Get(projectId);
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
            var projects = await _projectRepo.GetAll();
            return Ok(projects);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}


