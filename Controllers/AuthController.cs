using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Notie.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Notie.Controllers.Common;

namespace Notie.Controllers;
public class AuthController : Controller
{
    private readonly ApplicationContext _dbContext;
    public AuthController(ApplicationContext context)
    {
        _dbContext = context;
    }

    
    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity!.IsAuthenticated)
            return RedirectToAction("Index", "Home");
        return View();
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> Login(LoginUserModel model)
    {
        if (ModelState.IsValid)
        {
            // TODO Если пользователей совсем нет, будет ошибка
            // TODO Если пользователей совсем нет, будет ошибка
            // TODO Если пользователей совсем нет, будет ошибка
            var userSearchResult = _dbContext.Users
                .Where(x => model.Name == x.Name && model.Password == x.Password)
                    .Include(u => u.Role)
                .FirstOrDefault();

            if (userSearchResult is not null)
            {
                var claims = new List<Claim> 
                { 
                    new Claim(ClaimTypes.Name, userSearchResult.Name),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, userSearchResult.Role.Name)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                
                return RedirectToAction("Index", "Home");
            }
        }

        TempData["Message"] = "Incorrect username or password";
        return View();
        
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity!.IsAuthenticated)
            return RedirectToAction("Index", "Home");
        return View();
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterUserModel model)
    {
        if (ModelState.IsValid)
        {
            bool nameAlreadyExists = _dbContext.Users.Any(u => u.Name == model.Name);
            if (nameAlreadyExists)
            {
                ModelState.AddModelError("Name", "Имя уже существует");
                return View();
            }
            // TODO ИСПРАВИТЬ КОСТЫЛЬ
            // TODO ИСПРАВИТЬ КОСТЫЛЬ
            // TODO ИСПРАВИТЬ КОСТЫЛЬ
            Role userRole = _dbContext.Roles.FirstOrDefault(r => r.Name == "User") ?? new Role {Name = "User"};


            UserModel user = new UserModel 
            {
                Name = model.Name,
                Password = model.Password,
                Role = userRole
            };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var claims = new List<Claim> 
            { 
                new Claim(ClaimTypes.Name, model.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "User")
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);

            return RedirectToAction("Index", "Home");
        }
        return View();
    }
    



    
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    

    public IActionResult CheckNameAvailability(string name)
    {
        bool nameAlreadyExists = _dbContext.Users.Any(u => u.Name == name);
        if (nameAlreadyExists)
        {
            return Json(false);
        }
        else
        {
            return Json(true);
        }
    }
    public IActionResult AddToDB()
    {

        Role Admin = new Role { Name = "Admin" };
        Role User = new Role { Name = "User" };

        var _dbContextMOCK = new List<UserModel> {
            new UserModel {Name = "123", Password = "123", Role = Admin},
            new UserModel {Name = "Vasiliy", Password = "123", Role = Admin},
            new UserModel {Name = "1", Password = "1", Role = User}
        };
        // _dbContext.Roles.AddRange(Admin, User);
        _dbContext.Users.AddRange(_dbContextMOCK);

        _dbContext.SaveChanges();

        return View("Index");

    }
    
}