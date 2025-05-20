using Application.DTOs.Error;
using Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace Presentation.Middleware
{

    public class ErrorMiddleware(RequestDelegate _next, ILogger<ErrorMiddleware> _logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro em ErrorMiddleware");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            int statusCode;
            object response;


            switch (exception)
            {
                case NotFoundException notFoundEx:
                    statusCode = (int)HttpStatusCode.NotFound;
                    response = new ErrorDTO { Error = notFoundEx.Message };
                    break;

                case BusinessException businessEx:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    response = new ErrorDTO { Error = businessEx.Message };
                    break;

                case UnauthorizedAccessException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    response = new ErrorDTO { Error = "Acesso não autorizado" };
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    response = new ErrorDTO
                    {
                        Error = "Erro interno no servidor",
                        Details = "Tente novamente ou entre em contato com o suporte"
                    };
                    break;
            }

            var jsonResponse = JsonSerializer.Serialize(response);

            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(jsonResponse);
        }
    }

}
