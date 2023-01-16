using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Notie.Models;



using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Notie.Controllers;
public class AuthController : Controller
{
    private readonly ApplicationContext _dbContext;
    public AuthController(ApplicationContext context)
    {
        _dbContext = context;
    }
    public IActionResult Index() {
        var user = HttpContext.User.Identity;
        if (user.IsAuthenticated)
            TempData["Message"] = $"Hello, {user.Name}";
        return View();
    } 
    

    [HttpPost]
    public async Task<IActionResult> Login(UserModel user)
    {

        var userSearchResult = _dbContext.Users
            .Where(x => user.Name == x.Name && user.Password == x.Password)
                .Include(u => u.Role)
            .First();

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

            return View("Index");
        }
        else
        {
            TempData["Message"] = "Incorrect username or password";
            return View("Index");
        }
    }
    
    
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("Index");
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
        _dbContext.Roles.AddRange(Admin, User);
        _dbContext.Users.AddRange(_dbContextMOCK);

        _dbContext.SaveChanges();

        return View("Index");

    }
    
}