namespace Domain.Exceptions;

public class UserNotAvailableException:Exception
{
    public UserNotAvailableException():base("The user is not available")
    { }
}