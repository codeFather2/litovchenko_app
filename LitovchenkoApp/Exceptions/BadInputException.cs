namespace LitovchenkoApp.Exceptions;

public class BadInputException : Exception
{
    private readonly string message;
    public BadInputException(string message)
    {
        this.message = message;
    }
    public override string Message => message;
}