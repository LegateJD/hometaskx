using CSharpFunctionalExtensions;
using HomeTask1.Projects.Domain.Entities;
using HomeTask1.Shared;

namespace HomeTask1.Projects.WebApi.Services;

public interface IUserSettingService
{
    Task<Result<UserSetting, ApiError>> CreateUserSettingAsync(Contracts.V1.CreateUserSetting request);
    
    Task<Result<UserSetting, ApiError>> UpdateUserSettingAsync(int userId, Contracts.V1.UpdateUserSetting request);
    
    Task<Result<bool, ApiError>> DeleteUserSettingAsync(int userId);
    
    Task<Result<IEnumerable<UserSetting>, ApiError>> GetAllUserSettingsAsync();
    
    Task<Result<UserSetting, ApiError>> GetUserSettingByIdAsync(int userId);
}