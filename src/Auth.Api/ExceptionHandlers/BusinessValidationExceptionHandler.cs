using Auth.Api.ExceptionHandlers.Interfaces;
using Auth.Api.ExceptionHandlers.Responses;
using Auth.Application.Exceptions;

namespace Auth.Api.ExceptionHandlers;

public class BusinessValidationExceptionHandler : IExceptionHandler
{
    public ExceptionResponse HandleException(Exception ex, string traceId)
    {
        var exception = ex as BusinessValidationException;
            
        return new ExceptionResponse
        {
            Type = "Business rule validation",
            Title = exception?.Message,
            Status = StatusCodes.Status400BadRequest,
            TraceId = traceId
        };
    }
}