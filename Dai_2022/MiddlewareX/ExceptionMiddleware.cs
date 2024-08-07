using System.Net;
using Domain.Exceptions;

namespace Dai;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        CostumValidationProblemDetails problem = new CostumValidationProblemDetails();

        switch (ex)
        {
            case InvalidUserCredentialsException invalidUserCredentialsException:
                statusCode = HttpStatusCode.BadRequest;
                problem = new CostumValidationProblemDetails
                {
                    Title = invalidUserCredentialsException.Message,
                    Status = (int)statusCode,
                    Detail = invalidUserCredentialsException.InnerException?.Message,
                    Type = nameof(InvalidUserCredentialsException)
                };
                break;
            
            case UserAlreadyExistsException userAlreadyExistsException:
                statusCode = HttpStatusCode.BadRequest;
                problem = new CostumValidationProblemDetails
                {
                    Title = userAlreadyExistsException.Message,
                    Status = (int)statusCode,
                    Detail = userAlreadyExistsException.InnerException?.Message,
                    Type = nameof(UserAlreadyExistsException)
                };
                break;
            case ContaxtYourSapportException contaxtYourSapportException:
                statusCode = HttpStatusCode.BadRequest;
                problem = new CostumValidationProblemDetails
                {
                    Title = contaxtYourSapportException.Message,
                    Status = (int)statusCode,
                    Detail = contaxtYourSapportException.InnerException?.Message,
                    Type = nameof(ContaxtYourSapportException)
                };
                break;
            case UserNotAvailableException userNotAvailableException:
                statusCode = HttpStatusCode.BadRequest;
                problem = new CostumValidationProblemDetails
                {
                    Title = userNotAvailableException.Message,
                    Status = (int)statusCode,
                    Detail = userNotAvailableException.InnerException?.Message,
                    Type = nameof(UserNotAvailableException)
                };
                break;
            case InvalidOperationException invalidOperationException:
                statusCode = HttpStatusCode.BadRequest;
                problem = new CostumValidationProblemDetails
                {
                    Title = invalidOperationException.Message,
                    Status = (int)statusCode,
                    Detail = invalidOperationException.InnerException?.Message,
                    Type = nameof(InvalidOperationException)
                };
                break;
            default: 
                problem = new CostumValidationProblemDetails
                {
                    Title = ex.Message,
                    Status = (int)statusCode,
                    Type = nameof(HttpStatusCode.InternalServerError),
                    Detail = ex.StackTrace
                };
                break;
        }

        httpContext.Response.StatusCode = (int)statusCode;
        await httpContext.Response.WriteAsJsonAsync(problem);
    }
}