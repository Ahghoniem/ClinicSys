using LibraryMAnagementSystem.API.Errors;
using LibraryManagementSystem.BLL.Exceptions;
using System.Net;

namespace LibraryMAnagementSystem.API.MiddleWares
{

    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger<CustomExceptionHandlerMiddleware> logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next,
            IWebHostEnvironment webHostEnvironment,
            ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            this.next = next;
            this.webHostEnvironment = webHostEnvironment;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);

            }
            catch (Exception ex)
            {

                #region Logging
                if (webHostEnvironment.IsDevelopment())
                {
                    logger.LogError(ex, ex.Message);

                }
                else
                {
                }
                #endregion
                await HandleExceptionAsync(httpContext, ex);

            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            ApiResponse response;
            switch (ex)
            {
                case NotFoundExceptions:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    httpContext.Response.ContentType = "application/json";
                    response = new ApiResponse(404, ex.Message);
                    await httpContext.Response.WriteAsync(response.ToString());


                    break;
                case ValidationException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    httpContext.Response.ContentType = "application/json";
                    response = new ApiResponse(401, ex.Message);
                    await httpContext.Response.WriteAsync(response.ToString());
                    break;
                case BadRequestExceptions:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    httpContext.Response.ContentType = "application/json";
                    response = new ApiResponse(400, ex.Message);
                    await httpContext.Response.WriteAsync(response.ToString());
                    break;

                case UnAuthorizedExceptionv:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    httpContext.Response.ContentType = "application/json";
                    response = new ApiResponse(401, ex.Message);
                    await httpContext.Response.WriteAsync(response.ToString());
                    break;

                default:

                    response = webHostEnvironment.IsDevelopment() ?
                        new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                        : response = new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);


                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsync(response.ToString());
                    break;

            }
        }
    }

}
