using CombineSample.Models;
using Microsoft.AspNetCore.Mvc;
public class ErrorController : Controller
{
     private readonly ILogger<ErrorController> _logger;
    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }
[HttpGet("Error/Errors/{statusCode:int:min(100):max(999)}")]
public IActionResult Errors(int statusCode)
{
    try
    {
    var errorViewModel = new ErrorViewModel();

switch (statusCode)
{
    case 404:
        errorViewModel.NullableErrorType = ErrorViewModel.ErrorType.NotFound;
        ViewData["Messagefound"] = "Page not found";
        break; 
    case 403:
        errorViewModel.NullableErrorType = ErrorViewModel.ErrorType.Forbidden;
        ViewData["Messageaccess"] = "Access forbidden.";
        break;
    case 500:
        errorViewModel.NullableErrorType = ErrorViewModel.ErrorType.InternalServerError;
        ViewData["Messageserver"] = "Internal server error.";
        break;
    default:
        errorViewModel.NullableErrorType = null;
        ViewData["Message"] = "An error occurred.";
        break;
}
        return View("Errors", errorViewModel);
    }
    catch (Exception ex)
    {
        
        _logger.LogError(ex, "An exception occurred in the Errors action.");

        
        Response.StatusCode = 500;
        ViewData["Message"] = "An unexpected error occurred.";

        return View("Errors");
    }
}
}

