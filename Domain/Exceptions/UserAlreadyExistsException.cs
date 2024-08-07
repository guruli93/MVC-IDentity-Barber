namespace Domain.Exceptions;

public class UserAlreadyExistsException:Exception
{
    public UserAlreadyExistsException():base("User already Exist")
    { }
}