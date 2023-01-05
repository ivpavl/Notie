using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Notie.Models;
using Microsoft.EntityFrameworkCore;

namespace Notie.Controllers;

public class HomeController : Controller
{


    private static List<TaskModel> TestTasks = new List<TaskModel> 
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
    [HttpGet]
    public IActionResult Tasks()
    {
        var allTasks = _dbContext.Tasks.ToList();

        return View(allTasks);
    }

    [HttpPost]
    public IActionResult DeleteTask(string taskIdRaw)
    {   
        Console.WriteLine(taskIdRaw);
        if (taskIdRaw is not null)
        { 
            int taskId = Convert.ToInt32(taskIdRaw);
            TaskModel deleteTask = _dbContext.Tasks.Find(taskId)!;
            if (deleteTask is not null)
            {
                _dbContext.Tasks.Remove(deleteTask);
                _dbContext.SaveChanges();

            }
        }

        List<TaskModel> allTasks = _dbContext.Tasks.ToList();

        return View("Tasks", allTasks);
    }

    [HttpPost]
    public IActionResult AddTask(TaskModel task)
    {   
        if (task is not null)
        {
            _dbContext.Tasks.Add(task);
            _dbContext.SaveChanges();
        }
        List<TaskModel> allTasks = _dbContext.Tasks.ToList();

        return View("Tasks", allTasks);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
