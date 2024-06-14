using System.Net.Mime;
using System.Text.Json;
using Auth.Api.ExceptionHandlers.Factories;
using Auth.Api.ExceptionHandlers.Responses;

namespace Auth.Api.ExceptionHandlers.Middlewares;

public class ExceptionMiddleware
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
    
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {

        var responseExceptionResult = GetResponseExceptionOrDefault(context, ex);

        var responseJson = JsonSerializer.Serialize(responseExceptionResult, SerializerOptions);

        context.Response.StatusCode = responseExceptionResult.Status;
        context.Response.ContentType = MediaTypeNames.Application.Json;
        return context.Response.WriteAsync(responseJson);
    }
    
    private static ExceptionResponse GetResponseExceptionOrDefault(HttpContext context, Exception ex)
    {
        ExceptionResponse? responseExceptionResult = null;
        var exceptionHandler = context.RequestServices.GetService<ExceptionHandlerFactory>();

        if (exceptionHandler == null)
        {
            return responseExceptionResult ?? new ExceptionResponse()
            {
                Type = "Erro interno.",
                Title = "Ocorreu um erro interno.",
                Status = StatusCodes.Status500InternalServerError,
                TraceId = context.TraceIdentifier
            };
        }
        
        var handler = exceptionHandler.GetExceptionHandlerBy(context.RequestServices, ex.GetType()) ?? 
                      exceptionHandler.GetExceptionHandlerBy(context.RequestServices, typeof(Exception));

        if (handler != null)
        {
            responseExceptionResult = handler.HandleException(ex, context.TraceIdentifier);
        }

        return responseExceptionResult ?? new ExceptionResponse
        {
            Type = "Internal error.",
            Title = "An internal error has occurred",
            Status = StatusCodes.Status500InternalServerError,
            TraceId = context.TraceIdentifier
        };
    }
}