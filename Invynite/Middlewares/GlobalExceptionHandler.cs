using Invynite.Middlewares.Exceptions;
using Invynite.Services.Procurement.Exceptions;
using Invynite.Services.Productions.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace Invynite.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An exception occured: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Title = "An error occured",
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception.Message
        };

        switch (exception)
        {
            case BadRequestException:
                problemDetails.Status = StatusCodes.Status400BadRequest;
                httpContext.Response.StatusCode = problemDetails.Status.Value; break;

            case NotFoundException:
                problemDetails.Status = StatusCodes.Status404NotFound;
                httpContext.Response.StatusCode = problemDetails.Status.Value; break;

            case InsufficientStockException:
                problemDetails.Status = StatusCodes.Status409Conflict;
                httpContext.Response.StatusCode = problemDetails.Status.Value; break;

            case PurchaseOrderNotPendingException:
                problemDetails.Status = StatusCodes.Status409Conflict;
                httpContext.Response.StatusCode = problemDetails.Status.Value; break;

            default:
                problemDetails.Title = "An unexpected error occured";
                problemDetails.Detail = null;
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError; break;
        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
