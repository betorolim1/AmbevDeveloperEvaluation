using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.WebApi.Common;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware
{
    public class BusinessExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public BusinessExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BusinessException ex)
            {
                await HandleBusinessExceptionAsync(context, ex);

            }
        }

        private static Task HandleBusinessExceptionAsync(HttpContext context, BusinessException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var response = new ApiResponse
            {
                Success = false,
                Message = exception.Message,
                Errors = exception.Errors != null
                    ? exception.Errors.Select(e => new ValidationErrorDetail { Error = e })
                    : new List<ValidationErrorDetail>()
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
        }
    }
}
