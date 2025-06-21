using HomeTask1.Shared;
using HomeTask1.Users.WebApi;
using HomeTask1.Users.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/subscriptions")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly ILogger<SubscriptionsController> _logger;

    public SubscriptionsController(ISubscriptionService subscriptionService, ILogger<SubscriptionsController> logger)
    {
        _subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Retrieves all subscriptions.
    /// </summary>
    [HttpGet]
    public Task<IActionResult> GetAllSubscriptions() => 
        RequestHandler.HandleQuery(() => _subscriptionService.GetAllSubscriptionsAsync(), _logger);

    /// <summary>
    /// Retrieves a subscription by the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the subscription to retrieve.</param>
    [HttpGet("{id}")]
    public Task<IActionResult> GetSubscriptionById(int id) => 
        RequestHandler.HandleQuery(() => _subscriptionService.GetSubscriptionByIdAsync(id), _logger);

    /// <summary>
    /// Adds a new subscription to the system.
    /// </summary>
    /// <param name="request">An object containing the details of the subscription to be created.</param>
    [HttpPost]
    public Task<IActionResult> AddSubscription([FromBody] Contracts.V1.CreateSubscription request) =>
        RequestHandler.HandleCommand(() => _subscriptionService.AddSubscriptionAsync(request), _logger,
            ApiSuccessCode.Created);

    /// <summary>
    /// Updates an existing subscription with the provided details.
    /// </summary>
    /// <param name="id">The unique identifier of the subscription to be updated.</param>
    /// <param name="request">The request object containing updated subscription details.</param>
    [HttpPut("{id}")]
    public Task<IActionResult> UpdateSubscription(int id, [FromBody] Contracts.V1.UpdateSubscription request) =>
        RequestHandler.HandleCommand(() => _subscriptionService.UpdateSubscriptionAsync(id, request), _logger,
            ApiSuccessCode.Created);

    /// <summary>
    /// Deletes the subscription associated with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the subscription to delete.</param>
    [HttpDelete("{id}")]
    public Task<IActionResult> DeleteSubscription(int id) =>
        RequestHandler.HandleCommand(() => _subscriptionService.DeleteSubscriptionAsync(id), _logger,
            ApiSuccessCode.NoContent);
}