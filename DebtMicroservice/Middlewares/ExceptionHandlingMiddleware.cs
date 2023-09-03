using System.Net;
using DebtMicroservice.Exceptions;
using DebtMicroservice.ViewModels;

namespace DebtMicroservice.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException e)
        {
            await HandleException(context, e);
        }
        catch (UnauthorizedAccessException e)
        {
            await HandleException(context, e);
        }
        catch (BadRequestException e)
        {
            await HandleException(context, e);
        }
        catch (Exception e)
        {
            await HandleException(context, e);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var errorResponse = new ErrorResponseDto();

        switch (exception)
        {
            case NotFoundException:
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.IsSuccess = false;
                errorResponse.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Message = exception.Message;
                break;
            }
            case UnauthorizedException:
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.IsSuccess = false;
                errorResponse.Message = exception.Message;
                break;
            }
            case BadRequestException:
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.IsSuccess = false;
                errorResponse.Message = exception.Message;
                break;
            }
            case not null:
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.IsSuccess = false;
                errorResponse.Message = exception.Message;
                break;
            }
        }

        await context.Response.WriteAsJsonAsync(errorResponse);
    }
}