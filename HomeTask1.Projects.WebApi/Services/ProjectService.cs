using CSharpFunctionalExtensions;
using HomeTask1.Projects.Domain;
using HomeTask1.Projects.Domain.Entities;
using HomeTask1.Projects.WebApi.Models;
using HomeTask1.Shared;
using MongoDB.Bson;

namespace HomeTask1.Projects.WebApi.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserServiceClient _userServiceClient;

    public ProjectService(IProjectRepository projectRepository, IUserServiceClient userServiceClient)
    {
        _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
        _userServiceClient = userServiceClient ?? throw new ArgumentNullException(nameof(userServiceClient));
    }
    
    public async Task<Result<IEnumerable<Project>, ApiError>> GetAllProjectsAsync()
    {
        var projects = await _projectRepository.GetAllProjectsAsync();

        if (projects == null)
        {
            return Result.Success<IEnumerable<Project>, ApiError>(new List<Project>());
        }

        return Result.Success<IEnumerable<Project>, ApiError>(projects);
    }
    
    public async Task<Result<Project, ApiError>> GetProjectByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            return Result.Failure<Project, ApiError>(
                new ApiError(ApiErrorCode.BadRequest, $"Invalid project ID: {id}")
            );
        }
        
        var project = await _projectRepository.GetProjectByIdAsync(id);

        if (project == null)
        {
            return Result.Failure<Project, ApiError>(
                new ApiError(ApiErrorCode.NotFound, $"Project with ID {id} not found.")
            );
        }

        return Result.Success<Project, ApiError>(project);
    }

    public async Task<Result<IEnumerable<Project>, ApiError>> GetProjectsByUserIdAsync(int userId)
    {
        var projects = await _projectRepository.GetProjectsByUserIdAsync(userId);

        if (projects == null)
        {
            return Result.Success<IEnumerable<Project>, ApiError>(new List<Project>());
        }

        return Result.Success<IEnumerable<Project>, ApiError>(projects);
    }

    public async Task<Result<Project, ApiError>> CreateProjectAsync(Contracts.V1.CreateProject request)
    {
        var userExists = await _userServiceClient.UserExistsAsync(request.UserId);
            
        if (!userExists)
        {
            return Result.Failure<Project, ApiError>(
                new ApiError(ApiErrorCode.NotFound, $"User with ID {request.UserId} does not exist.")
            );
        }

        var project = new Project
        {
            UserId = request.UserId,
            Name = request.Name,
            Charts = request.Charts.Select(chartRequest => new Chart
            {
                Symbol = chartRequest.Symbol,
                Timeframe = chartRequest.Timeframe,
                Indicators = chartRequest.Indicators.Select(indicatorRequest => new Indicator
                {
                    Name = indicatorRequest.Name,
                    Parameters = indicatorRequest.Parameters
                }).ToList()
            }).ToList()
        };

        await _projectRepository.CreateProjectAsync(project);

        return Result.Success<Project, ApiError>(project);
    }
    
    public async Task<Result<Project, ApiError>> UpdateProjectAsync(string id, Contracts.V1.UpdateProject request)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            return Result.Failure<Project, ApiError>(
                new ApiError(ApiErrorCode.BadRequest, $"Invalid project ID: {id}")
            );
        }
            
        var userExists = await _userServiceClient.UserExistsAsync(request.UserId);
            
        if (!userExists)
        {
            return Result.Failure<Project, ApiError>(
                new ApiError(ApiErrorCode.NotFound, $"User with ID {request.UserId} does not exist.")
            );
        }

        var updatedProject = new Project
        {
            Id = id,
            UserId = request.UserId,
            Name = request.Name,
            Charts = request.Charts.Select(chart => new Chart
            {
                Symbol = chart.Symbol,
                Timeframe = chart.Timeframe,
                Indicators = chart.Indicators.Select(indicator => new Indicator
                {
                    Name = indicator.Name,
                    Parameters = indicator.Parameters
                }).ToList()
            }).ToList()
        };

        var result = await _projectRepository.UpdateProjectAsync(id, updatedProject);
            
        if (!result)
        {
            await _projectRepository.CreateProjectAsync(updatedProject);
        }

        return Result.Success<Project, ApiError>(updatedProject);
    }

    public async Task<Result<bool, ApiError>> DeleteProjectAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            return Result.Failure<bool, ApiError>(
                new ApiError(ApiErrorCode.BadRequest, $"Invalid project ID: {id}")
            );
        }

        var existingProject = await _projectRepository.GetProjectByIdAsync(id);
        
        if (existingProject == null)
        {
            return Result.Success<bool, ApiError>(true);
        }

        await _projectRepository.DeleteProjectAsync(id);

        return Result.Success<bool, ApiError>(true);
    }
    
    public async Task<Result<List<PopularIndicator>, ApiError>> GetPopularIndicatorsAsync(string subscriptionType)
    {
        var users = await _userServiceClient.GetUsersBySubscriptionTypeAsync(subscriptionType);
            
        if (users == null || !users.Any())
        {
            return Result.Failure<List<PopularIndicator>, ApiError>(
                new ApiError(ApiErrorCode.NotFound, $"No users found with subscription type '{subscriptionType}'.")
            );
        }

        var userIds = users.Select(u => u.Id).ToList();

        var projectsResult = await _projectRepository.GetProjectsByUserIdsAsync(userIds);
            
        if (projectsResult == null || !projectsResult.Any())
        {
            return Result.Failure<List<PopularIndicator>, ApiError>(
                new ApiError(ApiErrorCode.NotFound, $"No projects found for users with subscription type '{subscriptionType}'.")
            );
        }

        var indicatorUsage = projectsResult
            .SelectMany(p => p.Charts)
            .SelectMany(c => c.Indicators)
            .GroupBy(i => i.Name)
            .Select(group => new PopularIndicator
            {
                Name = group.Key,
                Used = group.Count()
            })
            .OrderByDescending(pi => pi.Used)
            .Take(3)
            .ToList();

        return Result.Success<List<PopularIndicator>, ApiError>(indicatorUsage);
    }
}