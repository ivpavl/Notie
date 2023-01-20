using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Notie.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Notie.Controllers.Common;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Notie.Controllers;
public class AuthController : Controller
{
    private readonly ApplicationContext _dbContext;
    public AuthController(ApplicationContext context)
    {
        _dbContext = context;
    }
    
    public IActionResult Index() => RedirectToAction("Login");

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity!.IsAuthenticated)
            return RedirectToAction("Index", "Home");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginUserModel model)
    {
        if (ModelState.IsValid)
        {
            var userSearchResult = _dbContext.Users
                .Where(x => model.Name == x.Name && model.Password == x.Password)
                    .Include(u => u.Role)
                .FirstOrDefault();

            if (userSearchResult is not null)
            {
                await SignInUser(userSearchResult, HttpContext);
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
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

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

            Role userRole = _dbContext.Roles.FirstOrDefault(r => r.Name == "User")!;
            if (userRole is null)
            {
                userRole = new Role { Name = "User" };
                _dbContext.Roles.Add(userRole);
            }

            UserModel user = new UserModel 
            {
                Name = model.Name!,
                Password = model.Password!,
                Role = userRole
            };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            await SignInUser(user, HttpContext);
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
    
    public async Task<IActionResult> SetRole(string roleName)
    {
        Role userRole = _dbContext.Roles.FirstOrDefault(r => r.Name == roleName)!;
        if (userRole is null)
        {
            userRole = new Role { Name = roleName };
            _dbContext.Roles.Add(userRole);
        }
        UserModel currentUser = UserHelper.GetLoggedUserModel(HttpContext, _dbContext);
        currentUser.Role = userRole;

        _dbContext.SaveChanges();

        await SignInUser(currentUser, HttpContext);
        return RedirectToAction("Index", "Home");
    }
        // TODO ДОПИСАТЬ ЭТУ КОПИПАСТУ


    // private IActionResult AddToDB()
    // {

    //     Role Admin = new Role { Name = "Admin" };
    //     Role User = new Role { Name = "User" };

    //     var _dbContextMOCK = new List<UserModel> {
    //         new UserModel {Name = "123", Password = "123", Role = Admin},
    //         new UserModel {Name = "Vasiliy", Password = "123", Role = Admin},
    //         new UserModel {Name = "1", Password = "1", Role = User}
    //     };
    //     // _dbContext.Roles.AddRange(Admin, User);
    //     _dbContext.Users.AddRange(_dbContextMOCK);

    //     _dbContext.SaveChanges();

    //     return View("Index");

    // }



    private async Task SignInUser(UserModel user, HttpContext httpContext)
    {
        var claims = new List<Claim> 
        { 
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
        };
        var claimsIdentity = new ClaimsIdentity(claims, 
            CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        await httpContext.SignInAsync(claimsPrincipal);
    }
}