using StudentWebAPI.Exceptions;
using System.Text.Json;

namespace StudentWebAPI.Middlewares
{
    public class ExceptionMiddleware
    {
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
            context.Response.ContentType = "application/json";

            // 🔥 Step 1: Determine status code
            var statusCode = ex switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = statusCode;

            // 🔥 Step 2: Build response
            var response = new
            {
                success = false,
                message = statusCode == StatusCodes.Status500InternalServerError
                            ? "An unexpected error occurred"
                            : ex.Message,
                traceId = context.TraceIdentifier
            };

            // 🔥 Step 3: Convert to JSON
            var json = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(json);
        }
    }
}