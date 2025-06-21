namespace HomeTask1.Users.WebApi.Services;

/// <summary>
/// Client service for interacting with project service.
/// </summary>
public interface IProjectServiceClient
{
    /// <summary>
    /// Checks if user has associated projects.
    /// </summary>
    /// <param name="userId">User's identifier.</param>
    Task<bool> HasProjectsAsync(int userId);

    /// <summary>
    /// Checks if user has associated user settings.
    /// </summary>
    /// <param name="userId">User's identifier.</param>
    Task<bool> HasUserSettingsAsync(int userId);
}