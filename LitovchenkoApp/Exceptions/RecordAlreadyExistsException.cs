namespace LitovchenkoApp.Exceptions;

public class RecordAlreadyExistsException : Exception
{
    private readonly string message;
    public RecordAlreadyExistsException(string message)
    {
        this.message = message;
    }
    public override string Message => message;
}