using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HomeTask1.Projects.Domain.Entities;

public class UserSetting
{
    public string Id { get; set; }
    
    public int UserId { get; set; }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public Language Language { get; set; }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public Theme Theme { get; set; }
}

public enum Language
{
    English,
    Spanish
}

public enum Theme
{
    Light,
    Dark
}
