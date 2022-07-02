#pragma warning disable CS8604
using System;
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
        _repoMock.Setup(repo => repo.GetAll()).Returns(Task.FromResult(Constants.validWhatsNewList));

        // Act
        var whatsNews = await _service.GetWhatsNewsInRange(Constants.validProject1.Id, "3.0.0", "3.0.4");
        // validWhatsNew2 as version 3.0.5, it shouldn't be in range

        // Assert
        whatsNews.Should().Contain(Constants.validWhatsNew);
        whatsNews.Should().NotContain(Constants.validWhatsNew2);
    }

    [Fact]
    public async Task GetWhatsNew_ShouldReturnCorrectVersionAccordingToProject()
    {
        // Arrange
        _repoMock.Setup(repo => repo.GetAll()).Returns(Task.FromResult(Constants.validWhatsNewList));

        // Act
        var whatsNew = await _service.GetWhatsNew(Constants.validProject1.Id, Constants.validWhatsNew.Version);

        // Assert
        whatsNew.Should().Be(Constants.validWhatsNew);
    }

    [Fact]
    public async Task GetWhatsNew_ShouldThrowArgumentExceptionForInvalidVersion()
    {
        // Arrange
        _repoMock.Setup(repo => repo.GetAll()).Returns(Task.FromResult(Constants.validWhatsNewList));

        // Act
        Func<Task> action = async () =>
        {
            var whatsNew = await _service.GetWhatsNew(Constants.validProject1.Id, Constants.validWhatsNew.Id);
        };

        // Assert
        await action.Should().ThrowAsync<FirebaseException>();
    }
}


