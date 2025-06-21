using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HomeTask1.Shared;

public enum ApiSuccessCode
{
    Ok = 200,
    Created = 201,
    NoContent = 204
}

public enum ApiErrorCode
{
    BadRequest = 400,
    NotFound = 404,
    Conflict = 409,
    InternalServerError = 500
}

public class ApiError
{
    public string ErrorMessage { get; set; }
    public ApiErrorCode ErrorCode { get; set; }
    
    public ApiError(ApiErrorCode errorCode, string errorMessage)
    {
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }
}

public static class RequestHandler
{
    public static async Task<IActionResult> HandleCommand<S>(
        Func<Task<Result<S, ApiError>>> handler, ILogger log, ApiSuccessCode apiSuccessCode = ApiSuccessCode.Ok)
    {
        try
        {
            log.LogDebug("Handling HTTP request");
            var result = await handler();

            if (!result.IsFailure)
                return apiSuccessCode switch
                {
                    ApiSuccessCode.Created => new CreatedResult("", result.Value),
                    ApiSuccessCode.NoContent => new NoContentResult(),
                    _ => new OkResult()
                };
            
            var error = result.Error;

            return error.ErrorCode switch
            {
                ApiErrorCode.Conflict => new ConflictObjectResult(error.ErrorMessage),
                ApiErrorCode.NotFound => new NotFoundObjectResult(error.ErrorMessage),
                ApiErrorCode.InternalServerError => new BadRequestObjectResult(error.ErrorMessage) { StatusCode = 500},
                _ => new BadRequestObjectResult(error.ErrorMessage)
            };

        }
        catch (Exception e)
        {
            log.LogDebug(e, "Error handling the command");
            var result = new BadRequestObjectResult(new
            {
                error = e.Message, stackTrace = e.StackTrace
            })
            {
                StatusCode = 500
            };

            return result;
        }
    }
        
    public static async Task<IActionResult> HandleQuery<T>(
        Func<Task<Result<T, ApiError>>> query, ILogger log)
    {
        try
        {
            var result = await query();
            
            if (!result.IsFailure) return new OkObjectResult(result.Value);
            
            var error = result.Error;
            
            return error.ErrorCode switch
            {
                ApiErrorCode.Conflict => new ConflictObjectResult(error.ErrorMessage),
                _ => new BadRequestObjectResult(error)
            };
        }
        catch (Exception e)
        {
            log.LogDebug(e, "Error handling the query");
            var result = new BadRequestObjectResult(new
            {
                error = e.Message, stackTrace = e.StackTrace
            })
            {
                StatusCode = 500
            };
            
            return result;
        }
    }
}