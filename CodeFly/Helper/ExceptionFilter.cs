using System.Net;
using CodeFly.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CodeFly.Helper;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        // Set the response status code
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        // Create an error response object
        var errorResponse = new Result<object>
        {
            Data = (object)null,
            Error = context.Exception.Message+"\n"+context.Exception.StackTrace,
            Status = 500
        };

        // Write the error response to the response body
        context.Result = new JsonResult(errorResponse);
        context.ExceptionHandled = true;
    }
}