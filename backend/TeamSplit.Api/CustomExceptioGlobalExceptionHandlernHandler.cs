using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TeamSplit.Api;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception ex,
        CancellationToken cancellationToken)
    {
        logger.LogError("Error Message: {exceptionMessage}", ex.Message);

        int status = StatusCodes.Status500InternalServerError;

        if (ex is ArgumentException) status = StatusCodes.Status400BadRequest;

        var problemDetails = new ProblemDetails
        {
            Status = status,
            Title = ex.Message,
            Detail = ex.StackTrace
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            
        return true;
    }
}