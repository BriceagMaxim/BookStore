using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using BookStore.API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookStore.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, IncludeFields = true };
                if (_env.IsDevelopment())
                {
                    var response = new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString());
                    var json = JsonSerializer.Serialize(response, options);
                    await context.Response.WriteAsync(json);
                }
                else
                {
                    var response = new ApiResponse((int)HttpStatusCode.InternalServerError);
                    var json = JsonSerializer.Serialize(response, options);
                    await context.Response.WriteAsync(json);
                }
            }
        }
    }
}