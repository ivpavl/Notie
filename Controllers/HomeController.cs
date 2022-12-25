using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Notie.Models;

namespace Notie.Controllers;

public class HomeController : Controller
{

    private readonly ILogger<HomeController> _logger;

    List<TaskModel> TestTasks = new List<TaskModel> 
    {
        new TaskModel(25, "cha", "he"),
        new TaskModel(25, "bha", "he22"),
        new TaskModel(5, "aha", "he8987")
    };
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult Tasks()
    {
        return View(TestTasks);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
