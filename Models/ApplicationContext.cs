using Microsoft.EntityFrameworkCore;
using Notie.Models;
public class ApplicationContext : DbContext
{
    public DbSet<TaskModel> Tasks {get;set;} = null!;
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlite(@"Data Source=NotieApp.db");
    // }


}