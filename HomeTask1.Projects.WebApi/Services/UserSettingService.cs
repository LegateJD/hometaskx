using CSharpFunctionalExtensions;
using HomeTask1.Projects.Domain;
using HomeTask1.Projects.Domain.Entities;
using HomeTask1.Shared;

namespace HomeTask1.Projects.WebApi.Services;

public class UserSettingService : IUserSettingService
{
    private readonly IUserSettingRepository _userSettingRepository;
    private readonly IUserServiceClient _userServiceClient;
    
    public UserSettingService(IUserSettingRepository userSettingRepository, IUserServiceClient userServiceClient)
    {
        _userSettingRepository = userSettingRepository ?? throw new ArgumentNullException(nameof(userSettingRepository));
        _userServiceClient = userServiceClient ?? throw new ArgumentNullException(nameof(userServiceClient));
    }
    
    public async Task<Result<UserSetting, ApiError>> GetUserSettingByIdAsync(int userId)
    {
        if (userId <= 0)
        {
            return Result.Failure<UserSetting, ApiError>(
                new ApiError(ApiErrorCode.BadRequest, "User ID must be greater than zero.")
            );
        }

        var userSetting = await _userSettingRepository.GetUserSettingByUserIdAsync(userId);
        
        if (userSetting == null)
        {
            return Result.Failure<UserSetting, ApiError>(
                new ApiError(ApiErrorCode.NotFound, "UserSetting not found for the specified User ID.")
            );
        }

        return Result.Success<UserSetting, ApiError>(userSetting);
    }

    
    public async Task<Result<IEnumerable<UserSetting>, ApiError>> GetAllUserSettingsAsync()
    {
        var userSettings = await _userSettingRepository.GetAllUserSettingsAsync();

        if (userSettings == null)
        {
            return Result.Success<IEnumerable<UserSetting>, ApiError>(new List<UserSetting>());
        }
        
        return Result.Success<IEnumerable<UserSetting>, ApiError>(userSettings);
    }
    
    public async Task<Result<UserSetting, ApiError>> CreateUserSettingAsync(Contracts.V1.CreateUserSetting request)
    {
        var userExists = await _userServiceClient.UserExistsAsync(request.UserId);
        
        if (!userExists)
        {
            return Result.Failure<UserSetting, ApiError>(new ApiError(ApiErrorCode.Conflict,
                "The specified User does not exist."));
        }

        var userSettingExists = await _userSettingRepository.GetUserSettingByUserIdAsync(request.UserId);
        
        if (userSettingExists != null)
        {
            return Result.Failure<UserSetting, ApiError>(new ApiError(ApiErrorCode.Conflict,
                "A UserSetting for the specified UserId already exists."));
        }

        var userSetting = new UserSetting
        {
            UserId = request.UserId,
            Language = Enum.Parse<Language>(request.Language, true),
            Theme = Enum.Parse<Theme>(request.Theme, true)
        };

        await _userSettingRepository.CreateUserSettingAsync(userSetting);

        return Result.Success<UserSetting, ApiError>(userSetting);
    }

    public async Task<Result<UserSetting, ApiError>> UpdateUserSettingAsync(int userId, Contracts.V1.UpdateUserSetting request)
    {
        var existingUserSetting = await _userSettingRepository.GetUserSettingByUserIdAsync(userId);
        
        if (existingUserSetting != null)
        {
            existingUserSetting.Language = Enum.Parse<Language>(request.Language, true);
            existingUserSetting.Theme = Enum.Parse<Theme>(request.Theme, true);

            var isUpdated = await _userSettingRepository.UpdateUserSettingAsync(userId, existingUserSetting);

            return Result.Success<UserSetting, ApiError>(existingUserSetting);
        }

        var userExists = await _userServiceClient.UserExistsAsync(userId);
            
        if (!userExists)
        {
            return Result.Failure<UserSetting, ApiError>(new ApiError(ApiErrorCode.Conflict,
                "The specified User does not exist."));
        }
            
        var newUserSetting = new UserSetting
        {
            UserId = userId,
            Language = Enum.Parse<Language>(request.Language, true),
            Theme = Enum.Parse<Theme>(request.Theme, true)
        };
            
        await _userSettingRepository.CreateUserSettingAsync(newUserSetting);
            
        return Result.Success<UserSetting, ApiError>(newUserSetting);
    }
    
    public async Task<Result<bool, ApiError>> DeleteUserSettingAsync(int userId)
    {
        if (userId <= 0)
        {
            return Result.Failure<bool, ApiError>(
                new ApiError(ApiErrorCode.BadRequest, "User ID must be greater than zero.")
            );
        }

        var existingUserSetting = await _userSettingRepository.GetUserSettingByUserIdAsync(userId);
        
        if (existingUserSetting == null)
        {
            return Result.Success<bool, ApiError>(true);
        }

        var isDeleted = await _userSettingRepository.DeleteUserSettingAsync(userId);

        return isDeleted
            ? Result.Success<bool, ApiError>(true)
            : Result.Failure<bool, ApiError>(
                new ApiError(ApiErrorCode.InternalServerError, "Failed to delete the UserSetting.")
            );

    }
}