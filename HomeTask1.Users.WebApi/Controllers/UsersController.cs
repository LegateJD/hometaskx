using HomeTask1.Shared;
using HomeTask1.Users.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeTask1.Users.WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    [HttpGet]
    public Task<IActionResult> GetAllUsers() => 
        RequestHandler.HandleQuery(() => _userService.GetAllUsersAsync(), _logger);

    /// <summary>
    /// Retrieves a specific user by their unique identifier.
    /// </summary>
    /// <param name="id">
    /// The unique identifier of the user to retrieve.
    /// </param>
    [HttpGet("{id}")]
    public Task<IActionResult> GetUserById(int id) =>
        RequestHandler.HandleQuery(() => _userService.GetUserByIdAsync(id), _logger);

    /// <summary>
    /// Retrieves a collection of users filtered by a specific subscription type.
    /// </summary>
    /// <param name="subscriptionType">
    /// The subscription type used to filter the users.
    /// </param>
    [HttpGet("bySubscriptionType/{subscriptionType}")]
    public Task<IActionResult> GetUsersBySubscriptionType(string subscriptionType) => 
        RequestHandler.HandleQuery(() => _userService.GetUsersBySubscriptionTypeAsync(subscriptionType), _logger);

    /// <summary>
    /// Adds a new user.
    /// </summary>
    /// <param name="request">
    /// The request object containing details about the user to be created.
    /// </param>
    [HttpPost]
    public Task<IActionResult> AddUser([FromBody] Contracts.V1.CreateUser request) =>
        RequestHandler.HandleCommand(() => _userService.AddUserAsync(request), _logger, ApiSuccessCode.Created);

    /// <summary>
    /// Updates an existing user with the specified details.
    /// </summary>
    /// <param name="id">The unique identifier of the user to be updated.</param>
    /// <param name="request">The request object containing user details to be updated.</param>
    [HttpPut("{id}")]
    public Task<IActionResult> UpdateUser(int id, [FromBody] Contracts.V1.UpdateUser request) =>
        RequestHandler.HandleCommand(() => _userService.UpdateUserAsync(id, request), _logger, ApiSuccessCode.Created);

    /// <summary>
    /// Deletes a user by their unique identifier.
    /// </summary>
    /// <param name="id">
    /// The unique identifier of the user to delete.
    /// </param>
    [HttpDelete("{id}")]
    public Task<IActionResult> DeleteUser(int id) => 
        RequestHandler.HandleCommand(() => _userService.DeleteUserAsync(id), _logger, ApiSuccessCode.NoContent);
}