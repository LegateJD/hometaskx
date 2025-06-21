using HomeTask1.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HomeTask1.Users.Domain;

public enum SubscriptionType
{
    Free,
    Super,
    Trial,
}

public class Subscription
{
    public int Id { get; set; }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public SubscriptionType Type { get; set; }
    
    [JsonConverter(typeof(DateTimeCustomConverter))]
    public DateTime StartDate { get; set; }
    
    [JsonConverter(typeof(DateTimeCustomConverter))]
    public DateTime EndDate { get; set; }
    
    [JsonIgnore]
    public ICollection<User>? Users { get; set; }
}