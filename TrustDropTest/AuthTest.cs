using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TrustDrop.Auth.Bl;
using TrustDrop.Auth.Dal;
using TrustDrop.Common.Database;
using TrustDrop.Common.Error;
using TrustDrop.Common.Result;

namespace TrustDropTest;

[TestFixture]
public class AuthTest : BaseTest
{
    private IAuthDal authDal = null!;
    private IAuthBl authBl = null!;
    private Mock<ILogger<AuthBl>> mockLogger = null!;

    protected override Task OneTimeConcreteSetUp()
    {
        return Task.CompletedTask;
    }

    protected override Task OneTimeConcreteTearDown()
    {
        return Task.CompletedTask;
    }

    protected override Task ConcreteSetUp()
    {
        authDal = new AuthDal(dbContext);
        mockLogger = new Mock<ILogger<AuthBl>>();
        mockTransactional = new Mock<ITransactional>();

        authBl = new AuthBl(authDal, mockLogger.Object, mockTransactional.Object);
        
        return Task.CompletedTask;
    }
    
    protected override Task ConcreteTearDown()
    {
        return Task.CompletedTask;
    }

    #region Registration Tests

    [Test]
    public async Task RegisterUser_WithValidData_ShouldSucceed()
    {
        // Arrange
        var login = "testuser";
        var password = "testPassword123";
        var email = "test@example.com";

        // Act
        var result = await authBl.RegisterUser(login, password, email);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.True);

        var user = await authDal.GetUser(login);
        Assert.That(user, Is.Not.Null);
        Assert.That(user!.Username, Is.EqualTo(login));
        Assert.That(user.Email, Is.EqualTo(email.ToLower()));
    }

    [Test]
    public async Task RegisterUser_WithDuplicateUsername_ShouldReturnConflict()
    {
        // Arrange
        var login = "testuser";
        var password = "testPassword123";
        var email1 = "test1@example.com";
        var email2 = "test2@example.com";

        await authBl.RegisterUser(login, password, email1);

        // Act
        var result = await authBl.RegisterUser(login, password, email2);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Code, Is.EqualTo(ErrorCode.Conflict));
        Assert.That(result.Error, Is.EqualTo(AuthError.USER_NAME_ALREADY_EXIST));
    }

    [Test]
    public async Task RegisterUser_WithDuplicateEmail_ShouldReturnConflict()
    {
        // Arrange
        var login1 = "testuser1";
        var login2 = "testuser2";
        var password = "testPassword123";
        var email = "test@example.com";

        await authBl.RegisterUser(login1, password, email);

        // Act
        var result = await authBl.RegisterUser(login2, password, email);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Code, Is.EqualTo(ErrorCode.Conflict));
        Assert.That(result.Error, Is.EqualTo(AuthError.USER_EMAIL_ALREADY_EXIST));
    }

    [Test]
    public async Task RegisterUser_WithWhitespaceInLoginAndEmail_ShouldTrimAndNormalize()
    {
        // Arrange
        var login = "  testuser  ";
        var password = "testPassword123";
        var email = "  Test@Example.COM  ";

        // Act
        var result = await authBl.RegisterUser(login, password, email);

        // Assert
        Assert.That(result.IsSuccess, Is.True);

        var user = await authDal.GetUser(login.Trim());
        Assert.That(user, Is.Not.Null);
        Assert.That(user!.Username, Is.EqualTo(login.Trim()));
        Assert.That(user.Email, Is.EqualTo(email.Trim().ToLower()));
    }

    #endregion

    #region Login Tests

    [Test]
    public async Task LoginUser_WithValidCredentials_ShouldSucceed()
    {
        // Arrange
        var login = "testuser";
        var password = "testPassword123";
        var email = "test@example.com";

        await authBl.RegisterUser(login, password, email);

        // Act
        var result = await authBl.LoginUser(login, password);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.JwtToken, Is.Not.Empty);
        Assert.That(result.Value.RefreshToken, Is.Not.Empty);
        Assert.That(result.Value.Username, Is.EqualTo(login));
        Assert.That(result.Value.ExpireAt, Is.GreaterThan(DateTime.UtcNow));
    }

    [Test]
    public async Task LoginUser_WithInvalidUsername_ShouldReturnUnauthorized()
    {
        // Arrange
        var login = "nonexistentuser";
        var password = "testPassword123";

        // Act
        var result = await authBl.LoginUser(login, password);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Code, Is.EqualTo(ErrorCode.Unauthorized));
        Assert.That(result.Error, Is.EqualTo(AuthError.USER_WRONG_CREDENTIALS));
    }

    [Test]
    public async Task LoginUser_WithInvalidPassword_ShouldReturnUnauthorized()
    {
        // Arrange
        var login = "testuser";
        var password = "testPassword123";
        var email = "test@example.com";

        await authBl.RegisterUser(login, password, email);

        // Act
        var result = await authBl.LoginUser(login, "wrongPassword");

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Code, Is.EqualTo(ErrorCode.Unauthorized));
        Assert.That(result.Error, Is.EqualTo(AuthError.USER_WRONG_CREDENTIALS));
    }

    [Test]
    public async Task LoginUser_WithWhitespaceInLogin_ShouldTrimAndSucceed()
    {
        // Arrange
        var login = "testuser";
        var password = "testPassword123";
        var email = "test@example.com";

        await authBl.RegisterUser(login, password, email);

        // Act
        var result = await authBl.LoginUser("  " + login + "  ", password);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value!.Username, Is.EqualTo(login));
    }

    [Test]
    public async Task LoginUser_WithDeletedUser_ShouldReturnUnauthorized()
    {
        // Arrange
        var login = "testuser";
        var password = "testPassword123";
        var email = "test@example.com";

        await authBl.RegisterUser(login, password, email);
        var user = await authDal.GetUser(login);
        user!.DeletedAt = DateTime.UtcNow;
        await authDal.UpdateUser(user);

        // Act - need to use DbContext directly to bypass soft delete filter
        var userWithDeleted = await dbContext.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Username == login);
        Assert.That(userWithDeleted!.DeletedAt, Is.Not.Null);

        var result = await authBl.LoginUser(login, password);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Code, Is.EqualTo(ErrorCode.Unauthorized));
    }

    [Test]
    public async Task LoginUser_ShouldUpdateLastLoginTime()
    {
        // Arrange
        var login = "testuser";
        var password = "testPassword123";
        var email = "test@example.com";

        await authBl.RegisterUser(login, password, email);
        var userBefore = await authDal.GetUser(login);
        var lastLoginBefore = userBefore!.LastLogin;

        await Task.Delay(100); // Small delay to ensure timestamp difference

        // Act
        var result = await authBl.LoginUser(login, password);

        // Assert
        Assert.That(result.IsSuccess, Is.True);

        var userAfter = await authDal.GetUser(login);
        Assert.That(userAfter!.LastLogin, Is.Not.Null);
        Assert.That(userAfter.LastLogin, Is.GreaterThan(lastLoginBefore ?? DateTime.MinValue));
    }

    [Test]
    public async Task LoginUser_ShouldCreateRefreshToken()
    {
        // Arrange
        var login = "testuser";
        var password = "testPassword123";
        var email = "test@example.com";

        await authBl.RegisterUser(login, password, email);

        // Act
        var result = await authBl.LoginUser(login, password);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value!.RefreshToken, Is.Not.Empty);

        // Verify refresh token exists in database
        var refreshTokenCount = await dbContext.RefreshTokens.CountAsync();
        Assert.That(refreshTokenCount, Is.EqualTo(1));
    }

    #endregion

    #region Integration Tests

    [Test]
    public async Task RegisterAndLogin_FullFlow_ShouldSucceed()
    {
        // Arrange
        var login = "integrationuser";
        var password = "integrationPassword123";
        var email = "integration@example.com";

        // Act - Register
        var registerResult = await authBl.RegisterUser(login, password, email);

        // Assert - Register
        Assert.That(registerResult.IsSuccess, Is.True);

        // Act - Login
        var loginResult = await authBl.LoginUser(login, password);

        // Assert - Login
        Assert.That(loginResult.IsSuccess, Is.True);
        Assert.That(loginResult.Value!.Username, Is.EqualTo(login));
        Assert.That(loginResult.Value.JwtToken, Is.Not.Empty);
        Assert.That(loginResult.Value.RefreshToken, Is.Not.Empty);
    }

    [Test]
    public async Task MultipleUsers_ShouldBeIsolated()
    {
        // Arrange
        var user1Login = "user1";
        var user2Login = "user2";
        var password = "testPassword123";

        // Act
        await authBl.RegisterUser(user1Login, password, "user1@example.com");
        await authBl.RegisterUser(user2Login, password, "user2@example.com");

        var login1Result = await authBl.LoginUser(user1Login, password);
        var login2Result = await authBl.LoginUser(user2Login, password);

        // Assert
        Assert.That(login1Result.IsSuccess, Is.True);
        Assert.That(login2Result.IsSuccess, Is.True);
        Assert.That(login1Result.Value!.UserId, Is.Not.EqualTo(login2Result.Value!.UserId));
        Assert.That(login1Result.Value.Username, Is.EqualTo(user1Login));
        Assert.That(login2Result.Value.Username, Is.EqualTo(user2Login));
    }

    #endregion
}