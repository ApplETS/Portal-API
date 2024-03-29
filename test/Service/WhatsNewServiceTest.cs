﻿#pragma warning disable CS8604
using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using PortalUnitTest.Mock;
using WhatsNewApi.Models.Exceptions;
using WhatsNewApi.Models.FirestoreModels;
using WhatsNewApi.Repos.Abstractions;
using WhatsNewApi.Services;

namespace PortalUnitTest.Service;

public class WhatsNewServiceTest
{
    private readonly Mock<ILogger<WhatsNewService>> _loggerMock;
    private readonly Mock<IFirestoreRepository<WhatsNew>> _repoMock;
    private readonly IWhatsNewService _service;

    public WhatsNewServiceTest()
    {
        _loggerMock = new Mock<ILogger<WhatsNewService>>();
        _repoMock = new Mock<IFirestoreRepository<WhatsNew>>();
        _service = new WhatsNewService(_loggerMock.Object, _repoMock.Object);
    }

    [Fact]
    public async Task GetWhatsNewsInRange_ShouldReturnWhatsNewForCorrectRange()
    {
        // Arrange
        _repoMock.Setup(repo => repo.GetAll()).Returns(Task.FromResult(Constants.ValidWhatsNewList));

        // Act
        var whatsNews = await _service.GetWhatsNewsInRange(Constants.ValidProject.Id, "3.0.0", "3.0.4");
        // ValidWhatsNew2 as version 3.0.5, it shouldn't be in range

        // Assert
        whatsNews.Should().Contain(Constants.ValidWhatsNew);
        whatsNews.Should().NotContain(Constants.ValidWhatsNew2);
    }

    [Fact]
    public async Task GetWhatsNewsInRange_ForFirebaseIssues_ShouldThrowFirebaseException()
    {
        // Arrange
        _repoMock.Setup(repo => repo.GetAll()).Throws(new FirebaseException("test"));

        // Act
        Func<Task> action = async () =>
        {
            await _service.GetWhatsNewsInRange(Constants.ValidProject.Id, "3.0.0", "3.0.4");
        };

        // Assert
        await action.Should().ThrowAsync<FirebaseException>();
    }

    [Fact]
    public async Task GetWhatsNewsInRange_ForEmptyRange_ShouldThrowFirebaseException()
    {
        // Arrange
        IEnumerable<WhatsNew> emptyList = new WhatsNew[] { };
        _repoMock.Setup(repo => repo.GetAll()).Returns(Task.FromResult(emptyList));

        // Act
        Func<Task> action = async () =>
        {
            await _service.GetWhatsNewsInRange(Constants.ValidProject.Id, "3.0.0", "3.0.4");
        };

        // Assert
        await action.Should().ThrowAsync<FirebaseException>();
    }

    [Fact]
    public async Task GetWhatsNew_ShouldReturnCorrectVersionAccordingToProject()
    {
        // Arrange
        _repoMock.Setup(repo => repo.GetAll()).Returns(Task.FromResult(Constants.ValidWhatsNewList));

        // Act
        var whatsNew = await _service.GetWhatsNew(Constants.ValidProject.Id, Constants.ValidWhatsNew.Version);

        // Assert
        whatsNew.Should().Be(Constants.ValidWhatsNew);
    }

    [Fact]
    public async Task GetWhatsNew_ShouldThrowArgumentExceptionForInvalidVersion()
    {
        // Arrange
        _repoMock.Setup(repo => repo.GetAll()).Returns(Task.FromResult(Constants.ValidWhatsNewList));

        // Act
        Func<Task> action = async () =>
        {
            var whatsNew = await _service.GetWhatsNew(Constants.ValidProject.Id, Constants.ValidWhatsNew.Id);
        };

        // Assert
        await action.Should().ThrowAsync<FirebaseException>();
    }

    [Fact]
    public async Task GetAllWhatsNews_ShouldThrowForEmptyLists()
    {
        // Arrange
        IEnumerable<WhatsNew> emptyList = new WhatsNew[] { };
        _repoMock.Setup(repo => repo.GetAll()).Returns(Task.FromResult(emptyList));

        // Act
        Func<Task> action = async () =>
        {
            var whatsNew = await _service.GetAllWhatsNews(Constants.ValidProject.Id);
        };

        // Assert
        await action.Should().ThrowAsync<FirebaseException>();
    }

    [Fact]
    public async Task GetAllWhatsNews_ShouldReturnForValidProjectId()
    {
        // Arrange
        _repoMock.Setup(repo => repo.GetAll()).Returns(Task.FromResult(Constants.ValidWhatsNewList));

        // Act
        var whatsNew = await _service.GetAllWhatsNews(Constants.ValidProject.Id);

        // Assert
        whatsNew.Should().BeEquivalentTo(Constants.ValidWhatsNewList);
    }

    [Fact]
    public async Task CreateWhatsNew_ForFirebaseIssues_ShouldThrowFirebaseException()
    {
        // Arrange
        _repoMock.Setup(repo => repo.Create(It.IsAny<WhatsNew>())).ThrowsAsync(new FirebaseException("Invalid whats new"));

        // Act
        Func<Task> action = async () =>
        {
            await _service.CreateWhatsNew(Constants.ValidProject.Id, Constants.ValidWhatsNew.Version, new WhatsNewPage[] { Constants.ValidWhatsNewPage });
        };

        // Assert
        await action.Should().ThrowAsync<FirebaseException>();
    }

    [Fact]
    public async Task CreateWhatsNew_ShouldCallRepoCreate()
    {
        // Arrange

        // Act
        await _service.CreateWhatsNew(Constants.ValidProject.Id, Constants.ValidWhatsNew.Version, new WhatsNewPage[] { Constants.ValidWhatsNewPage });

        // Assert
        _repoMock.Verify(mock => mock.Create(It.IsAny<WhatsNew>()), Times.Once);
    }

    [Fact]
    public async Task UpdateWhatsNew_ShouldCallRepoUpdate()
    {
        // Arrange

        // Act
        await _service.UpdateWhatsNew(Constants.ValidWhatsNew.Id, Constants.ValidWhatsNew);

        // Assert
        _repoMock.Verify(mock => mock.Update(It.IsAny<string>(), It.IsAny<WhatsNew>()), Times.Once);
    }

    [Fact]
    public async Task UpdateWhatsNew_ForFirebaseIssues_ShouldThrowFirebaseException()
    {
        // Arrange
        _repoMock.Setup(repo => repo.Update(It.IsAny<string>(), It.IsAny<WhatsNew>())).ThrowsAsync(new FirebaseException("Invalid whats new"));

        // Act
        Func<Task> action = async () =>
        {
            await _service.UpdateWhatsNew(Constants.ValidWhatsNew.Id, Constants.ValidWhatsNew);
        };

        // Assert
        await action.Should().ThrowAsync<FirebaseException>();
    }

    [Fact]
    public async Task DeleteWhatsNew_ShouldCallRepoDelete()
    {
        // Arrange

        // Act
        await _service.DeleteWhatsNew(Constants.ValidWhatsNew.Id);

        // Assert
        _repoMock.Verify(mock => mock.Delete(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteWhatsNew_ForFirebaseIssues_ShouldThrowFirebaseException()
    {
        // Arrange
        _repoMock.Setup(repo => repo.Delete(It.IsAny<string>())).ThrowsAsync(new FirebaseException("Invalid whats new"));

        // Act
        Func<Task> action = async () =>
        {
            await _service.DeleteWhatsNew(Constants.ValidWhatsNew.Id);
        };

        // Assert
        await action.Should().ThrowAsync<FirebaseException>();
    }
}


