using HomeTask1.Projects.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace HomeTask1.Projects.Infrastructure;

public class ProjectsDbContext
{
    private readonly IMongoDatabase _database;

    public ProjectsDbContext(IConfiguration config)
    {
        var client = new MongoClient(config.GetConnectionString("MongoDb"));
        _database = client.GetDatabase("ProjectService");
    }

    public IMongoCollection<Project> Projects => _database.GetCollection<Project>("projects");
    public IMongoCollection<UserSetting> UserSettings => _database.GetCollection<UserSetting>("user.settings");
}

public static class MongoConfig
{
    public static void ConfigureMappings()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Project)))
        {
            BsonClassMap.RegisterClassMap<Project>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance);
            });
        }
        
        if (!BsonClassMap.IsClassMapRegistered(typeof(UserSetting)))
        {
            BsonClassMap.RegisterClassMap<UserSetting>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance);
            });
        }
    }
}
