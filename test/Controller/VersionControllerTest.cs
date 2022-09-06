#pragma warning disable CS8604
using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PortalUnitTest.Mock;
using WhatsNewApi.Models.FirestoreModels;

namespace PortalUnitTest.Controller;


public class VersionControllerTest
{
    private VersionController _controller;
    private readonly Mock<IWhatsNewService> _whatsNewServiceMock;

    public VersionControllerTest()
    {
        _whatsNewServiceMock = new Mock<IWhatsNewService>();
        _controller = new VersionController(_whatsNewServiceMock.Object);
    }

    [Fact]
    public async Task GetWhatsNewInRangeFromTo_ForValidRange_ShouldReturnCorrectWhatsNews()
    {
        // Arrange
        IEnumerable<WhatsNew> whatsNewsToReturn = new List<WhatsNew> {
            Constants.ValidWhatsNew, Constants.ValidWhatsNew2
        };
        _whatsNewServiceMock.Setup(service => service
            .GetWhatsNewsInRange(Constants.ValidProject.Id, Constants.ValidWhatsNew.Id, Constants.ValidWhatsNew2.Id))
            .Returns(Task.FromResult(whatsNewsToReturn));

        // Act
        var actionResult = await _controller
            .GetWhatsNewInRangeFromTo(Constants.ValidProject.Id, Constants.ValidWhatsNew.Id, Constants.ValidWhatsNew2.Id);
        var whatsNews = ((OkObjectResult)actionResult).Value as IEnumerable<WhatsNew>;

        // Assert
        whatsNews.Should().BeSameAs(whatsNewsToReturn);
    }

    [Fact]
    public async Task GetWhatsNewInRangeFromTo_ForInvalidFirebaseReturn_ShouldReturnBadRequestWithExceptionMessage()
    {
        // Arrange
        var exceptionMessage = "Invalid firebase return";
        _whatsNewServiceMock.Setup(service => service.GetWhatsNewsInRange(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Throws(() => new Exception("Invalid firebase return"));

        // Act
        var actionResult = await _controller.GetWhatsNewInRangeFromTo(Constants.ValidProject.Id, "0.0.0", "9.9.9");
        var response = (BadRequestObjectResult)actionResult;
        var project = response.Value as string;

        // Assert
        response.StatusCode.Should().Be(400);
        var resMessage = response.Value as string;
        resMessage.Should().BeSameAs(exceptionMessage);
    }

    [Fact]
    public async Task GetWhatsNewByVersion_ForValidVersion_ShouldReturnCorrectWhatsNew()
    {
        // Arrange
        _whatsNewServiceMock.Setup(service => service
            .GetWhatsNew(It.IsAny<string>(), Constants.ValidWhatsNewDto1.Version))
            .Returns(Task.FromResult(Constants.ValidWhatsNew));

        // Act
        var actionResult = await _controller
            .GetWhatsNewByVersion(Constants.ValidProject.Id, Constants.ValidWhatsNew.Version);
        var whatsNew = ((OkObjectResult)actionResult).Value as WhatsNew;

        // Assert
        whatsNew.Should().Be(Constants.ValidWhatsNew);
    }

    [Fact]
    public async Task GetWhatsNewByVersion_ForInvalidFirebaseReturn_ShouldReturnBadRequestWithExceptionMessage()
    {
        // Arrange
        var exceptionMessage = "Invalid firebase return";
        _whatsNewServiceMock.Setup(service => service.GetWhatsNew(It.IsAny<string>(), It.IsAny<string>()))
            .Throws(() => new Exception("Invalid firebase return"));

        // Act
        var actionResult = await _controller.GetWhatsNewByVersion(Constants.ValidProject.Id, Constants.ValidWhatsNew.Version);
        var response = (BadRequestObjectResult)actionResult;
        var project = response.Value as string;

        // Assert
        response.StatusCode.Should().Be(400);
        var resMessage = response.Value as string;
        resMessage.Should().BeSameAs(exceptionMessage);
    }

    [Fact]
    public async Task GetAllWhatsNews_ForValidProjectId_ShouldReturnCorrectWhatsNews()
    {
        // Arrange
        _whatsNewServiceMock.Setup(service => service.GetAllWhatsNews(It.IsAny<string>()))
            .Returns(Task.FromResult(Constants.ValidWhatsNewList));

        // Act
        var actionResult = await _controller.GetAllWhatsNews(Constants.ValidProject.Id);
        var whatsNews = ((OkObjectResult)actionResult).Value as IEnumerable<WhatsNew>;

        // Assert
        whatsNews.Should().BeSameAs(Constants.ValidWhatsNewList);
    }

    [Fact]
    public async Task GetAllWhatsNews_ForInvalidFirebaseReturn_ShouldReturnBadRequestWithExceptionMessage()
    {
        // Arrange
        var exceptionMessage = "Invalid firebase return";
        _whatsNewServiceMock.Setup(service => service.GetAllWhatsNews(It.IsAny<string>()))
            .Throws(() => new Exception("Invalid firebase return"));

        // Act
        var actionResult = await _controller.GetAllWhatsNews(Constants.ValidProject.Id);
        var response = (BadRequestObjectResult)actionResult;
        var project = response.Value as string;

        // Assert
        response.StatusCode.Should().Be(400);
        var resMessage = response.Value as string;
        resMessage.Should().BeSameAs(exceptionMessage);
    }

    [Fact]
    public async Task CreateWhatsNew_ForValidDto_ShouldCorrectlyCallService()
    {
        // Arrange

        // Act
        await _controller.CreateWhatsNew(Constants.ValidProject.Id, Constants.ValidWhatsNewDto1);

        // Assert
        _whatsNewServiceMock.Verify(service => service.CreateWhatsNew(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<WhatsNewPage>>()), Times.Once);
    }

    [Fact]
    public async Task CreateWhatsNew_ForInvalidDto_ShouldReturnBadRequest()
    {
        // Arrange

        // Act
        var actionResult = await _controller.CreateWhatsNew(Constants.ValidProject.Id, Constants.InvalidWhatsNewDto1);
        var response = (BadRequestResult)actionResult;

        // Assert
        response.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task CreateWhatsNew_ForInvalidFirebaseReturn_ShouldReturnBadRequestWithExceptionMessage()
    {
        // Arrange
        var exceptionMessage = "Invalid firebase return";
        _whatsNewServiceMock.Setup(service => service.CreateWhatsNew(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<WhatsNewPage>>()))
            .Throws(() => new Exception("Invalid firebase return"));

        // Act
        var actionResult = await _controller.CreateWhatsNew(Constants.ValidProject.Id, Constants.ValidWhatsNewDto1);
        var response = (BadRequestObjectResult)actionResult;

        // Assert
        response.StatusCode.Should().Be(400);
        var resMessage = response.Value as string;
        resMessage.Should().BeSameAs(exceptionMessage);
    }
    
    [Fact]
    public async Task CreateWhatsNew_ForInvalidVersion_ShouldReturnBadRequetWithExceptionMessage()
    {
        // Act
        var actionResult = await _controller.CreateWhatsNew(Constants.ValidProject.Id, Constants.InvalidWhatsNewDto1);
        var response = (BadRequestObjectResult)actionResult;

        // Assert
        response.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task UpdateWhatsNew_ForValidDto_ShouldCorrectlyCallService()
    {
        // Arrange

        // Act
        await _controller.UpdateWhatsNew(Constants.ValidProject.Id, Constants.ValidWhatsNew.Id, Constants.ValidWhatsNewDto1);

        // Assert
        _whatsNewServiceMock.Verify(service => service.UpdateWhatsNew(It.IsAny<string>(), It.IsAny<WhatsNew>()), Times.Once);
    }

    [Fact]
    public async Task UpdateWhatsNew_ForInvalidDto_ShouldReturnBadRequest()
    {
        // Arrange

        // Act
        var actionResult = await _controller.UpdateWhatsNew(Constants.ValidProject.Id, Constants.ValidWhatsNew.Id, Constants.InvalidWhatsNewDto1);
        var response = (BadRequestResult)actionResult;

        // Assert
        response.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task UpdateWhatsNew_ForInvalidFirebaseReturn_ShouldReturnBadRequestWithExceptionMessage()
    {
        // Arrange
        var exceptionMessage = "Invalid firebase return";
        _whatsNewServiceMock.Setup(service => service.UpdateWhatsNew(It.IsAny<string>(), It.IsAny<WhatsNew>()))
            .Throws(() => new Exception("Invalid firebase return"));

        // Act
        var actionResult = await _controller.UpdateWhatsNew(Constants.ValidProject.Id, Constants.ValidWhatsNew.Id, Constants.ValidWhatsNewDto1);
        var response = (BadRequestObjectResult)actionResult;

        // Assert
        response.StatusCode.Should().Be(400);
        var resMessage = response.Value as string;
        resMessage.Should().BeSameAs(exceptionMessage);
    }

    [Fact]
    public async Task UpdateWhatsNew_ForInvalidVersion_ShouldReturnBadRequetWithExceptionMessage()
    {
        // Act
        var actionResult = await _controller.UpdateWhatsNew(Constants.ValidProject.Id, Constants.ValidWhatsNew.Id, Constants.InvalidWhatsNewDto1);
        var response = (BadRequestObjectResult)actionResult;

        // Assert
        response.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task DeleteWhatsNew_ForValidId_ShouldCorrectlyCallService()
    {
        // Arrange

        // Act
        await _controller.DeleteWhatsNew(Constants.ValidWhatsNew.Id);

        // Assert
        _whatsNewServiceMock.Verify(service => service.DeleteWhatsNew(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteWhatsNew_ForInvalidFirebaseReturn_ShouldReturnBadRequestWithExceptionMessage()
    {
        // Arrange
        var exceptionMessage = "Invalid firebase return";
        _whatsNewServiceMock.Setup(service => service.DeleteWhatsNew(It.IsAny<string>()))
            .Throws(() => new Exception("Invalid firebase return"));

        // Act
        var actionResult = await _controller.DeleteWhatsNew(Constants.ValidWhatsNew.Id);
        var response = (BadRequestObjectResult)actionResult;

        // Assert
        response.StatusCode.Should().Be(400);
        var resMessage = response.Value as string;
        resMessage.Should().BeSameAs(exceptionMessage);
    }
}


