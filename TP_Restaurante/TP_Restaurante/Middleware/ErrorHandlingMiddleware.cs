using Application.Models.Response;
using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TP_Restaurante.Exceptions;

namespace TP_Restaurante.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        //Tomorrow search if this is okay -REMINDER
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Continue processing the request
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception 
                _logger.LogError(ex, $"Catch Error: {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }

        }
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            //This method select the server response and wrapp with the ApiError

            context.Response.ContentType = "application/json";

            HttpStatusCode statusCode;
            string message = ex.Message;

            switch (ex)
            {
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case ConflictException:
                    statusCode = HttpStatusCode.Conflict;
                    break;
                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case JsonException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "El formato JSON de la solicitud es inválido. Asegúrate de que los valores booleanos sean 'true' o 'false' sin comillas si no son strings.";
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "Public Server Error";
                    break;
            }
            context.Response.StatusCode = (int)statusCode;

            var error = new ApiError(message);
            var result = JsonSerializer.Serialize(error);

            await context.Response.WriteAsync(result);
        }
    }
}
