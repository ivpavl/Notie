using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notie.Models;

public class LoginUserModel
{
    [Required (ErrorMessage = "Не указано имя")]
    public string? Name {get; set;}

    [Required (ErrorMessage = "Не введен пароль")]
    public string? Password {get; set;}

}


