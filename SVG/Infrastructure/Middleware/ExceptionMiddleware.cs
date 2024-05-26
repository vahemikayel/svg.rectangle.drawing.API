using FluentValidation;
using SVG.API.Models.Response.Error;
using System.Net;
using System.Text.Json;

namespace SVG.API.Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                ErrorResponseModel response;
                var responseJson = string.Empty;
                context.Response.ContentType = "application/json";
                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                if (ex is ValidationException validationException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    var errors = validationException?.Errors?.Select(x => x.ErrorMessage)?.ToList();
                    var clientMessage = errors?.FirstOrDefault();
                    response = new ApiValidationErrorResponse()
                    {
                        Message = clientMessage,
                        Errors = errors,
                        StatusCode = (int)HttpStatusCode.BadRequest,
                    };
                    responseJson = JsonSerializer.Serialize(response, jsonOptions);
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response = new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString());
                    responseJson = JsonSerializer.Serialize(response, jsonOptions);
                }

                await context.Response.WriteAsync(responseJson);
            }
        }
    }
}
