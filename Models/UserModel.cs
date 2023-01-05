namespace Notie.Models;

public class UserModel
{
    public int Id {get; set;}

    private string _name = "User";
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
