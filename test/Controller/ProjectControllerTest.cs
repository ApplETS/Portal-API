using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PortalUnitTest.Mock;
using WhatsNewApi.Models.FirestoreModels;

namespace PortalUnitTest.Controller;

public class ProjectControllerTest
{
    private ProjectController _controller;
    private readonly Mock<IProjectService> _projectServiceMock;

    public ProjectControllerTest()
    {
        _projectServiceMock = new Mock<IProjectService>();
        _controller = new ProjectController(_projectServiceMock.Object);
    }
    
    [Fact(DisplayName = "GetProject for valid Id, should return OK (200) response with project")]
    public async Task GetProject_ForValidId_ShouldReturnOKWithProject()
    {
        // Arrange
        _projectServiceMock.Setup(service => service.GetProject(Constants.validProject1.Id!))
            .Returns(() => Task.FromResult(Constants.validProject1));

        // Act
        var actionResult = await _controller.GetProject(Constants.validProject1.Id!);
        var project = ((OkObjectResult)actionResult).Value as Project;

        // Assert
        Constants.validProject1.Should().BeSameAs(project);
    }

    [Fact(DisplayName = "GetProject for invalid Id, should return Bad Request (400) response with exception message")]
    public async Task GetProject_ForInvalidId_ShouldReturnBadRequest()
    {
        // Arrange
        var exceptionMessage = "Invalid firebase return";
        _projectServiceMock.Setup(service => service.GetProject(It.IsAny<string>()))
            .Throws(() => new Exception(exceptionMessage));

        // Act
        var actionResult = await _controller.GetProject(Constants.validProject1.Id!);
        var response = ((BadRequestObjectResult)actionResult);
        
        // Assert
        response.StatusCode.Should().Be(400);
        var resMessage = response.Value as string;
        resMessage.Should().BeSameAs(exceptionMessage);
    }
    
    [Fact(DisplayName = "GetAllProject for valid firebase return, should return OK (200) with all projects.")]
    public async Task GetAllProject_ForValidFirebaseReturn_ShouldReturnOKWithProjects()
    {
        // Arrange
        _projectServiceMock.Setup(service => service.GetProjects())
            .Returns(() => Task.FromResult(Constants.validProjectsList));

        // Act
        var actionResult = await _controller.GetAllProjects();
        var projects = ((OkObjectResult)actionResult).Value as IEnumerable<Project>;

        // Assert
        Constants.validProjectsList.Should().BeSameAs(projects);
    }
    
    [Fact(DisplayName = "GetAllProject for invalid firebase return, should return bad request (400) with exception message.")]
    public async Task GetAllProject_ForInvalidFirebaseReturn_ShouldReturnBadRequestWithExceptionMessage()
    {
        // Arrange
        var exceptionMessage = "Invalid firebase return";
        _projectServiceMock.Setup(service => service.GetProjects())
            .Throws(() => new Exception("Invalid firebase return"));

        // Act
        var actionResult = await _controller.GetAllProjects();
        var response = (BadRequestObjectResult)actionResult;
        var project = response.Value as string;

        // Assert
        response.StatusCode.Should().Be(400);
        var resMessage = response.Value as string;
        resMessage.Should().BeSameAs(exceptionMessage);
    }
}

