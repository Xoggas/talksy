using System.Text;

namespace Talksy.Api.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Метод + путь
        _logger.LogInformation("Request: {method} {url}", context.Request.Method, context.Request.Path);

        // Тело (нужно включить EnableBuffering, иначе тело будет "съедено")
        context.Request.EnableBuffering();
        if (context.Request.ContentLength > 0 && context.Request.Body.CanRead)
        {
            context.Request.Body.Position = 0;
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0; // сбросить позицию, чтобы пайплайн мог снова читать тело
            _logger.LogInformation("Body: {body}", body);
        }

        await _next(context); // передаем дальше
    }
}