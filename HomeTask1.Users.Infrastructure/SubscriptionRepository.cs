using HomeTask1.Users.Domain;
using Microsoft.EntityFrameworkCore;

namespace HomeTask1.Users.Infrastructure;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly UsersDbContext _usersDbContext;

    public SubscriptionRepository(UsersDbContext usersDbContext)
    {
        _usersDbContext = usersDbContext ?? throw new ArgumentNullException(nameof(usersDbContext));
    }
    
    public async Task<bool> HasUsersAsync(int subscriptionId)
    {
        return await _usersDbContext.Users.AnyAsync(u => u.SubscriptionId == subscriptionId);
    }

    public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync()
    {
        return await _usersDbContext.Subscriptions.ToListAsync();
    }

    public async Task<Subscription> GetSubscriptionByIdAsync(int id)
    {
        return await _usersDbContext.Subscriptions.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task AddSubscriptionAsync(Subscription subscription)
    {
        await _usersDbContext.Subscriptions.AddAsync(subscription);
        await _usersDbContext.SaveChangesAsync();
    }

    public async Task UpdateSubscriptionAsync(Subscription subscription)
    {
        var existingSubscription = await _usersDbContext.Subscriptions.FindAsync(subscription.Id);

        if (existingSubscription != null)
        {
            existingSubscription.Type = subscription.Type;
            existingSubscription.StartDate = subscription.StartDate;
            existingSubscription.EndDate = subscription.EndDate;

            _usersDbContext.Subscriptions.Update(existingSubscription);
            await _usersDbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteSubscriptionAsync(int id)
    {
        var subscription = await _usersDbContext.Subscriptions.FindAsync(id);

        if (subscription != null)
        {
            _usersDbContext.Subscriptions.Remove(subscription);
            await _usersDbContext.SaveChangesAsync();
        }
    }

}