using HomeTask1.Projects.WebApi.Services;
using HomeTask1.Shared;
using Microsoft.AspNetCore.Mvc;

namespace HomeTask1.Projects.WebApi.Controllers;

[ApiController]
[Route("api/userSettings")]
public class UserSettingsController : ControllerBase
{
    private readonly IUserSettingService _userSettingService;
    private readonly ILogger<UserSettingsController> _logger;

    public UserSettingsController(IUserSettingService userSettingService, ILogger<UserSettingsController> logger)
    {
        _userSettingService = userSettingService ?? throw new ArgumentNullException(nameof(userSettingService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Retrieves the user setting for a specific user based on their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose settings are to be retrieved.</param>
    [HttpGet("{userId}")]
    public Task<IActionResult> GetUserSetting(int userId) => 
        RequestHandler.HandleQuery(() => _userSettingService.GetUserSettingByIdAsync(userId), _logger);

    /// <summary>
    /// Retrieves a collection of all user settings.
    /// </summary>
    [HttpGet]
    public Task<IActionResult> GetAllUserSettings() => 
        RequestHandler.HandleQuery(_userSettingService.GetAllUserSettingsAsync, _logger);

    /// <summary>
    /// Adds a new user setting based on the provided request data.
    /// </summary>
    /// <param name="request">An object containing data for the new user setting.</param>
    [HttpPost]
    public Task<IActionResult> AddUserSetting([FromBody] Contracts.V1.CreateUserSetting request) =>
        RequestHandler.HandleCommand(() => _userSettingService.CreateUserSettingAsync(request), _logger, ApiSuccessCode.Created);

    /// <summary>
    /// Updates the user setting for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose settings need to be updated.</param>
    /// <param name="request">The request object containing updated user setting details.</param>
    [HttpPut("{userId}")]
    public Task<IActionResult> UpdateUserSetting(int userId, [FromBody] Contracts.V1.UpdateUserSetting request) =>
        RequestHandler.HandleCommand(() => _userSettingService.UpdateUserSettingAsync(userId, request), _logger,
            ApiSuccessCode.Created);

    /// <summary>
    /// Deletes the user setting associated with the given user ID.
    /// </summary>
    /// <param name="userId">The ID of the user for which the setting needs to be deleted.</param>
    [HttpDelete("{userId}")]
    public Task<IActionResult> DeleteUserSetting(int userId)
        => RequestHandler.HandleCommand(() => _userSettingService.DeleteUserSettingAsync(userId), _logger, ApiSuccessCode.NoContent);
}