namespace HomeTask1.Users.Domain;

public interface ISubscriptionRepository
{
    Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync();
    
    Task<Subscription> GetSubscriptionByIdAsync(int id);
    
    Task AddSubscriptionAsync(Subscription subscription);
    
    Task UpdateSubscriptionAsync(Subscription subscription);
    
    Task DeleteSubscriptionAsync(int id);
    
    Task<bool> HasUsersAsync(int subscriptionId);
}