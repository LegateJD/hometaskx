using CSharpFunctionalExtensions;
using HomeTask1.Shared;
using HomeTask1.Users.Domain;

namespace HomeTask1.Users.WebApi.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IProjectServiceClient _projectServiceClient;

    public UserService(IUserRepository userRepository, ISubscriptionRepository subscriptionRepository, IProjectServiceClient projectServiceClient)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _subscriptionRepository = subscriptionRepository ?? throw new ArgumentNullException(nameof(subscriptionRepository));
        _projectServiceClient = projectServiceClient ?? throw new ArgumentNullException(nameof(projectServiceClient));
    }
    
    public async Task<Result<IEnumerable<User>, ApiError>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsers();

        if (users == null)
        {
            return Result.Success<IEnumerable<User>, ApiError>(new List<User>());
        }

        return Result.Success<IEnumerable<User>, ApiError>(users);
    }

    
    public async Task<Result<User, ApiError>> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);

        if (user == null)
        {
            return Result.Failure<User, ApiError>(
                new ApiError(ApiErrorCode.NotFound, $"User with ID {id} not found."));
        }

        return Result.Success<User, ApiError>(user);
    }

    public async Task<Result<IEnumerable<User>, ApiError>> GetUsersBySubscriptionTypeAsync(string subscriptionType)
    {
        if (!Enum.TryParse<SubscriptionType>(subscriptionType, true, out var subscriptionTypeEnum))
        {
            return Result.Failure<IEnumerable<User>, ApiError>(
                new ApiError(ApiErrorCode.BadRequest, $"Invalid subscription type: {subscriptionType}."));
        }

        var users = await _userRepository.GetUsersBySubscriptionTypeAsync(subscriptionTypeEnum);

        if (users == null)
        {
            return Result.Failure<IEnumerable<User>, ApiError>(
                new ApiError(ApiErrorCode.NotFound, $"No users found with subscription type {subscriptionTypeEnum}."));
        }
        
        return Result.Success<IEnumerable<User>, ApiError>(users);
    }


    public async Task<Result<User, ApiError>> AddUserAsync(Contracts.V1.CreateUser request)
    {
        var subscription = await _subscriptionRepository.GetSubscriptionByIdAsync(request.SubscriptionId);
            
        if (subscription == null)
        {
            return Result.Failure<User, ApiError>(
                new ApiError(ApiErrorCode.BadRequest, $"Subscription with ID {request.SubscriptionId} does not exist.")
            );
        }

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            SubscriptionId = request.SubscriptionId
        };

        await _userRepository.AddUserAsync(user);

        return Result.Success<User, ApiError>(user);
    }

    public async Task<Result<bool, ApiError>> UpdateUserAsync(int id, Contracts.V1.UpdateUser request)
    {
        if (id <= 0)
        {
            return Result.Failure<bool, ApiError>(
                new ApiError(ApiErrorCode.BadRequest, "The ID parameter must be greater than zero.")
            );
        }

        var subscription = await _subscriptionRepository.GetSubscriptionByIdAsync(request.SubscriptionId);
        
        if (subscription == null)
        {
            return Result.Failure<bool, ApiError>(
                new ApiError(ApiErrorCode.BadRequest, $"Subscription with ID {request.SubscriptionId} does not exist.")
            );
        }

        var existingUser = await _userRepository.GetUserByIdAsync(id);

        if (existingUser == null)
        {
            var newUser = new User
            {
                Id = id,
                Name = request.Name,
                Email = request.Email,
                SubscriptionId = request.SubscriptionId
            };

            await _userRepository.AddUserAsync(newUser);

            return Result.Success<bool, ApiError>(true);
        }

        existingUser.Name = request.Name;
        existingUser.Email = request.Email;
        existingUser.SubscriptionId = request.SubscriptionId;

        await _userRepository.UpdateUserAsync(existingUser);

        return Result.Success<bool, ApiError>(true);

    }

    public async Task<Result<bool, ApiError>> DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        
        if (user == null)
        {
            return Result.Success<bool, ApiError>(true);
        }

        var hasUserSettings = await _projectServiceClient.HasUserSettingsAsync(id);
        
        if (hasUserSettings)
        {
            return Result.Failure<bool, ApiError>(
                new ApiError(ApiErrorCode.BadRequest, $"Cannot delete user with id {id}: User has associated settings.")
            );
        }

        var hasProjects = await _projectServiceClient.HasProjectsAsync(id);
        
        if (hasProjects)
        {
            return Result.Failure<bool, ApiError>(
                new ApiError(ApiErrorCode.BadRequest, $"Cannot delete user with id {id}: User has associated projects.")
            );
        }

        await _userRepository.DeleteUserAsync(id);

        return Result.Success<bool, ApiError>(true);
    }

}