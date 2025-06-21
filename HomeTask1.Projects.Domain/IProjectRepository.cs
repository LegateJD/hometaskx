using HomeTask1.Projects.Domain.Entities;

namespace HomeTask1.Projects.Domain;

public interface IProjectRepository
{
    Task<Project> GetProjectByIdAsync(string id);
    
    Task<List<Project>> GetProjectsByUserIdsAsync(List<int> userIds);

    Task<List<Project>> GetAllProjectsAsync();
    
    Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int userId);

    Task CreateProjectAsync(Project project);

    Task<bool> UpdateProjectAsync(string id, Project updatedProject);

    Task DeleteProjectAsync(string id);

}