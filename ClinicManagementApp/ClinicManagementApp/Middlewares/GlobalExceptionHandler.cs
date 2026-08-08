// ClinicManagementApp/Handlers/GlobalExceptionHandler.cs
using ClinicManagementApp.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementApp.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            // 1. Log the error
            _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

            // 2. Determine the status code based on the specific exception model
            var statusCode = exception switch
            {
                DoctorNotFoundException => StatusCodes.Status404NotFound,
                PatientNotFoundException => StatusCodes.Status404NotFound,
                AppointmentNotFoundException => StatusCodes.Status404NotFound,
                PatientRecordNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            httpContext.Response.StatusCode = statusCode;

            // 3. Return a clean error response
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = "An error occurred while processing your request.",
                Detail = exception.Message
            };

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true; // Indicates the exception has been handled
        }
    }
}