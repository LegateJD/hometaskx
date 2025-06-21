using HomeTask1.Projects.Domain;
using HomeTask1.Projects.Domain.Entities;
using MongoDB.Driver;

namespace HomeTask1.Projects.Infrastructure;

public class UserSettingRepository : IUserSettingRepository
{
    private readonly IMongoCollection<UserSetting> _userSettings;

    public UserSettingRepository(ProjectsDbContext dbContext)
    {
        _userSettings = dbContext?.UserSettings ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<UserSetting> GetUserSettingByUserIdAsync(int userId)
    {
        return await _userSettings.Find(us => us.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task<List<UserSetting>> GetAllUserSettingsAsync()
    {
        return await _userSettings.Find(_ => true).ToListAsync();
    }

    public async Task CreateUserSettingAsync(UserSetting userSetting)
    {
        await _userSettings.InsertOneAsync(userSetting);
    }

    public async Task<bool> UpdateUserSettingAsync(int userId, UserSetting updatedUserSetting)
    {
        var result = await _userSettings.ReplaceOneAsync(
            us => us.UserId == userId,
            updatedUserSetting);

        return result.MatchedCount > 0;
    }

    public async Task<bool> DeleteUserSettingAsync(int userId)
    {
        var result = await _userSettings.DeleteOneAsync(us => us.UserId == userId);
        return result.DeletedCount > 0;
    }
}
