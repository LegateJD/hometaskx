using FluentAssertions;
using HomeTask1.Shared;
using HomeTask1.Users.Domain;
using HomeTask1.Users.WebApi.Services;
using Moq;

namespace HomeTask1.Users.WebApi.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ISubscriptionRepository> _subscriptionRepositoryMock;
    private readonly Mock<IProjectServiceClient> _projectServiceClientMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
        _projectServiceClientMock = new Mock<IProjectServiceClient>();
        _userService = new UserService(_userRepositoryMock.Object, _subscriptionRepositoryMock.Object, _projectServiceClientMock.Object);
    }
    
    [Fact]
    public async Task AddUser_WithValidSubscription_ShouldReturnSuccess()
    {
        var request = new Contracts.V1.CreateUser
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            SubscriptionId = 1
        };

        _subscriptionRepositoryMock
            .Setup(repo => repo.GetSubscriptionByIdAsync(request.SubscriptionId))
            .ReturnsAsync(new Subscription { Id = request.SubscriptionId });

        var result = await _userService.AddUserAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be(request.Name);
        result.Value.Email.Should().Be(request.Email);
        _userRepositoryMock.Verify(repo => repo.AddUserAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task AddUser_WithInvalidSubscription_ShouldReturnFailure()
    {
        var request = new Contracts.V1.CreateUser
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            SubscriptionId = 1
        };

        _subscriptionRepositoryMock
            .Setup(repo => repo.GetSubscriptionByIdAsync(request.SubscriptionId))
            .ReturnsAsync((Subscription)null);

        var result = await _userService.AddUserAsync(request);

        result.IsFailure.Should().BeTrue();
        result.Error.ErrorCode.Should().Be(ApiErrorCode.BadRequest);
        _userRepositoryMock.Verify(repo => repo.AddUserAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task UpdateUser_WithValidId_ShouldUpdateUser()
    {
        var request = new Contracts.V1.UpdateUser
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            SubscriptionId = 123
        };

        var existingUser = new User { Id = 1, Name = "Old Name", Email = "old.email@example.com", SubscriptionId = 14 };
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(existingUser.Id)).ReturnsAsync(existingUser);
        _subscriptionRepositoryMock.Setup(repo => repo.GetSubscriptionByIdAsync(request.SubscriptionId))
            .ReturnsAsync(new Subscription { Id = request.SubscriptionId });

        var result = await _userService.UpdateUserAsync(1, request);

        result.IsSuccess.Should().BeTrue();
        _userRepositoryMock.Verify(
            repo => repo.UpdateUserAsync(It.Is<User>(user =>
                user.Name == request.Name && user.Email == request.Email &&
                user.SubscriptionId == request.SubscriptionId)), Times.Once);
    }
    
    [Fact]
    public async Task UpdateUser_WithInvalidId_ShouldReturnFailure()
    {
        var request = new Contracts.V1.UpdateUser
        { 
            Name = "John Doe",
            Email = "john.doe@example.com",
            SubscriptionId = 123
        };

        var result = await _userService.UpdateUserAsync(0, request);

        result.IsFailure.Should().BeTrue();
        result.Error.ErrorCode.Should().Be(ApiErrorCode.BadRequest);
        _userRepositoryMock.Verify(repo => repo.UpdateUserAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task UpdateUser_WithMissingSubscription_ShouldReturnFailure()
    {
        var request = new Contracts.V1.UpdateUser
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            SubscriptionId = 123
        };

        _subscriptionRepositoryMock
            .Setup(repo => repo.GetSubscriptionByIdAsync(request.SubscriptionId))
            .ReturnsAsync((Subscription)null);

        var result = await _userService.UpdateUserAsync(1, request);

        result.IsFailure.Should().BeTrue();
        result.Error.ErrorCode.Should().Be(ApiErrorCode.BadRequest);
        _userRepositoryMock.Verify(repo => repo.UpdateUserAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task DeleteUser_WithNoSettingsOrProjects_ShouldReturnSuccess()
    {
        var userId = 1;
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(new User { Id = userId });
        _projectServiceClientMock.Setup(client => client.HasUserSettingsAsync(userId)).ReturnsAsync(false);
        _projectServiceClientMock.Setup(client => client.HasProjectsAsync(userId)).ReturnsAsync(false);

        var result = await _userService.DeleteUserAsync(userId);

        result.IsSuccess.Should().BeTrue();
        _userRepositoryMock.Verify(repo => repo.DeleteUserAsync(userId), Times.Once);
    }

    [Fact]
    public async Task DeleteUser_WithAssociatedSettings_ShouldReturnFailure()
    {
        var userId = 1;
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(new User { Id = userId });
        _projectServiceClientMock.Setup(client => client.HasUserSettingsAsync(userId)).ReturnsAsync(true);

        var result = await _userService.DeleteUserAsync(userId);

        result.IsFailure.Should().BeTrue();
        result.Error.ErrorCode.Should().Be(ApiErrorCode.BadRequest);
        _userRepositoryMock.Verify(repo => repo.DeleteUserAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task DeleteUser_WithNonExistentUser_ShouldReturnSuccess()
    {
        var userId = 123;
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

        var result = await _userService.DeleteUserAsync(userId);

        result.IsSuccess.Should().BeTrue();
        _userRepositoryMock.Verify(repo => repo.DeleteUserAsync(It.IsAny<int>()), Times.Never);
    }
}