using HomeTask1.Projects.Domain;
using HomeTask1.Projects.Domain.Entities;
using MongoDB.Driver;

namespace HomeTask1.Projects.Infrastructure;

public class ProjectRepository : IProjectRepository
{
    private readonly ProjectsDbContext _context;
    
    private readonly IMongoCollection<Project> _projects;

    public ProjectRepository(ProjectsDbContext dbContext)
    {
        _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _projects = dbContext.Projects ?? throw new ArgumentNullException(nameof(dbContext.Projects));
    }
    
    public async Task<List<Project>> GetProjectsByUserIdsAsync(List<int> userIds)
    {
        if (userIds == null || !userIds.Any())
        {
            return new List<Project>();
        }

        return await _context.Projects
            .Find(p => userIds.Contains(p.UserId))
            .ToListAsync();
    }


    public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int userId)
    {
        return await _projects
            .Find(p => p.UserId == userId)
            .ToListAsync();

    }

    public async Task<Project> GetProjectByIdAsync(string id)
    {
        return await _projects.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Project>> GetAllProjectsAsync()
    {
        return await _projects.Find(_ => true).ToListAsync();
    }

    public async Task CreateProjectAsync(Project project)
    {
        await _projects.InsertOneAsync(project);
    }

    public async Task<bool> UpdateProjectAsync(string id, Project updatedProject)
    {
        var result = await _projects.ReplaceOneAsync(
            p => p.Id == id,
            updatedProject);

        return result.MatchedCount > 0;
    }

    public async Task DeleteProjectAsync(string id)
    {
        await _projects.DeleteOneAsync(p => p.Id == id);
    }
}