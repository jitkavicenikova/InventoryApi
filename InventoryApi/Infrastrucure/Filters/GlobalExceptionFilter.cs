using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InventoryApi.Infrastrucure.Filters;

/// <summary>
/// A global exception filter that handles unhandled exceptions thrown by controllers
/// and converts them into appropriate HTTP responses.
/// </summary>
public class GlobalExceptionFilter : IExceptionFilter
{
    /// <summary>
    /// Called when an exception occurs during the execution of a controller action.
    /// Converts known exceptions into specific HTTP status codes and sets a JSON response.
    /// </summary>
    /// <param name="context">The exception context containing information about the exception and the current request.</param>

    public void OnException(ExceptionContext context)
    {
        int statusCode = context.Exception switch
        {
            KeyNotFoundException => StatusCodes.Status404NotFound,
            InvalidOperationException => StatusCodes.Status400BadRequest,
            ArgumentOutOfRangeException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Result = new ObjectResult(new
        {
            message = context.Exception.Message,
            status = statusCode
        })
        {
            StatusCode = statusCode
        };

        context.ExceptionHandled = true;
    }
}