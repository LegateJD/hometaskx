using CSharpFunctionalExtensions;
using HomeTask1.Shared;
using HomeTask1.Users.Domain;

namespace HomeTask1.Users.WebApi.Services;

public class SubscriptionService: ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    
    public SubscriptionService(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository ?? throw new ArgumentNullException(nameof(subscriptionRepository));
    }
    
    public async Task<Result<IEnumerable<Subscription>, ApiError>> GetAllSubscriptionsAsync()
    {
        var subscriptions = await _subscriptionRepository.GetAllSubscriptionsAsync();

        if (subscriptions == null)
        {
            return Result.Success<IEnumerable<Subscription>, ApiError>(new List<Subscription>());

        }

        return Result.Success<IEnumerable<Subscription>, ApiError>(subscriptions);
    }

    
    public async Task<Result<Subscription, ApiError>> GetSubscriptionByIdAsync(int id)
    {
        var subscription = await _subscriptionRepository.GetSubscriptionByIdAsync(id);

        if (subscription == null)
        {
            return Result.Failure<Subscription, ApiError>(
                new ApiError(ApiErrorCode.NotFound, $"Subscription with ID {id} not found."));
        }

        return Result.Success<Subscription, ApiError>(subscription);
    }

    
    public async Task<Result<Subscription, ApiError>> AddSubscriptionAsync(Contracts.V1.CreateSubscription request)
    {
        var subscription = new Subscription
        {
            Type = Enum.Parse<SubscriptionType>(request.Type, true),
            StartDate = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc),
            EndDate = DateTime.SpecifyKind(request.EndDate, DateTimeKind.Utc)
        };

        await _subscriptionRepository.AddSubscriptionAsync(subscription);

        return Result.Success<Subscription, ApiError>(subscription);
    }
    
    public async Task<Result<Subscription, ApiError>> UpdateSubscriptionAsync(int id, Contracts.V1.UpdateSubscription request)
    {
        var existingSubscription = await _subscriptionRepository.GetSubscriptionByIdAsync(id);
        
        if (existingSubscription == null)
        {
            return Result.Failure<Subscription, ApiError>(
                new ApiError(ApiErrorCode.NotFound, $"Subscription with ID {id} not found.")
            );
        }

        existingSubscription.Type = Enum.Parse<SubscriptionType>(request.Type, true);
        existingSubscription.StartDate = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc);
        existingSubscription.EndDate = DateTime.SpecifyKind(request.EndDate, DateTimeKind.Utc);

        await _subscriptionRepository.UpdateSubscriptionAsync(existingSubscription);

        return Result.Success<Subscription, ApiError>(existingSubscription);
    }
    
    public async Task<Result<bool, ApiError>> DeleteSubscriptionAsync(int id)
    {
        var existingSubscription = await _subscriptionRepository.GetSubscriptionByIdAsync(id);
        
        if (existingSubscription == null)
        {
            return Result.Success<bool, ApiError>(true);
        }

        var hasUsers = await _subscriptionRepository.HasUsersAsync(id);
        
        if (hasUsers)
        {
            return Result.Failure<bool, ApiError>(
                new ApiError(ApiErrorCode.BadRequest, $"Subscription with ID {id} cannot be deleted because it contains existing users.")
            );
        }

        await _subscriptionRepository.DeleteSubscriptionAsync(id);

        return Result.Success<bool, ApiError>(true);
    }
}