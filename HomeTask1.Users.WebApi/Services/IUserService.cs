using CSharpFunctionalExtensions;
using HomeTask1.Shared;
using HomeTask1.Users.Domain;

namespace HomeTask1.Users.WebApi.Services;

/// <summary>
/// Service for managing users.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves a collection of all users.
    /// </summary>
    Task<Result<IEnumerable<User>, ApiError>> GetAllUsersAsync();

    /// <summary>
    /// Retrieves a user by their identifier.
    /// </summary>
    /// <param name="id">
    /// The unique identifier of the user to retrieve.
    /// </param>
    Task<Result<User, ApiError>> GetUserByIdAsync(int id);

    /// <summary>
    /// Retrieves a collection of users based on the provided subscription type.
    /// </summary>
    /// <param name="subscriptionType">
    /// The subscription type used to filter the users.
    /// </param>
    Task<Result<IEnumerable<User>, ApiError>> GetUsersBySubscriptionTypeAsync(string subscriptionType);
    
    /// <summary>
    /// Adds a new user.
    /// </summary>
    /// <param name="request">
    /// The details of the user to be added, including name, email, and subscription ID.
    /// </param>
    Task<Result<User, ApiError>> AddUserAsync(Contracts.V1.CreateUser request);

    /// <summary>
    /// Updates an existing user  with the specified details.
    /// </summary>
    /// <param name="id">The unique identifier of the user to be updated.</param>
    /// <param name="request">The user details containing the updated information.</param>
    Task<Result<bool, ApiError>> UpdateUserAsync(int id, Contracts.V1.UpdateUser request);

    /// <summary>
    /// Deletes a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to be deleted.</param>
    Task<Result<bool, ApiError>> DeleteUserAsync(int id);
}