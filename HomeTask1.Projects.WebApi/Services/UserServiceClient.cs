using System.Net;
using HomeTask1.Users.Domain;
using Newtonsoft.Json;

namespace HomeTask1.Projects.WebApi.Services;

public class UserServiceClient : IUserServiceClient
{
    private readonly HttpClient _httpClient;

    public UserServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }
    
    public async Task<List<User>> GetUsersBySubscriptionTypeAsync(string subscriptionType)
    {
        var response = await _httpClient.GetAsync($"/api/users/bySubscriptionType/{subscriptionType}");

        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<User>>(content);
    }
    
    public async Task<bool> UserExistsAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"/api/users/{userId}");

        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();
        return false;
    }
}