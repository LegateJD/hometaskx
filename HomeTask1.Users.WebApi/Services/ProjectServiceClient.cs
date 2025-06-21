using HomeTask1.Users.WebApi.Models;
using Newtonsoft.Json;

namespace HomeTask1.Users.WebApi.Services;

public class ProjectServiceClient : IProjectServiceClient
{
    private readonly HttpClient _httpClient;

    public ProjectServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<bool> HasProjectsAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"/api/projects/byuser/{userId}");

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();
        var projects = await response.Content.ReadAsStringAsync();
        var projectList = JsonConvert.DeserializeObject<List<Project>>(projects);

        return projectList != null && projectList.Any();
    }
    
    public async Task<bool> HasUserSettingsAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"/api/userSettings/{userId}");

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();

        return true;
    }
}