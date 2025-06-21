using CSharpFunctionalExtensions;
using HomeTask1.Shared;
using HomeTask1.Users.Domain;

namespace HomeTask1.Users.WebApi.Services;

/// <summary>
/// Service for managing subscriptions.
/// </summary>
public interface ISubscriptionService
{
    /// <summary>
    /// Retrieves all subscriptions.
    /// </summary>
    Task<Result<IEnumerable<Subscription>, ApiError>> GetAllSubscriptionsAsync();

    /// <summary>
    /// Retrieves a subscription by id.
    /// </summary>
    /// <param name="id">Identifier of the subscription to be retrieved.</param>
    Task<Result<Subscription, ApiError>> GetSubscriptionByIdAsync(int id);
    
    /// <summary>
    /// Adds a new subscription.
    /// </summary>
    /// <param name="request">A model containing the details of the subscription to be added.</param>
    Task<Result<Subscription, ApiError>> AddSubscriptionAsync(Contracts.V1.CreateSubscription request);

    /// <summary>
    /// Updates an existing subscription with the specified ID using the details provided in the update request.
    /// </summary>
    /// <param name="id">The unique identifier of the subscription to be updated.</param>
    /// <param name="request">An object containing the new subscription details such as type, start date, and end date.</param>
    Task<Result<Subscription, ApiError>> UpdateSubscriptionAsync(int id, Contracts.V1.UpdateSubscription request);

    /// <summary>
    /// Deletes a subscription by its unique identifier if it does not contain associated users.
    /// </summary>
    /// <param name="id">The unique identifier of the subscription to be deleted.</param>
    Task<Result<bool, ApiError>> DeleteSubscriptionAsync(int id);
}