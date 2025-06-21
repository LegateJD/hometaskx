using CSharpFunctionalExtensions;
using HomeTask1.Projects.Domain.Entities;
using HomeTask1.Projects.WebApi.Models;
using HomeTask1.Shared;

namespace HomeTask1.Projects.WebApi.Services;

public interface IProjectService
{
    Task<Result<IEnumerable<Project>, ApiError>> GetAllProjectsAsync();
    
    Task<Result<Project, ApiError>> GetProjectByIdAsync(string id);
    
    Task<Result<IEnumerable<Project>, ApiError>> GetProjectsByUserIdAsync(int userId);
    
    Task<Result<List<PopularIndicator>, ApiError>> GetPopularIndicatorsAsync(string subscriptionType);
    
    Task<Result<Project, ApiError>> CreateProjectAsync(Contracts.V1.CreateProject request);
    
    Task<Result<Project, ApiError>> UpdateProjectAsync(string id, Contracts.V1.UpdateProject request);
    
    Task<Result<bool, ApiError>> DeleteProjectAsync(string id);
}