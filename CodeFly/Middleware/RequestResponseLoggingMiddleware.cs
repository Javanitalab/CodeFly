using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace CodeFly.Middleware;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestResponseLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    private async Task<string> ReadRequestBody(HttpRequest request)
    {
        request.EnableBuffering();
        using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
        {
            string requestBody = await reader.ReadToEndAsync();
            request.Body.Position = 0;
            return requestBody;
        }
    }

    public async Task Invoke(HttpContext context)
    {
        string requestBody = await ReadRequestBody(context.Request);
        Log.Information($"Request: {context.Request.Method} {context.Request.Path} {requestBody}");

        // Capture the original response stream
        var originalBodyStream = context.Response.Body;

        try
        {
            // Create a new memory stream to intercept the response
            using (var responseBody = new MemoryStream())
            {
                // Set the response body to the new memory stream
                context.Response.Body = responseBody;

                // Continue processing the request
                await _next(context);

                // After the request has been processed, read the response body from the memory stream
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                string responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                // Log the request and response details
                Log.Information($"Response: {context.Response.StatusCode} Body: {responseBodyText}");

                // Copy the content from the response memory stream back to the original response stream
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
        finally
        {
            // Restore the original response body stream for other middleware and components
            context.Response.Body = originalBodyStream;
        }
    }
}
