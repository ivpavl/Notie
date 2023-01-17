using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
 
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();// добавляем IDistributedMemoryCache
builder.Services.AddSession();  // добавляем сервисы сессии

// builder.Services.AddAuthentication("Bearer").AddJwtBearer(options => {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         // указывает, будет ли валидироваться издатель при валидации токена
//         ValidateIssuer = true,
//         // строка, представляющая издателя
//         ValidIssuer = AuthOptions.ISSUER,
//         // будет ли валидироваться потребитель токена
//         ValidateAudience = true,
//         // установка потребителя токена
//         ValidAudience = AuthOptions.AUDIENCE,
//         // будет ли валидироваться время существования
//         ValidateLifetime = true,
//         // установка ключа безопасности
//         IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
//         // валидация ключа безопасности
//         ValidateIssuerSigningKey = true,
//     };
// });
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    config =>
    {
        config.LoginPath = "/Auth/Login";
        config.AccessDeniedPath = "/Home/Index";
        // config.ExpireTimeSpan = new TimeSpan (0,0,30);
    }
);
builder.Services.AddAuthorization();

builder.Services.AddDbContext<ApplicationContext>(options => {
    // options.UseSqlite(
    //     builder.Configuration.GetConnectionString("DefaultConnectionSQLite")
    //     );
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnectionMySQL"), 
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnectionMySQL"))
        );
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}




app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
// app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
// public class AuthOptions
// {
//     public const string ISSUER = "Notie"; // издатель токена
//     public const string AUDIENCE = "localhost"; // потребитель токена
//     const string KEY = "key!@#123KEYKEYKEY2828";   // ключ для шифрации
//     public static SymmetricSecurityKey GetSymmetricSecurityKey() => 
//         new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
// }