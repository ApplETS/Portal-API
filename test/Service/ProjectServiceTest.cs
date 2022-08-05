#pragma warning disable CS8604
using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using PortalUnitTest.Mock;
using WhatsNewApi.Models.Exceptions;
using WhatsNewApi.Models.FirestoreModels;
using WhatsNewApi.Repos.Abstractions;
using WhatsNewApi.Services;


namespace PortalUnitTest.Service;

public class ProjectServiceTest
{
    private readonly Mock<ILogger<ProjectService>> _loggerMock;
    private readonly Mock<IFirestoreRepository<Project>> _repoMock;
    private readonly IProjectService _service;

    public ProjectServiceTest()
    {
        _loggerMock = new Mock<ILogger<ProjectService>>();
        _repoMock = new Mock<IFirestoreRepository<Project>>();
        _service = new ProjectService(_loggerMock.Object, _repoMock.Object);
    }

    [Fact]
    public async Task CreateProject_ForFirebaseIssues_ShouldThrowFirebaseException()
    {
        // Arrange
        _repoMock.Setup(repo => repo.Create(It.IsAny<Project>()))
            .Throws(new FirebaseException("Invalid project id"));

        // Act
        Func<Task> action = async () =>
        {
            await _service.CreateProject(Constants.ValidProject.Name, Constants.ValidProject.CurrentVersion);
        };

        // Assert
        await action.Should().ThrowAsync<FirebaseException>();
    }

    [Fact]
    public async Task CreateProject_ShouldCallRepoCreate()
    {
        _repoMock.Setup(repo => repo.Create(It.IsAny<Project>()))
            .Returns(Task.FromResult(Constants.ValidProject));

        // Act
        
        await _service.CreateProject(Constants.ValidProject.Name, Constants.ValidProject.CurrentVersion);

        // Assert
        _repoMock.Verify(mock => mock.Create(It.IsAny<Project>()), Times.Once);
    }

    [Fact]
    public async Task UpdateVersion_ForFirebaseIssues_ShouldThrowFirebaseException()
    {
        // Arrange
        _repoMock.Setup(repo => repo.Get(It.IsAny<string>()))
            .Returns(Task.FromResult(Constants.ValidProject));
        _repoMock.Setup(repo => repo.Update(It.IsAny<string>(), It.IsAny<Project>()))
            .Throws(new FirebaseException("Invalid project id"));

        // Act
        Func<Task> action = async () =>
        {
            await _service.UpdateVersion(Constants.ValidProject.Id, Constants.ValidProject.CurrentVersion);
        };

        // Assert
        await action.Should().ThrowAsync<FirebaseException>();
    }

    [Fact]
    public async Task UpdateVersion_ShouldCallRepoUpdate()
    {
        // Arrange
        _repoMock.Setup(repo => repo.Get(It.IsAny<string>()))
            .Returns(Task.FromResult(Constants.ValidProject));

        // Act
        await _service.UpdateVersion(Constants.ValidProject.Id, Constants.ValidProject.CurrentVersion);

        // Assert
        _repoMock.Verify(mock => mock.Update(It.IsAny<string>(), It.IsAny<Project>()), Times.Once);
    }
    
    [Fact]
    public async Task GetProject_ForFirebaseIssues_ShouldThrowFirebaseException()
    {
        // Arrange
        _repoMock.Setup(repo => repo.Get(It.IsAny<string>()))
            .Throws(new FirebaseException("Invalid project id"));

        // Act
        Func<Task> action = async () =>
        {
            await _service.GetProject(Constants.ValidProject.Id);
        };

        // Assert
        await action.Should().ThrowAsync<FirebaseException>();
    }

    [Fact]
    public async Task GetProject_ShouldCallRepoGet()
    {
        // Arrange
        _repoMock.Setup(repo => repo.Get(It.IsAny<string>()))
            .Returns(Task.FromResult(Constants.ValidProject));

        // Act
        var project = await _service.GetProject(Constants.ValidProject.Id);

        // Assert
        project.Should().BeEquivalentTo(Constants.ValidProject);
    }

    [Fact]
    public async Task GetProjects_ForFirebaseIssues_ShouldThrowFirebaseException()
    {
        // Arrange
        _repoMock.Setup(repo => repo.GetAll())
            .Throws(new FirebaseException("Invalid project id"));

        // Act
        Func<Task> action = async () =>
        {
            await _service.GetProjects();
        };

        // Assert
        await action.Should().ThrowAsync<FirebaseException>();
    }

    [Fact]
    public async Task GetProjects_ShouldCallRepoGetAll()
    {
        // Arrange
        _repoMock.Setup(repo => repo.GetAll())
            .Returns(Task.FromResult(Constants.ValidProjectsList));

        // Act
        var projects = await _service.GetProjects();

        // Assert
        projects.Should().BeEquivalentTo(Constants.ValidProjectsList);
    }

    [Fact]
    public async Task DeleteProject_ForFirebaseIssues_ShouldThrowFirebaseException()
    {
        // Arrange
        _repoMock.Setup(repo => repo.Delete(It.IsAny<string>()))
            .Throws(new FirebaseException("Invalid project id"));

        // Act
        Func<Task> action = async () =>
        {
            await _service.DeleteProject(Constants.ValidProject.Id);
        };

        // Assert
        await action.Should().ThrowAsync<FirebaseException>();
    }

    [Fact]
    public async Task DeleteProject_ShouldCallRepoDelete()
    {
        // Arrange
        _repoMock.Setup(repo => repo.Delete(It.IsAny<string>()))
            .Returns(Task.FromResult(Constants.ValidProject));

        // Act
        await _service.DeleteProject(Constants.ValidProject.Id);

        // Assert
        _repoMock.Verify(mock => mock.Delete(It.IsAny<string>()), Times.Once);
    }
}
