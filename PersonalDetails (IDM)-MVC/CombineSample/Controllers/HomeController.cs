using Microsoft.AspNetCore.Mvc;
using MyAlias = CombineSample.Models;
using CombineSample.Data;
using Microsoft.AspNetCore.Diagnostics;
namespace CombineSample.Controllers;
[TypeFilter(typeof(LoggingActionFilter))] //  action filter at the controller level
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    public IActionResult Index()
    {
        _logger.LogInformation("excecution of index action method");
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }
   [Route("Error")]
    public IActionResult Error()
    {
        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;
        var errorModel = new MyAlias.ErrorViewModel
        {
            ErrorMessage = "An error occurred while processing your request.",
            ErrorDetails = exception?.Message
        };
        return View(errorModel);
    }
}