using CampusTradingPlatform.Attributes;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CampusTradingPlatform.Filters
{
    public class UnitOfWorkFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = await next();
            if (result.Exception != null)
            {
                return;
            }
            var controllerActionDes = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDes == null)
            {
                return;
            }
            var unitAttr = controllerActionDes.MethodInfo.GetCustomAttribute<UnitOfWorkAttribute>();
            if (unitAttr == null)
            {
                return;
            }
            foreach (var dbContextType in unitAttr.DbContextType)
            {
                var dbContext = context.HttpContext.RequestServices.GetService(dbContextType) as DbContext;
                if (dbContext != null)
                {
                    await dbContext.SaveChangesAsync();
                }
            }

        }
    }
}
