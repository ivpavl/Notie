using Notie.Models;

namespace Notie.Controllers.Common;

public class AuthHelper
{
    public static UserModel GetLoggedUserModel(HttpContext HttpContext, ApplicationContext dbContext)
    {
        bool isLogged = HttpContext.User.Identity!.IsAuthenticated;
        if (isLogged is false)
            return null!;

        var userName = HttpContext.User.Identity?.Name;
        UserModel user = dbContext.Users.FirstOrDefault(u => u.Name == userName)!;

        if (user is null)
            throw new Exception("Already logged user is not found in DB");

        return user;
    }

}