using Make_a_Drop.Application.Exceptions;
using Make_a_Drop.Core.Exceptions;

namespace Make_a_Drop.MVC.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private Task HandleException(HttpContext context, Exception ex)
        {
            _logger.LogError(ex.Message);

            var code = StatusCodes.Status500InternalServerError;
            var errors = new List<string> { ex.Message };
            var errorMessage = ex.Message;

            code = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                ResourceNotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                UnprocessableRequestException => StatusCodes.Status422UnprocessableEntity,
                BadHttpRequestException => StatusCodes.Status401Unauthorized,                              
                _ => code
            };

            context.Response.ContentType = "text/html";
            context.Response.StatusCode = code;

            return context.Response.WriteAsync($"<h1><b>HTTP ERROR {code}</b></h1><h3>{errorMessage}</h3>");
        }
        

    }
}

