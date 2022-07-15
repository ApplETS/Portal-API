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
        _projectServiceMock.Setup(service => service.GetProject(Constants.ValidProject.Id!))
            .Returns(() => Task.FromResult(Constants.ValidProject));

        // Act
        var actionResult = await _controller.GetProject(Constants.ValidProject.Id!);
        var project = ((OkObjectResult)actionResult).Value as Project;

        // Assert
        Constants.ValidProject.Should().BeSameAs(project);
    }

    [Fact(DisplayName = "GetProject for invalid Id, should return Bad Request (400) response with exception message")]
    public async Task GetProject_ForInvalidId_ShouldReturnBadRequest()
    {
        // Arrange
        var exceptionMessage = "Invalid firebase return";
        _projectServiceMock.Setup(service => service.GetProject(It.IsAny<string>()))
            .Throws(() => new Exception(exceptionMessage));

        // Act
        var actionResult = await _controller.GetProject(Constants.ValidProject.Id!);
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
            .Returns(() => Task.FromResult(Constants.ValidProjectsList));

        // Act
        var actionResult = await _controller.GetAllProjects();
        var projects = ((OkObjectResult)actionResult).Value as IEnumerable<Project>;

        // Assert
        Constants.ValidProjectsList.Should().BeSameAs(projects);
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

    [Fact]
    public async Task CreateProject_ForValidDto_ShouldReturnOKWithProject()
    {
        // Arrange
        _projectServiceMock.Setup(service => service.CreateProject(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(() => Task.FromResult(Constants.ValidProject));

        // Act
        await _controller.CreateProject(Constants.ValidProjectDto);

        // Assert
        _projectServiceMock.Verify(service => service.CreateProject(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task CreateProject_ForInvalidDto_ShouldReturnBadRequestWithExceptionMessage()
    {
        // Arrange
        var exceptionMessage = "Invalid firebase return";
        _projectServiceMock.Setup(service => service.CreateProject(It.IsAny<string>(), It.IsAny<string>()))
            .Throws(() => new Exception(exceptionMessage));

        // Act
        var actionResult = await _controller.CreateProject(Constants.ValidProjectDto);
        var response = (BadRequestObjectResult)actionResult;

        // Assert
        response.StatusCode.Should().Be(400);
        var resMessage = response.Value as string;
        resMessage.Should().BeSameAs(exceptionMessage);
    }

    [Fact]
    public async Task CreateProject_ForInvalidDto_ShouldReturnBadRequset()
    {
        // Arrange

        // Act
        var actionResult = await _controller.CreateProject(Constants.InvalidProjectDto);
        var response = (BadRequestResult)actionResult;

        // Assert
        response.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task UpdateProject_ForValidDto_ShouldReturnOKWithProject()
    {
        // Arrange
        _projectServiceMock.Setup(service => service.UpdateVersion(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(() => Task.FromResult(Constants.ValidProject));

        // Act
        await _controller.UpdateProject(Constants.ValidProject.Id, Constants.ValidProjectUpdateDto);

        // Assert
        _projectServiceMock.Verify(service => service.UpdateVersion(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdateProject_ForInvalidDto_ShouldReturnBadRequestWithExceptionMessage()
    {
        // Arrange
        var exceptionMessage = "Invalid firebase return";
        _projectServiceMock.Setup(service => service.UpdateVersion(It.IsAny<string>(), It.IsAny<string>()))
            .Throws(() => new Exception(exceptionMessage));

        // Act
        var actionResult = await _controller.UpdateProject(Constants.ValidProject.Id, Constants.ValidProjectUpdateDto);
        var response = (BadRequestObjectResult)actionResult;

        // Assert
        response.StatusCode.Should().Be(400);
        var resMessage = response.Value as string;
        resMessage.Should().BeSameAs(exceptionMessage);
    }

    [Fact]
    public async Task UpdateProject_ForInvalidDto_ShouldReturnBadRequset()
    {
        // Arrange

        // Act
        var actionResult = await _controller.UpdateProject(Constants.ValidProject.Id, Constants.InvalidProjectUpdateDto);
        var response = (BadRequestResult)actionResult;

        // Assert
        response.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task DeleteProject_ForValidId_ShouldReturnOK()
    {
        // Arrange
        _projectServiceMock.Setup(service => service.DeleteProject(It.IsAny<string>()))
            .Returns(() => Task.FromResult(Constants.ValidProject));

        // Act
        await _controller.DeleteProject(Constants.ValidProject.Id!);

        // Assert
        _projectServiceMock.Verify(service => service.DeleteProject(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteProject_ForInvalidId_ShouldReturnBadRequestWithExceptionMessage()
    {
        // Arrange
        var exceptionMessage = "Invalid firebase return";
        _projectServiceMock.Setup(service => service.DeleteProject(It.IsAny<string>()))
            .Throws(() => new Exception(exceptionMessage));

        // Act
        var actionResult = await _controller.DeleteProject(Constants.ValidProject.Id);
        var response = (BadRequestObjectResult)actionResult;

        // Assert
        response.StatusCode.Should().Be(400);
        var resMessage = response.Value as string;
        resMessage.Should().BeSameAs(exceptionMessage);
    }
}

