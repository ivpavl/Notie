using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notie.Models;

namespace Notie.Controllers;

[Authorize]
public class HomeController : Controller
{


    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationContext _dbContext;
    public HomeController(ILogger<HomeController> logger, ApplicationContext context)
    {
        _dbContext = context;
        _logger = logger;
    }

    [AllowAnonymous]
    public IActionResult Index() => View();


    [Authorize(Roles = "Admin")]
    public IActionResult Privacy() => View();


    public IActionResult Tasks()
    {
        UserModel currentUser = GetLoggedUserModel();

        var allTasks = _dbContext.Tasks
            .Where(t => t.User == currentUser)
            .ToList();

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
    // TODO Переделать проверку модели таски. Добавить соответственно новую модель для проверки
    public IActionResult AddTask(TaskModel task)
    {   
        UserModel currentUser = GetLoggedUserModel();
        TaskModel newTask = new TaskModel {Name = task.Name, Description = task.Description, User = currentUser};

        // if (ModelState.IsValid)
        // {
        _dbContext.Tasks.Add(newTask);
        _dbContext.SaveChanges();
        // }
        // List<TaskModel> allTasks = _dbContext.Tasks.ToList();

        return Redirect("Tasks");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }




    [NonAction]
    private UserModel GetLoggedUserModel()
    {
        var userName = HttpContext.User.Identity?.Name;
        UserModel user = _dbContext.Users.FirstOrDefault(u => u.Name == userName)!;

        if (user is null)
            throw new Exception("Already logged user is not found in DB");

        return user;
    }
}
