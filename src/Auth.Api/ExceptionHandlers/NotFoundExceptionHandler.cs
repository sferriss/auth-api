using Auth.Api.ExceptionHandlers.Interfaces;
using Auth.Api.ExceptionHandlers.Responses;
using Auth.Application.Exceptions;

namespace Auth.Api.ExceptionHandlers;

public class NotFoundExceptionHandler : IExceptionHandler
{
    public ExceptionResponse HandleException(Exception ex, string traceId)
    {
        var exception = ex as NotFoundException;
            
        return new ExceptionResponse
        {
            Type = "Resource not found",
            Title = exception?.Message,
            Status = StatusCodes.Status404NotFound,
            TraceId = traceId
        };
    }
}