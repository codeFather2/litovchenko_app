namespace LitovchenkoApp.Exceptions;

public class ExceptionResponse
{
    public string Type { get; set; } = "Error";
    public string Message { get; set; } = "Something went wrong";
    public string StackTrace { get; set; } = string.Empty;

    public ExceptionResponse(Exception ex, bool isDebug = false)
    {
        if (isDebug)
        {
            Type = ex.GetType().Name;
            Message = ex.Message;
            StackTrace = ex.ToString();
        }
    }

    public ExceptionResponse()
    {
    }
}