using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Event_planning_back.Core.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Event_planning_back.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly bool _isEnabled;
        private readonly string _logFilePath;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _isEnabled = configuration.GetValue<bool>("LoggingMiddleware:IsEnabled");

            var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            var logFileName = $"Log_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            _logFilePath = Path.Combine(logDirectory, logFileName);
        }

        public async Task InvokeAsync(HttpContext context, IJwtProvider jwtProvider)
        {
            if (_isEnabled)
            {
                var token = context.Request.Cookies?["AuthToken"];

                var userId = Guid.Empty;
                var userName = "Anonymous";
                if (token != null)
                {
                    userId = jwtProvider.GetUserId(token);
                    userName = jwtProvider.GetUserName(token);
                }


                var method = context.Request.Method;
                var path = context.Request.Path;
                var startTime = DateTime.UtcNow;

                var controllerActionDescriptor = context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();
                var controllerName = controllerActionDescriptor?.ControllerTypeInfo.FullName;
                var actionName = controllerActionDescriptor?.ActionName;

                var log = new StringBuilder();
                log.AppendLine("----- Request Information -----");
                log.AppendLine($"User id: {userId}");
                log.AppendLine($"Username:{userName}");
                log.AppendLine($"Controller: {controllerName}");
                log.AppendLine($"Action: {actionName}");
                log.AppendLine($"Method: {method}");
                log.AppendLine($"Path: {path}");
                log.AppendLine($"Start Time: {startTime:O}");

                await _next(context);

                var endTime = DateTime.UtcNow;
                var duration = endTime - startTime;
                var durationInMilliseconds = duration.TotalMilliseconds;

                log.AppendLine($"End Time: {endTime:O}");
                log.AppendLine($"Duration: {durationInMilliseconds} ms");
                log.AppendLine("------------------------------");

                await File.AppendAllTextAsync(_logFilePath, log.ToString());
            }
            else
            {
                await _next(context);
            }
        }
    }
}
