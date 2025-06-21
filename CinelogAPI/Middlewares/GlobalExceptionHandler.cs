using Service.Helpers.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CinelogAPI.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            var title = "Internal Server Error";
            var detail = _env.IsDevelopment() ? exception.ToString() : "An unexpected error occurred.";

            switch (exception)
            {
                case ArgumentNullException:
                case ArgumentException:
                    statusCode = StatusCodes.Status400BadRequest;
                    title = "Bad Request";
                    detail = exception.Message;
                    break;

                case UnauthorizedAccessException:
                    statusCode = StatusCodes.Status401Unauthorized;
                    title = "Unauthorized";
                    detail = exception.Message;
                    break;

                case KeyNotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    title = "Not Found";
                    detail = exception.Message;
                    break;

                case InvalidOperationException:
                    statusCode = StatusCodes.Status409Conflict;
                    title = "Conflict";
                    detail = exception.Message;
                    break;

                case InvalidCommentException:
                    statusCode = StatusCodes.Status400BadRequest;
                    title = "Invalid Comment";
                    detail = exception.Message;
                    break;

                case InvalidFavoriteException:
                    statusCode = StatusCodes.Status400BadRequest;
                    title = "Invalid Comment";
                    detail = exception.Message;
                    break;


                    // Add your custom exceptions here
            }

            _logger.LogError(exception, "Exception occurred");

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }



    }
}
