using Newtonsoft.Json;

namespace HomeTask1.Users.Domain;

public class User
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public int SubscriptionId { get; set; }
    
    [JsonIgnore]
    public Subscription Subscription { get; set; }
}