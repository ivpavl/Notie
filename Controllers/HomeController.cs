using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notie.Models;
using Notie.Controllers.Common;


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
    public IActionResult AdminPage() => View();


    public IActionResult Tasks()
    {
        UserModel currentUser = UserHelper.GetLoggedUserModel(HttpContext, _dbContext);

        var allTasks = _dbContext.Tasks
            .Where(t => t.User == currentUser)
            .ToList();

        return View(allTasks);
    }

    [HttpPost]
    public IActionResult DeleteTask(string taskIdRaw)
    {   
        if (taskIdRaw is not null)
        { 
            try
            {
                int taskId = Convert.ToInt32(taskIdRaw);
                TaskModel deleteTask = _dbContext.Tasks.Find(taskId)!;
                if (deleteTask is not null)
                {
                    _dbContext.Tasks.Remove(deleteTask);
                    _dbContext.SaveChanges();
                }
            }
            catch
            {
                // Log                
            }
        }

        List<TaskModel> allTasks = _dbContext.Tasks.ToList();

        return View("Tasks", allTasks);
    }

    [HttpPost] 
    public IActionResult AddTask(TaskModel task)
    {   
        UserModel currentUser = UserHelper.GetLoggedUserModel(HttpContext, _dbContext);
        TaskModel newTask = new TaskModel {
            Name = task.Name,
            Description = task.Description,
            User = currentUser
            };

        _dbContext.Tasks.Add(newTask);
        _dbContext.SaveChanges();

        return Redirect("Tasks");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}
