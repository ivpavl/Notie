namespace Notie.Models;

public class TaskModel
{
    public int Id { get; set;}
    public int UserId { get; set;}

    private string _name = "Task";
    public string Name {
        get
        {
            return _name;
        }
        set 
        {
            if (value is not null)
                _name = value;
        }
    }

    public string? Description {get; set;}
}
