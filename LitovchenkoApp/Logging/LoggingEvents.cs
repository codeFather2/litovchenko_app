namespace LitovchenkoApp.Logging;

public static class LoggingEvents
{
    public static EventId Error = new(101, "Error");
    public static EventId DbCrud = new(102, "DbCrud");
}