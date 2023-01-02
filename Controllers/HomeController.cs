using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Notie.Models;
using Microsoft.EntityFrameworkCore;

namespace Notie.Controllers;

public class HomeController : Controller
{


    List<TaskModel> TestTasks = new List<TaskModel> 
    {
        new TaskModel{Name = "Hey1", Description = "hehe"},
        new TaskModel{Name = "Hey2", Description = "hehe"},
        new TaskModel{Name = "Hey3", Description = "hehe"},
    };
    // List<TaskModel> TestTasks = new List<TaskModel> 
    // {
    //     new TaskModel(25, "cha", "he"),
    //     new TaskModel(25, "bha", "he22"),
    //     new TaskModel(5, "aha", "he8987")
    // };
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationContext _dbContext;
    public HomeController(ILogger<HomeController> logger, ApplicationContext context)
    {
        _dbContext = context;
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
        // TaskModel task1 = new TaskModel{Name = "Hey1", Description = "hehe"};
        // _dbContext.Tasks.Add(task1);
        // _dbContext.Tasks.Add(new TaskModel{Name="Hey", Description="heh"});
        // _dbContext.SaveChanges();
        var tasks = _dbContext.Tasks.ToList();
        // Console.WriteLine(tasks);
        return View(tasks);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
