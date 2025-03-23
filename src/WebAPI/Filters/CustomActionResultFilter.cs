using Application.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters
{
    //public class CustomActionResultFilter :Attribute, IActionFilter
    //{
    //    public void OnActionExecuted(ActionExecutedContext context)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void OnActionExecuting(ActionExecutingContext context)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class CustomActionResultFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            else
            {
                if (context.Result is ObjectResult obj)
                {
                    context.Result = new OkObjectResult(new CustomActionResult<object>
                    {
                        Success= true,
                        Result=obj.Value
                    });
                }
                else
                {
                    context.Result = new OkObjectResult(new CustomActionResult
                    {
                        Success = true,
                    });
                }
            }
            base.OnActionExecuted(context);
        }
    }
}
