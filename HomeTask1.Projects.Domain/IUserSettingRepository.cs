using HomeTask1.Projects.Domain.Entities;

namespace HomeTask1.Projects.Domain;

public interface IUserSettingRepository
{
    Task<UserSetting> GetUserSettingByUserIdAsync(int userId);

    Task<List<UserSetting>> GetAllUserSettingsAsync();

    Task CreateUserSettingAsync(UserSetting userSetting);

    Task<bool> UpdateUserSettingAsync(int userId, UserSetting updatedUserSetting);
    
    Task<bool> DeleteUserSettingAsync(int userId);
}