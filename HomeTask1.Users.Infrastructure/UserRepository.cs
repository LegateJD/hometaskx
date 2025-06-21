using HomeTask1.Users.Domain;
using Microsoft.EntityFrameworkCore;

namespace HomeTask1.Users.Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly UsersDbContext _usersDbContext;

    public UserRepository(UsersDbContext usersDbContext)
    {
        _usersDbContext = usersDbContext ?? throw new ArgumentNullException(nameof(usersDbContext));
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _usersDbContext.Users
            .Include(u => u.Subscription)
            .ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _usersDbContext.Users
            .Include(u => u.Subscription)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    
    public async Task<List<User>> GetUsersBySubscriptionTypeAsync(SubscriptionType subscriptionType)
    {
        return await _usersDbContext.Users
            .Where(u => u.Subscription.Type == subscriptionType)
            .Include(u => u.Subscription)
            .ToListAsync();
    }


    public async Task AddUserAsync(User user)
    {
        await _usersDbContext.Users.AddAsync(user);
        await _usersDbContext.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        var existingUser = await _usersDbContext.Users.FindAsync(user.Id);

        if (existingUser == null) return;
        
        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.SubscriptionId = user.SubscriptionId;

        _usersDbContext.Users.Update(existingUser);
        await _usersDbContext.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _usersDbContext.Users.FindAsync(id);

        if (user == null) return;
        
        _usersDbContext.Users.Remove(user);
        await _usersDbContext.SaveChangesAsync();
    }
}