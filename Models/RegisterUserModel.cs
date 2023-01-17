using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Notie.Models;

public class RegisterUserModel
{
    [Required(ErrorMessage = "Не указано имя")]
    [StringLength(16, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 16 символов")]
    [Remote(action: "CheckNameAvailability", controller: "Auth", ErrorMessage = "Имя уже используеться")]
    public string? Name {get; set;}

    [Required (ErrorMessage = "Не введен пароль")]
    public string? Password {get; set;}

    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string? PasswordConfirm { get; set; }
}


