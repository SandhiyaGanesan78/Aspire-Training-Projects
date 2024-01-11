using CombineSample.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace CombineSample.Data
{
    public class LoggingActionFilter : IActionFilter
{
    private readonly ILogger _logger;

    public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var Controller = context.RouteData.Values["controller"];
        var action = context.RouteData.Values["action"];

        _logger.LogInformation("Executing {0} action in {1} controller", action, Controller);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var Controller = context.RouteData.Values["controller"];
        var action = context.RouteData.Values["action"];

        _logger.LogInformation("Executed {0} action in {1} controller", action, Controller);
    }
    
        }
    }


        public class AuthorizeAttributeEmployee : TypeFilterAttribute  //TypeFilterAttribute will allow AuthorizeAttributeEmployee as attribute in controller or in action mentod
{
   
    public AuthorizeAttributeEmployee() : base(typeof(CustomAuthorizeFilterEmployee))
    {
    }
}
public class CustomAuthorizeFilterEmployee : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext filterContext)
    {
        var role = filterContext.HttpContext.Session.GetObjectFromJson<Employee>("users");
        if (role == null)
        {
            Console.WriteLine("Error: users is null");
            // result is property of AuthorizationFilterContext
            filterContext.Result = new RedirectToActionResult("EmployeeLogin", "Employee", null);
            return;
        }
        Console.WriteLine($"users: {JsonConvert.SerializeObject(role)}");
    }
}

public class AuthorizeAttributeManager : TypeFilterAttribute //TypeFilterAttribute will allow AuthorizeAttributeEmployee as attribute in controller or in action mentod
{
    public AuthorizeAttributeManager() : base(typeof(CustomAuthorizeFilterManager))
    {
    }
}

public class CustomAuthorizeFilterManager : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext filterContext)
    {
        var role = filterContext.HttpContext.Session.GetObjectFromJson<Manager>("users");
        if (role == null)
        {
            Console.WriteLine("Error: users is null");
            filterContext.Result = new RedirectToActionResult("ManagerLogin", "Manager", null);
            return;
        }
        Console.WriteLine($"users: {JsonConvert.SerializeObject(role)}");
    }
}
public class CustomExceptionFilter :ExceptionFilterAttribute, IAsyncExceptionFilter //handle exceptions asynchronously
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        if (context.Exception is NullReferenceException)
        {
            // Handle NullReferenceException
            context.Result = new BadRequestObjectResult("Null reference error occurred.");
        }
        else if (context.Exception is InvalidOperationException)
        {
            // Handle InvalidOperationException
            context.Result = new BadRequestObjectResult("Invalid operation error occurred.");
        }
        else if (context.Exception is DbUpdateConcurrencyException)
        {
            // Handle DbUpdateConcurrencyException
            context.Result = new BadRequestObjectResult("Concurrency error occurred.");
        }
        else
        {
            // Handle other exceptions
            context.Result = new BadRequestObjectResult("An error occurred.");
        }

        context.ExceptionHandled = true;
        await Task.CompletedTask;
    }
}
public class ActionFilter : IResultFilter
{
    private readonly ILogger _logger;

    public ActionFilter(ILogger<ActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        
        var Controller = context.RouteData.Values["controller"];
    var action = context.RouteData.Values["action"];

    _logger.LogInformation("Executing Result ", action, Controller);
    }
    public void OnResultExecuted(ResultExecutedContext context)
        {
           var Controller = context.RouteData.Values["controller"];
    var action = context.RouteData.Values["action"];

    _logger.LogInformation("Executed Result ", action, Controller);
}
}
//custom exception or application exception
public class EmployeeAlreadyExistsException : Exception
{
    public EmployeeAlreadyExistsException(string message) : base(message)
    {
    }
}
 