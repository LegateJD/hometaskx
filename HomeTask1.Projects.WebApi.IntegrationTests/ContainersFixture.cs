using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Testcontainers.MongoDb;
using Testcontainers.PostgreSql;

namespace HomeTask1.Projects.WebApi.IntegrationTests;

public class ContainersFixture : IAsyncLifetime
{
    private readonly INetwork _network;
    private readonly IContainer _postgresContainer;
    private readonly IContainer _mongoDbContainer;
    private readonly IContainer _userServiceContainer;
    private readonly IContainer _projectServiceContainer;
    
    public ContainersFixture()
    {
        _network = new NetworkBuilder()
            .WithName(Guid.NewGuid().ToString("D"))
            .Build();
        
        _postgresContainer = new PostgreSqlBuilder()
            .WithImage("postgres:17")
            .WithNetwork(_network)
            .WithNetworkAliases("postgres")
            .WithDatabase("postgres")
            .WithUsername("postgres")
            .WithPassword("password")
            .Build();

        _mongoDbContainer = new MongoDbBuilder()
            .WithImage("mongo:8")
            .WithUsername(string.Empty)
            .WithPassword(string.Empty)
            .WithNetwork(_network)
            .WithNetworkAliases("mongo")
            .WithExposedPort(27017)
            .WithPortBinding(27017, 27017)
            .Build();
        
        _userServiceContainer = new ContainerBuilder()
            .WithImage("hometask1.users.webapi")
            .WithNetwork(_network)
            .WithNetworkAliases("userservice")
            .WithEnvironment("ConnectionStrings__Postgres", "Host=postgres;Port=5432;Database=postgres;Username=postgres;Password=password;")
            .WithEnvironment("Services__ProjectService", "http://projectservice")
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
            .WithExposedPort(80)
            .WithPortBinding(5002, 80)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(80))
            .Build();

        _projectServiceContainer = new ContainerBuilder()
            .WithImage("hometask1.projects.webapi")
            .WithNetwork(_network)
            .WithNetworkAliases("projectservice")
            .WithEnvironment("ConnectionStrings__MongoDb", "mongodb://mongo:27017")
            .WithEnvironment("Services__UserService", "http://userservice")
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
            .WithExposedPort(80)
            .WithPortBinding(5003, 80)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(80))
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _network.CreateAsync();
        await _postgresContainer.StartAsync();
        await _mongoDbContainer.StartAsync();
        await _userServiceContainer.StartAsync();
        await _projectServiceContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _projectServiceContainer.StopAsync();
        await _userServiceContainer.StopAsync();
        await _mongoDbContainer.StopAsync();
        await _postgresContainer.StopAsync();
        await _network.DeleteAsync();
    }
}