namespace Domain.Exceptions;

public class InvalidUserCredentialsException:Exception
{
    public InvalidUserCredentialsException()
        : base("Incorrect email or password")
    { }
}