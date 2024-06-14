using Auth.Api.ExceptionHandlers.Responses;

namespace Auth.Api.ExceptionHandlers.Interfaces;

public interface IExceptionHandler
{
    ExceptionResponse HandleException(Exception ex, string traceId);
}