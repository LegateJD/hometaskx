using HomeTask1.Projects.WebApi.Services;
using HomeTask1.Shared;
using Microsoft.AspNetCore.Mvc;

namespace HomeTask1.Projects.WebApi.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly ILogger<ProjectsController> _logger;

    public ProjectsController(IProjectService projectService, ILogger<ProjectsController> logger)
    {
        _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Retrieves all the projects available in the system.
    /// </summary>
    [HttpGet]
    public Task<IActionResult> GetAllProjects() =>
        RequestHandler.HandleQuery(() => _projectService.GetAllProjectsAsync(), _logger);

    /// <summary>
    /// Retrieves the project specified by the provided unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the project to retrieve.</param>
    [HttpGet("{id}")]
    public Task<IActionResult> GetProjectById(string id) => 
        RequestHandler.HandleQuery(() => _projectService.GetProjectByIdAsync(id), _logger);

    /// <summary>
    /// Retrieves a list of projects associated with a specific user by their ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose projects are to be retrieved.</param>
    [HttpGet("byuser/{userId}")]
    public Task<IActionResult> GetProjectsByUserId(int userId) => 
        RequestHandler.HandleQuery(() => _projectService.GetProjectsByUserIdAsync(userId), _logger);

    /// <summary>
    /// Creates a new project based on the provided details.
    /// </summary>
    /// <param name="request">The project creation request containing necessary details.</param>
    [HttpPost]
    public Task<IActionResult> AddProject([FromBody] Contracts.V1.CreateProject request) =>
        RequestHandler.HandleCommand(() => _projectService.CreateProjectAsync(request), _logger, ApiSuccessCode.Created);

    /// <summary>
    /// Updates a project with the specified ID using the provided update request data.
    /// </summary>
    /// <param name="id">The unique identifier of the project to be updated.</param>
    /// <param name="request">The data to update the project, containing its new properties.</param>
    [HttpPut("{id}")]
    public Task<IActionResult> UpdateProject(string id, [FromBody] Contracts.V1.UpdateProject request) =>
        RequestHandler.HandleCommand(() => _projectService.UpdateProjectAsync(id, request), _logger, ApiSuccessCode.Created);

    /// <summary>
    /// Deletes an existing project identified by the specified project ID.
    /// </summary>
    /// <param name="id">The unique identifier of the project to delete.</param>
    [HttpDelete("{id}")]
    public Task<IActionResult> DeleteProject(string id) =>
        RequestHandler.HandleCommand(() => _projectService.DeleteProjectAsync(id), _logger, ApiSuccessCode.NoContent);

    /// <summary>
    /// Retrieves a list of popular indicators based on the provided subscription type.
    /// </summary>
    /// <param name="subscriptionType">The type of subscription used to filter the popular indicators.</param>
    [HttpGet("popularIndicators/{subscriptionType}")]
    public Task<IActionResult> GetPopularIndicators(string subscriptionType) =>
        RequestHandler.HandleQuery(() => _projectService.GetPopularIndicatorsAsync(subscriptionType), _logger);
}
