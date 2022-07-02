#pragma warning disable CS8604
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
            Constants.validWhatsNew, Constants.validWhatsNew2
        };
        _whatsNewServiceMock.Setup(service => service
            .GetWhatsNewsInRange(Constants.validProject.Id, Constants.validWhatsNew.Id, Constants.validWhatsNew2.Id))
            .Returns(Task.FromResult(whatsNewsToReturn));

        // Act
        var actionResult = await _controller
            .GetWhatsNewInRangeFromTo(Constants.validProject.Id, Constants.validWhatsNew.Id, Constants.validWhatsNew2.Id);
        var whatsNews = ((OkObjectResult)actionResult).Value as IEnumerable<WhatsNew>;

        // Assert
        whatsNews.Should().BeSameAs(whatsNewsToReturn);
    }

    [Fact]
    public async Task GetWhatsNewByVersion_ForValidVersion_ShouldReturnCorrectWhatsNew()
    {
        // Arrange
        _whatsNewServiceMock.Setup(service => service
            .GetWhatsNew(It.IsAny<string>(), Constants.validDto1.Version))
            .Returns(Task.FromResult(Constants.validWhatsNew));

        // Act
        var actionResult = await _controller
            .GetWhatsNewByVersion(Constants.validProject.Id, Constants.validWhatsNew.Version);
        var whatsNew = ((OkObjectResult)actionResult).Value as WhatsNew;

        // Assert
        whatsNew.Should().Be(Constants.validWhatsNew);
    }
}


