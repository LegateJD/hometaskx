using System.Net;
using System.Text;
using FluentAssertions;
using HomeTask1.Projects.Domain.Entities;
using HomeTask1.Users.Domain;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace HomeTask1.Projects.WebApi.IntegrationTests;

public class ProjectsServiceTests : IClassFixture<ContainersFixture>
{
    private readonly HttpClient _userServiceClient;
    private readonly HttpClient _projectServiceClient;

    public ProjectsServiceTests()
    {
        _userServiceClient = new HttpClient { BaseAddress = new Uri("http://localhost:5002/api/") };
        _projectServiceClient = new HttpClient { BaseAddress = new Uri("http://localhost:5003/api/") };
    }
    
    [Fact]
    public async Task AddProject_WithCorrectUser_ShouldStoreProjectSuccessfully()
    {
        var newSubscription = new
        {
            type = "super",
            StartDate = "2022-05-17 15:28:19",
            EndDate = "2029-01-01 00:00:00"
        };

        var subscriptionContent = new StringContent(JsonConvert.SerializeObject(newSubscription), Encoding.UTF8, "application/json");
        var subscriptionResponse = await _userServiceClient.PostAsync("subscriptions", subscriptionContent);
        subscriptionResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var newUser = new
        {
            id = 1,
            name = "John Doe",
            email = "johndoe@example.com",
            subscriptionId = 1
        };

        var userContent = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");
        var userResponse = await _userServiceClient.PostAsync("users", userContent);

        userResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var getUserResponse = await _userServiceClient.GetAsync($"users/{newUser.id}");
        var getUserContent = await getUserResponse.Content.ReadAsStringAsync();
        getUserResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var fetchedUser = JsonConvert.DeserializeObject<User>(getUserContent)!;

        fetchedUser.Id.Should().Be(newUser.id);
        fetchedUser.Name.Should().Be(newUser.name);
        fetchedUser.Email.Should().Be(newUser.email);
        fetchedUser.SubscriptionId.Should().Be(newUser.subscriptionId);

        var newProject = new
        {
            userId = newUser.id,
            name = "my super project 1",
            charts = new[]
            {
                new
                {
                    symbol = "EURUSD",
                    timeframe = "M5",
                    indicators = new[]
                    {
                        new { name = "MA", parameters = "a=1;b=2" },
                        new { name = "RSI", parameters = "a=1;b=2" }
                    }
                }
            }
        };

        var projectContent = new StringContent(JsonConvert.SerializeObject(newProject), Encoding.UTF8, "application/json");
        var projectResponse = await _projectServiceClient.PostAsync("projects", projectContent);
        var projectResponseContent = await projectResponse.Content.ReadAsStringAsync();
        var createdProject = JsonConvert.DeserializeObject<Project>(projectResponseContent)!;
        projectResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var mongoClient = new MongoDB.Driver.MongoClient("mongodb://localhost:27017");
        var database = mongoClient.GetDatabase("ProjectService");
        var projectsCollection = database.GetCollection<Project>("projects");
        var storedProject = await projectsCollection
            .Find(p => p.Id == createdProject.Id)
            .FirstOrDefaultAsync();

        storedProject.Should().NotBeNull();
        storedProject.Id.Should().Be(createdProject.Id);
        storedProject.UserId.Should().Be(newProject.userId);
        storedProject.Name.Should().Be(newProject.name);

        var storedCharts = storedProject.Charts;
        storedCharts.Should().NotBeNull();
        storedCharts.Count.Should().Be(1);

        var storedChart = storedCharts[0];
        storedChart.Symbol.Should().Be("EURUSD");
        storedChart.Timeframe.Should().Be("M5");

        var storedIndicators = storedChart.Indicators;
        storedIndicators.Should().NotBeNull();
        storedIndicators.Count.Should().Be(2);

        storedIndicators[0].Name.Should().Be("MA");
        storedIndicators[0].Parameters.Should().Be("a=1;b=2");

        storedIndicators[1].Name.Should().Be("RSI");
        storedIndicators[1].Parameters.Should().Be("a=1;b=2");
    }
}