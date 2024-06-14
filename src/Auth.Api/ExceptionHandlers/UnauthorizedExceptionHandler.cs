using Auth.Api.ExceptionHandlers.Interfaces;
using Auth.Api.ExceptionHandlers.Responses;
using Auth.Application.Exceptions;

namespace Auth.Api.ExceptionHandlers;

public class UnauthorizedExceptionHandler : IExceptionHandler
{
    public ExceptionResponse HandleException(Exception ex, string traceId)
    {
        return new ExceptionResponse
        {
            Type = "Unauthorized",
            Title = "User unauthorized",
            Status = StatusCodes.Status401Unauthorized,
            TraceId = traceId
        };
    }
}