using Serilog.Context;

namespace StudentWebAPI.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string Headerkey = "X-Correlation-ID";

        public CorrelationIdMiddleware(RequestDelegate next)
        {
           _next = next; 
        }
        public async Task Invoke(HttpContext context)
        {
           var correlationId=context.Request.Headers[Headerkey].FirstOrDefault();

            if (string.IsNullOrEmpty(correlationId))
            {
                correlationId=Guid.NewGuid().ToString();
            }
            Console.WriteLine($"CorrelationId: {correlationId}");
            context.Items["CorrelationId"] = correlationId;
            context.Response.Headers[Headerkey] = correlationId;
            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);   
            }
        }
    }
}
