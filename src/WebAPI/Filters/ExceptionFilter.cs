using Application.Exceptions;
using Application.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var type = context.Exception.GetType();
            if (type == typeof(NotFoundException))
            {
                var exception = (NotFoundException)context.Exception;
                context.Result = new NotFoundObjectResult(new CustomActionResult
                {
                    Success = false,
                    Message = exception.Message
                });
                context.ExceptionHandled = true;
            }
            else if (type == typeof(CustomValidationException))
            {
                var exception = (CustomValidationException)context.Exception;
                context.Result = new BadRequestObjectResult(new CustomActionResult
                {
                    Success = false,
                    Errors = exception.Errors
                });
                context.ExceptionHandled = true;
            }
            else if (type == typeof(CustomException))
            {
                var exception = (CustomException)context.Exception;
                context.Result = new BadRequestObjectResult(new CustomActionResult
                {
                    Success = false,
                    Message = exception.Message
                });
                context.ExceptionHandled = true;
            }
            else
            {
                context.Result = new BadRequestObjectResult(new CustomActionResult
                {
                    Success = false,
                    Message = "unhandled error occured in server"
                });
            }
            base.OnException(context);
        }
    }
}
