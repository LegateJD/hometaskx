namespace HomeTask1.Users.Domain;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsers();
    
    Task<User> GetUserByIdAsync(int id);
    
    Task AddUserAsync(User user);
    
    Task UpdateUserAsync(User user);
    
    Task DeleteUserAsync(int id);
    
    Task<List<User>> GetUsersBySubscriptionTypeAsync(SubscriptionType subscriptionType);

}