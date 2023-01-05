using Microsoft.EntityFrameworkCore;
using Notie.Models;
public class ApplicationContext : DbContext
{
    public DbSet<TaskModel> Tasks {get;set;} = null!;
    public DbSet<UserModel> Users {get;set;} = null!;
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

}