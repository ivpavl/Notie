using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;



namespace Notie.Models;

public class UserModel
{
    public int Id {get; set;}
    public string Name {get; set;} = null!;
    public string Password {get; set;} = null!;

    public int RoleId {get; set;}
    public Role Role {get; set;}
    
    public ICollection<TaskModel> Tasks { get; set; }
}

[Index("Name", IsUnique = true)]
public class Role 
{
    public int Id {get; set;}
    public string Name {get; set;}
    public ICollection<UserModel> Users { get; set; }
}

