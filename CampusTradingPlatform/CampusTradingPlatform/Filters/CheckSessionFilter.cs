using CampusTradingPlatform.Attributes;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace CampusTradingPlatform.Filters
{
    public class CheckSessionFilter : IAsyncActionFilter
    {
        async Task IAsyncActionFilter.OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controllerActionDesc = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDesc == null)
            {
                return;
            }
            var noCheckSession = controllerActionDesc.MethodInfo.GetCustomAttribute<NoCheckSessionAttribute>();
            if (noCheckSession == null)
            {
                if (string.IsNullOrEmpty(context.HttpContext.Session.GetString("user")))
                {
                    // 跳转到登录页面。
                    context.HttpContext.Response.Redirect("/login/index?returnUrl="+context.HttpContext.Request.Path.ToString());
                    return;
                }

                await next();
            }
            else
            {
                // 找到了
                await next();
            }

        }
    }
}
