using HomeTask1.Users.Domain;

namespace HomeTask1.Projects.WebApi.Services;

/// <summary>
/// Client service for interacting with project service.
/// </summary>
public interface IUserServiceClient
{
    /// <summary>
    /// Retrieves a list of users filtered by their subscription type.
    /// </summary>
    /// <param name="subscriptionType">Subscription type.</param>
    Task<List<User>> GetUsersBySubscriptionTypeAsync(string subscriptionType);

    /// <summary>
    /// Checks if a user with the specified user ID exists.
    /// </summary>
    /// <param name="userId">User's identifier.</param>
    Task<bool> UserExistsAsync(int userId);
}