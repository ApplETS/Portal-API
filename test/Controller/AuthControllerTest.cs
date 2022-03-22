using Microsoft.AspNetCore.Mvc;
using WhatsNewApi.Models.Entities;

namespace WhatsNewUnitTest.Controller;

public class AuthControllerTest
{
    private Mock<IAuthentificationService> _authentificationServiceMock;
    private Mock<IFirebaseService> _firebaseServiceMock;
    private AuthController authController;

    public AuthControllerTest()
    {
        _authentificationServiceMock = new Mock<IAuthentificationService>();
        _firebaseServiceMock = new Mock<IFirebaseService>();
        authController = new AuthController(_authentificationServiceMock.Object, _firebaseServiceMock.Object);
    }

    [Fact]
    public async Task LoginEmailNullTest()
    {
        var user = new UserLoginCrendentialsDTO()
        {
            Email = null,
            Password = "pwd"
        };

        var result = await authController.Login(user);
        var badRequestResult = Assert.IsType<BadRequestResult>(result);

        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task LoginPasswordNullTest()
    {
        var user = new UserLoginCrendentialsDTO()
        {
            Email = "email",
            Password = null
        };

        var result = await authController.Login(user);
        var badRequestResult = Assert.IsType<BadRequestResult>(result);

        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task LoginSuccessTest()
    {
        var expectedModel = new User()
        {
            Email = "email",
            FirstName = "first",
            LastName = "last",
            FirebaseToken = "abc123",
            RefreshToken = "123abc"
        };

        _authentificationServiceMock.Setup(x => x.Authenticate("email", "pwd")).ReturnsAsync(expectedModel);


        var user = new UserLoginCrendentialsDTO()
        {
            Email = "email",
            Password = "pwd"
        };

        var result = await authController.Login(user);
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(expectedModel, okResult.Value);
    }

    [Fact]
    public async Task RegisterEmailNullTest()
    {
        var user = new UserLoginCrendentialsDTO()
        {
            Email = null,
            Password = "pwd"
        };

        var result = await authController.Login(user);
        var badRequestResult = Assert.IsType<BadRequestResult>(result);

        Assert.Equal(400, badRequestResult.StatusCode);
    }
}

